using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;
using X.PagedList.Extensions;

namespace LaptopShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonHangsController : Controller
    {
        private readonly ShopLaptopContext _context;

        public AdminDonHangsController(ShopLaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDonHangs
        public async Task<IActionResult> Index(int? page)
        {
            var laptopsQuery = _context.DonHangs.Include(d => d.IdTaiKhoanNavigation)
                .AsQueryable();
             
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var laptopsPaged = laptopsQuery.ToPagedList(pageNumber, pageSize);

            return View(laptopsPaged);
        }

        // GET: Admin/AdminDonHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.IdTaiKhoanNavigation)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Create
        public IActionResult Create()
        {
            ViewData["IdTaiKhoan"] = new SelectList(_context.TaiKhoans, "IdTaiKhoan", "IdTaiKhoan");
            return View();
        }

        // POST: Admin/AdminDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDonHang,IdTaiKhoan,NgayDat,DiaChiGiao,TongTien,NgayCapNhat,TrangThai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTaiKhoan"] = new SelectList(_context.TaiKhoans, "IdTaiKhoan", "IdTaiKhoan", donHang.IdTaiKhoan);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["IdTaiKhoan"] = new SelectList(_context.TaiKhoans, "IdTaiKhoan", "IdTaiKhoan", donHang.IdTaiKhoan);
            return View(donHang);
        }

        // POST: Admin/AdminDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDonHang,IdTaiKhoan,NgayDat,DiaChiGiao,TongTien,NgayCapNhat,TrangThai")] DonHang donHang)
        {
            if (id != donHang.IdDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.IdDonHang))
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
            ViewData["IdTaiKhoan"] = new SelectList(_context.TaiKhoans, "IdTaiKhoan", "IdTaiKhoan", donHang.IdTaiKhoan);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.IdTaiKhoanNavigation)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: Admin/AdminDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(string id)
        {
            return _context.DonHangs.Any(e => e.IdDonHang == id);
        }
    }
}
