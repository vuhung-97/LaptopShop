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
                laptopsQuery = laptopsQuery.Where(l => l.IdThuongHieu.ToString() == thuonghieu);
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

            int pageSize = 10;
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
            ViewData["IdThongTin"] = _context.ThongTinChiTiets
       .Select(t => new SelectListItem { Value = t.IdThongTin, Text = $"{t.Cpu} | {t.Ram} | {t.Ocung}| {t.DoHoa}| {t.ManHinh}| {t.HeDieuHanh}" })
       .ToList();
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu");
            return View();
        }

        // POST: Admin/AdminLaptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("IdThuongHieu,TenLapTop,GiaBan,SoLuong,IdThongTin,IdLoai")] Laptop laptop, List<IFormFile> HinhAnhMoi)
        {
            ModelState.Remove("IdLaptop");
            if (ModelState.IsValid)
            {
                try
                {
                    List<string> finalImages = new List<string>();

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
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }

                                finalImages.Add(fileName);
                            }
                        }
                    }

                    laptop.HinhAnh = string.Join(",", finalImages);

                    _context.Add(laptop);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai");
            ViewData["IdThongTin"] = _context.ThongTinChiTiets
       .Select(t => new SelectListItem { Value = t.IdThongTin, Text = $"{t.Cpu} | {t.Ram} | {t.Ocung}| {t.DoHoa}| {t.ManHinh}| {t.HeDieuHanh}" })
       .ToList();
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu");
            return View(laptop);
        }

        // GET: Admin/AdminLaptops/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "TenLoai", laptop.IdLoai);
            ViewData["IdThongTin"] = _context.ThongTinChiTiets
       .Select(t => new SelectListItem { Value = t.IdThongTin, Text = $"{t.Cpu} | {t.Ram} | {t.Ocung}| {t.DoHoa}| {t.ManHinh}| {t.HeDieuHanh}" })
       .ToList();
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "TenThuongHieu", laptop.IdThuongHieu);
            return View(laptop);
        }
       

        // POST: Admin/AdminLaptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(string id, [Bind("IdLaptop,IdThuongHieu,TenLapTop,GiaBan,HinhAnh,IdThongTin,IdLoai,SoLuong")] Laptop laptop, List<IFormFile> HinhAnhMoi, string[]? DeletedImages, string[]? OldImages)
        {
            if (id != laptop.IdLaptop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Bảo vệ trước Null
                    DeletedImages ??= new string[0];
                    OldImages ??= new string[0];

                    List<string> finalImages = new List<string>(OldImages);

                    if (DeletedImages.Length > 0)
                    {
                        foreach (var deletedImage in DeletedImages)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hinh", "laptop", deletedImage);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        // Xóa ảnh bị xóa khỏi danh sách finalImages
                        finalImages = finalImages.Where(img => !DeletedImages.Contains(img)).ToList();
                    }

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
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }

                                finalImages.Add(fileName);
                            }
                        }
                    }

                    laptop.HinhAnh = string.Join(",", finalImages);

                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.IdLaptop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "IdLoai", laptop.IdLoai);
            ViewData["IdThongTin"] = new SelectList(_context.ThongTinChiTiets, "IdThongTin", "IdThongTin", laptop.IdThongTin);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "IdThuongHieu", laptop.IdThuongHieu);
            return View(laptop);
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

            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(string id)
        {
            return _context.Laptops.Any(e => e.IdLaptop == id);
        }
    }
}
