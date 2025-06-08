using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;
using X.PagedList;
using X.PagedList.Extensions;
using LaptopShop.Areas.Model;

namespace LaptopShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLaptopsController : Controller
    {
        private readonly ShopLaptopContext _context;

        public AdminLaptopsController(ShopLaptopContext context)
        {
            _context = context;
        }



        //GET: Admin/AdminLaptops
        public async Task<IActionResult> Index(string searchString, string thuonghieu, int? minPrice, int? maxPrice, int? page, string sortColumn, string sortOrder)
        {
            var thuongHieuList = _context.ThuongHieus
                .Select(th => new { th.IdThuongHieu, th.TenThuongHieu })
                 .ToList();

            // Thêm mục "Tất cả" vào đầu danh sách
            thuongHieuList.Insert(0, new { IdThuongHieu = "", TenThuongHieu = "--Thương hiệu--" });

            ViewData["ThuongHieuList"] = new SelectList(thuongHieuList, "IdThuongHieu", "TenThuongHieu", thuonghieu);

            ViewData["searchString"] = searchString;
            ViewData["thuonghieu"] = thuonghieu;
            ViewData["minPrice"] = minPrice;
            ViewData["maxPrice"] = maxPrice;
            ViewData["sortColumn"] = sortColumn;
            ViewData["sortOrder"] = sortOrder;

            var laptopsQuery = _context.Laptops
                .Include(l => l.IdLoaiNavigation)
                .Include(l => l.IdThongTinNavigation)
                .Include(l => l.IdThuongHieuNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                laptopsQuery = laptopsQuery.Where(l => l.TenLapTop.Contains(searchString) ||
                                                       l.IdThuongHieuNavigation.TenThuongHieu.Contains(searchString) ||
                                                       l.IdLoaiNavigation.TenLoai.Contains(searchString));
            }

           
            if (!string.IsNullOrEmpty(thuonghieu))
            {
                laptopsQuery = laptopsQuery.Where(l => l.IdThuongHieu == thuonghieu);
            }

            if (minPrice.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.GiaBan >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.GiaBan <= maxPrice.Value);
            }
            // Thêm sắp xếp theo cột được chọn
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "TenLapTop":
                        laptopsQuery = (sortOrder == "asc")
                            ? laptopsQuery.OrderBy(l => l.TenLapTop)
                            : laptopsQuery.OrderByDescending(l => l.TenLapTop);
                        break;
                    case "GiaBan":
                        laptopsQuery = (sortOrder == "asc")
                            ? laptopsQuery.OrderBy(l => l.GiaBan)
                            : laptopsQuery.OrderByDescending(l => l.GiaBan);
                        break;
                    case "ThuongHieu":
                        laptopsQuery = (sortOrder == "asc")
                            ? laptopsQuery.OrderBy(l => l.IdThuongHieuNavigation.TenThuongHieu)
                            : laptopsQuery.OrderByDescending(l => l.IdThuongHieuNavigation.TenThuongHieu);
                        break;
                    case "Loai":
                        laptopsQuery = (sortOrder == "asc")
                            ? laptopsQuery.OrderBy(l => l.IdLoaiNavigation.TenLoai)
                            : laptopsQuery.OrderByDescending(l => l.IdLoaiNavigation.TenLoai);
                        break;
                    case "SoLuong":
                        laptopsQuery = (sortOrder == "asc")
                            ? laptopsQuery.OrderBy(l => l.SoLuong)
                            : laptopsQuery.OrderByDescending(l => l.SoLuong);
                        break;
                }
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var laptopsPaged = laptopsQuery.ToPagedList(pageNumber, pageSize);

            return View(laptopsPaged);
        }



        // GET: Admin/AdminLaptops/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .Include(l => l.IdLoaiNavigation)
                .Include(l => l.IdThongTinNavigation)
                .Include(l => l.IdThuongHieuNavigation)
                .FirstOrDefaultAsync(m => m.IdLaptop == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Admin/AdminLaptops/Create

        public IActionResult Create()
        {
            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai");
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu");

            var model = new LaptopCreateViewModel
            {
                Laptop = new Laptop(),                          // Bắt buộc
                ThongTin = new ThongTinChiTiet()               // Bắt buộc
               
            };

            return View(model);
        }



        // POST: Admin/AdminLaptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LaptopCreateViewModel model)
        {
            ModelState.Remove("Laptop.IdLaptop"); // nếu cần bỏ qua Id tự sinh

            if (ModelState.IsValid)
            {
                // Chuẩn hóa dữ liệu ThongTinChiTiet
                var tenLaptopChuanHoa = Normalize(model.Laptop.TenLapTop);
                var cpu = Normalize(model.ThongTin.Cpu);
                var ram = Normalize(model.ThongTin.Ram);
                var ocung = Normalize(model.ThongTin.Ocung);
                var manhinh = Normalize(model.ThongTin.ManHinh);
                var dohoa = Normalize(model.ThongTin.DoHoa);
                var cong = Normalize(model.ThongTin.CongGiaoTiep);
                var hedh = Normalize(model.ThongTin.HeDieuHanh);
                var pin = Normalize(model.ThongTin.Pin);
                var tl = Normalize(model.ThongTin.TrongLuong);

                // Tìm cấu hình đã có
                var thongTinDaCo = _context.ThongTinChiTiets
                    .AsEnumerable() // CHUYỂN VỀ LINQ TRÊN RAM
                    .FirstOrDefault(t =>
                        Normalize(t.Cpu) == cpu &&
                        Normalize(t.Ram) == ram &&
                        Normalize(t.Ocung) == ocung &&
                        Normalize(t.ManHinh) == manhinh &&
                        Normalize(t.DoHoa) == dohoa &&
                        Normalize(t.CongGiaoTiep) == cong &&
                        Normalize(t.HeDieuHanh) == hedh &&
                        Normalize(t.Pin) == pin &&
                        Normalize(t.TrongLuong) == tl
                    );

                

                if (thongTinDaCo != null)
                {
                    model.Laptop.IdThongTin = thongTinDaCo.IdThongTin;
                }
                else
                {
                    // Tạo mới cấu hình
                    model.ThongTin.IdThongTin = Guid.NewGuid().ToString();
                    _context.ThongTinChiTiets.Add(model.ThongTin);
                    await _context.SaveChangesAsync();

                    model.Laptop.IdThongTin = model.ThongTin.IdThongTin;
                }

                var normalizedTenLaptop = Normalize(model.Laptop.TenLapTop);

                var laptopsWithSameThongTin = await _context.Laptops
    .Where(l => l.IdThongTin == model.Laptop.IdThongTin)
    .ToListAsync();  // async, trả về List<Laptop>

                var laptopTrung = laptopsWithSameThongTin
                    .FirstOrDefault(l => Normalize(l.TenLapTop) == normalizedTenLaptop);

                if (laptopTrung != null)
                {
                    ModelState.AddModelError("", "Laptop này đã có trong kho!");
                    // Trả lại view với dữ liệu hiện tại + thông báo
                    ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", model.Laptop.IdLoai);
                    ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", model.Laptop.IdThuongHieu);
                    return View(model);
                }

                // Xử lý ảnh
                List<string> finalImages = new();
                if (model.HinhAnhMoi != null)
                {
                    foreach (var file in model.HinhAnhMoi)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hinh", "laptop", fileName);

                            if (!System.IO.File.Exists(filePath))
                            {
                                using var stream = new FileStream(filePath, FileMode.Create);
                                await file.CopyToAsync(stream);
                            }

                            finalImages.Add(fileName);
                        }
                    }
                }

                model.Laptop.HinhAnh = string.Join(",", finalImages);

                _context.Add(model.Laptop);
                await _context.SaveChangesAsync();
                TempData["createmess"] = "Sản phẩm đã được thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", model.Laptop.IdLoai);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", model.Laptop.IdThuongHieu);
            
            return View(model);
        }


        // GET: Admin/AdminLaptops/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null) return NotFound();

            var thongTin = await _context.ThongTinChiTiets
                .FirstOrDefaultAsync(t => t.IdThongTin == laptop.IdThongTin);

            var model = new LaptopEditViewModel
            {
                Laptop = laptop,
                ThongTin = thongTin ?? new ThongTinChiTiet(),
                OldImages = string.IsNullOrEmpty(laptop.HinhAnh) ? new List<string>() : laptop.HinhAnh.Split(',').ToList()
            };

            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", laptop.IdLoai);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", laptop.IdThuongHieu);

            return View(model);
        }



        // POST: Admin/AdminLaptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    string id,
    LaptopEditViewModel model,
    List<IFormFile> HinhAnhMoi,
    string[]? DeletedImages,
    string[]? OldImages)
        {
            if (id != model.Laptop.IdLaptop) return NotFound();

            DeletedImages ??= new string[0];
            OldImages ??= new string[0];

            if (ModelState.IsValid)
            {
                try
                {
                    // Chuẩn hóa cấu hình (bạn tự viết hàm)
                    NormalizeConfig(model.ThongTin);

                    // Tìm cấu hình trùng
                    var existingConfig = _context.ThongTinChiTiets.AsEnumerable()
                        .FirstOrDefault(t => IsSameConfig(t, model.ThongTin));

                    if (existingConfig != null)
                    {
                        model.Laptop.IdThongTin = existingConfig.IdThongTin;
                    }
                    else
                    {
                        model.ThongTin.IdThongTin = Guid.NewGuid().ToString();
                        _context.ThongTinChiTiets.Add(model.ThongTin);
                        await _context.SaveChangesAsync();
                        model.Laptop.IdThongTin = model.ThongTin.IdThongTin;
                    }

                    // Kiểm tra trùng laptop tên + cấu hình IdThongTin (ngoại trừ chính laptop này)
                    var normalizedTenLaptop = Normalize(model.Laptop.TenLapTop);

                    bool isDuplicate = _context.Laptops
                        .AsEnumerable() // ⚠ Chuyển về client
                        .Any(l => l.IdLaptop != model.Laptop.IdLaptop
                        && l.IdThongTin == model.Laptop.IdThongTin
                        && Normalize(l.TenLapTop) == normalizedTenLaptop);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("", "Đã tồn tại laptop cùng tên và cấu hình trong kho!");
                        ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", model.Laptop.IdLoai);
                        ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", model.Laptop.IdThuongHieu);
                        model.OldImages = OldImages.ToList();
                        return View(model);
                    }

                    // Xử lý ảnh
                    List<string> finalImages = new List<string>(OldImages);

                    foreach (var deletedImage in DeletedImages)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hinh", "laptop", deletedImage);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    finalImages = finalImages.Where(img => !DeletedImages.Contains(img)).ToList();

                    if (HinhAnhMoi != null && HinhAnhMoi.Count > 0)
                    {
                        foreach (var file in HinhAnhMoi)
                        {
                            if (file.Length > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hinh", "laptop", fileName);

                                if (!System.IO.File.Exists(filePath))
                                {
                                    using var stream = new FileStream(filePath, FileMode.Create);
                                    await file.CopyToAsync(stream);
                                }
                                finalImages.Add(fileName);
                            }
                        }
                    }

                    model.Laptop.HinhAnh = string.Join(",", finalImages);

                    //do EF Core đang track một thực thể Laptop với cùng IdLaptop khi bạn gọi _context.Update(model.Laptop) nên không thể dùng cách cũ
                    var laptopToUpdate = await _context.Laptops.FindAsync(model.Laptop.IdLaptop);
                    if (laptopToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật từng trường cần thiết (hoặc dùng SetValues):
                    laptopToUpdate.TenLapTop = model.Laptop.TenLapTop;
                    laptopToUpdate.IdLoai = model.Laptop.IdLoai;
                    laptopToUpdate.IdThuongHieu = model.Laptop.IdThuongHieu;
                    laptopToUpdate.IdThongTin = model.Laptop.IdThongTin;
                    laptopToUpdate.SoLuong = model.Laptop.SoLuong;
                    laptopToUpdate.GiaBan = model.Laptop.GiaBan;
                    laptopToUpdate.HinhAnh = model.Laptop.HinhAnh;

                    

                    await _context.SaveChangesAsync();
                    TempData["editmess"] = "Cập nhập sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(model.Laptop.IdLaptop)) return NotFound();
                    else throw;
                }
            }

            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", model.Laptop.IdLoai);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", model.Laptop.IdThuongHieu);
            model.OldImages = OldImages.ToList();
            TempData["editmess"] = "Sản phẩm đã được sửa thành công!";
            return View(model);
        }





        // GET: Admin/AdminLaptops/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .Include(l => l.IdLoaiNavigation)
                .Include(l => l.IdThongTinNavigation)
                .Include(l => l.IdThuongHieuNavigation)
                .FirstOrDefaultAsync(m => m.IdLaptop == id);

           
            if (laptop == null)
            {
                return NotFound();
            }
            
           


            return View(laptop);
        }

        // POST: Admin/AdminLaptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }

            
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
            }
            TempData["deletemess"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }
        private string Normalize(string? input)
        {
            return (input ?? "").Trim().ToLower().Replace(" ", "");
        }
        public void NormalizeConfig(ThongTinChiTiet config)
        {
            if (config == null) return;

            config.Cpu = Normalize(config.Cpu);
            config.Ram = Normalize(config.Ram);
            config.Ocung = Normalize(config.Ocung);
            config.ManHinh = Normalize(config.ManHinh);
            config.DoHoa = Normalize(config.DoHoa);
            config.CongGiaoTiep = Normalize(config.CongGiaoTiep);
            config.HeDieuHanh = Normalize(config.HeDieuHanh);
            config.Pin = Normalize(config.Pin);
            config.TrongLuong = Normalize(config.TrongLuong);
        }
        
        /// <summary>
        /// So sánh 2 cấu hình chi tiết có giống nhau hay không.
        /// So sánh từng trường đã được chuẩn hóa.
        /// </summary>
        public bool IsSameConfig(ThongTinChiTiet c1, ThongTinChiTiet c2)
        {
            if (c1 == null || c2 == null) return false;

            return string.Equals(Normalize(c1.Cpu), Normalize(c2.Cpu))
                && string.Equals(Normalize(c1.Ram), Normalize(c2.Ram))
                && string.Equals(Normalize(c1.Ocung), Normalize(c2.Ocung))
                && string.Equals(Normalize(c1.ManHinh), Normalize(c2.ManHinh))
                && string.Equals(Normalize(c1.DoHoa), Normalize(c2.DoHoa))
                && string.Equals(Normalize(c1.CongGiaoTiep), Normalize(c2.CongGiaoTiep))
                && string.Equals(Normalize(c1.HeDieuHanh), Normalize(c2.HeDieuHanh))
                && string.Equals(Normalize(c1.Pin), Normalize(c2.Pin))
                && string.Equals(Normalize(c1.TrongLuong), Normalize(c2.TrongLuong));
        }

        private bool LaptopExists(string id)
        {
            return _context.Laptops.Any(e => e.IdLaptop == id);
        }
    }
}
