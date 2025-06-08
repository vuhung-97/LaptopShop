using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;

namespace LaptopShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminThuongHieuxController : Controller
    {
        private readonly ShopLaptopContext _context;

        public AdminThuongHieuxController(ShopLaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminThuongHieux
        public async Task<IActionResult> Index()
        {
            var thuongHieus = _context.ThuongHieus.ToList();

            // Tạo Dictionary chứa IdThuongHieu và số lượng sản phẩm tương ứng
            var soLuongDict = _context.Laptops
                .GroupBy(sp => sp.IdThuongHieu)
                .ToDictionary(g => g.Key, g => g.Count());

            // Truyền vào ViewData
            ViewData["SoLuongSanPham"] = soLuongDict;
            return View(await _context.ThuongHieus.ToListAsync());
        }

        // GET: Admin/AdminThuongHieux/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.IdThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // GET: Admin/AdminThuongHieux/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminThuongHieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu, IFormFile? HinhAnhMoi)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhMoi != null && HinhAnhMoi.Length > 0)
                {
                    // Đường dẫn lưu ảnh
                    string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string thuMucLuuAnh = Path.Combine(wwwRootPath, "hinh", "thuonghieu");

                    // Tạo tên file ngẫu nhiên
                    string tenFileMoi = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhMoi.FileName);
                    string duongDanLuu = Path.Combine(thuMucLuuAnh, tenFileMoi);

                    // Lưu file
                    using (var stream = new FileStream(duongDanLuu, FileMode.Create))
                    {
                        await HinhAnhMoi.CopyToAsync(stream);
                    }

                    // Gán tên file vào thuộc tính Logo
                    thuongHieu.Logo = tenFileMoi;
                }

                _context.Add(thuongHieu);
                await _context.SaveChangesAsync();
                TempData["createmess"] = "Đã thêm mới thương hiệu!";
                return RedirectToAction(nameof(Index));
            }

            return View(thuongHieu);
        }


        // GET: Admin/AdminThuongHieux/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            return View(thuongHieu);
        }

        // POST: Admin/AdminThuongHieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu, IFormFile? HinhAnhMoi)
        {
            if (id != thuongHieu.IdThuongHieu)
                return NotFound();

            var thuongHieuCu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieuCu == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật tên thương hiệu
                    thuongHieuCu.TenThuongHieu = thuongHieu.TenThuongHieu;

                    if (HinhAnhMoi != null && HinhAnhMoi.Length > 0)
                    {
                        // Đường dẫn thư mục lưu ảnh
                        string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string thuMucLuuAnh = Path.Combine(wwwRootPath, "hinh", "thuonghieu");

                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(thuongHieuCu.Logo))
                        {
                            string duongDanAnhCu = Path.Combine(thuMucLuuAnh, thuongHieuCu.Logo);
                            if (System.IO.File.Exists(duongDanAnhCu))
                                System.IO.File.Delete(duongDanAnhCu);
                        }

                        // Lưu ảnh mới
                        string tenFileMoi = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhMoi.FileName);
                        string duongDanAnhMoi = Path.Combine(thuMucLuuAnh, tenFileMoi);

                        using (var stream = new FileStream(duongDanAnhMoi, FileMode.Create))
                        {
                            await HinhAnhMoi.CopyToAsync(stream);
                        }

                        thuongHieuCu.Logo = tenFileMoi;
                    }

                    _context.Update(thuongHieuCu);
                    await _context.SaveChangesAsync();
                    TempData["editmess"] = "Cập nhập thương hiệu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ThuongHieus.Any(e => e.IdThuongHieu == thuongHieu.IdThuongHieu))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(thuongHieu);
        }


        // GET: Admin/AdminThuongHieux/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.IdThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // POST: Admin/AdminThuongHieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu != null)
            {
                _context.ThuongHieus.Remove(thuongHieu);
            }

            await _context.SaveChangesAsync();
            TempData["deletemess"] = "Tài khoản đã bị xóa!";
            return RedirectToAction(nameof(Index));
        }

        private bool ThuongHieuExists(string id)
        {
            return _context.ThuongHieus.Any(e => e.IdThuongHieu == id);
        }
    }
}
