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
    public class AdminTaiKhoansController : Controller
    {
        private readonly ShopLaptopContext _context;

        public AdminTaiKhoansController(ShopLaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminTaiKhoans
        public async Task<IActionResult> Index(string searchString,int? page, string vaitro)
        {
            var taikhoanQuery= _context.TaiKhoans.AsQueryable();

            ViewData["searchString"] = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                taikhoanQuery = taikhoanQuery.Where(tk => tk.HoTen.Contains(searchString) || tk.Email.Contains(searchString) || tk.DiaChi.Contains(searchString));
            }

            ViewData["VaiTroList"] = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Vai trò --" },
                    new SelectListItem { Value = "Admin", Text = "Quản trị viên" },
                    new SelectListItem { Value = "KhachHang", Text = "Khách hàng" },

                };
            ViewData["VaiTro"] = vaitro;
            var validStatuses = new[] { "Admin", "KhachHang" };
            if (!string.IsNullOrEmpty(vaitro) && validStatuses.Contains(vaitro))
            {
                taikhoanQuery = taikhoanQuery.Where(tk => tk.Loai == vaitro);
            }


            int pageSize = 10;
            int pageNumber = page ?? 1;
            var taikhoanPaged = taikhoanQuery.ToPagedList(pageNumber, pageSize);

            return View(taikhoanPaged);
        }

        // GET: Admin/AdminTaiKhoans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.IdTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTaiKhoan,HoTen,MatKhau,Email,DienThoai,DiaChi,Loai")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdTaiKhoan,HoTen,MatKhau,Email,DienThoai,DiaChi,Loai")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.IdTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.IdTaiKhoan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["editmess"] = "Đã cập nhập tài khoản!";
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.IdTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }

            await _context.SaveChangesAsync();
            TempData["deletemess"] = "Tài khoản đã bị xóa!";
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoans.Any(e => e.IdTaiKhoan == id);
        }
    }
}
