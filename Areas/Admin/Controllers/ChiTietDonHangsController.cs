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
    public class ChiTietDonHangsController : Controller
    {
        private readonly ShopLaptopContext _context;

        public ChiTietDonHangsController(ShopLaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/ChiTietDonHangs
        public async Task<IActionResult> Index()
        {
            var shopLaptopContext = _context.ChiTietDonHangs.Include(c => c.IdDonHangNavigation).Include(c => c.IdLaptopNavigation);
            return View(await shopLaptopContext.ToListAsync());
        }

        // GET: Admin/ChiTietDonHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.IdDonHangNavigation)
                .Include(c => c.IdLaptopNavigation)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Create
        public IActionResult Create()
        {
            ViewData["IdDonHang"] = new SelectList(_context.DonHangs, "IdDonHang", "IdDonHang");
            ViewData["IdLaptop"] = new SelectList(_context.Laptops, "IdLaptop", "IdLaptop");
            return View();
        }

        // POST: Admin/ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDonHang,IdLaptop,SoLuong,DonGia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDonHang"] = new SelectList(_context.DonHangs, "IdDonHang", "IdDonHang", chiTietDonHang.IdDonHang);
            ViewData["IdLaptop"] = new SelectList(_context.Laptops, "IdLaptop", "IdLaptop", chiTietDonHang.IdLaptop);
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }
            ViewData["IdDonHang"] = new SelectList(_context.DonHangs, "IdDonHang", "IdDonHang", chiTietDonHang.IdDonHang);
            ViewData["IdLaptop"] = new SelectList(_context.Laptops, "IdLaptop", "IdLaptop", chiTietDonHang.IdLaptop);
            return View(chiTietDonHang);
        }

        // POST: Admin/ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDonHang,IdLaptop,SoLuong,DonGia")] ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.IdDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDonHangExists(chiTietDonHang.IdDonHang))
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
            ViewData["IdDonHang"] = new SelectList(_context.DonHangs, "IdDonHang", "IdDonHang", chiTietDonHang.IdDonHang);
            ViewData["IdLaptop"] = new SelectList(_context.Laptops, "IdLaptop", "IdLaptop", chiTietDonHang.IdLaptop);
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.IdDonHangNavigation)
                .Include(c => c.IdLaptopNavigation)
                .FirstOrDefaultAsync(m => m.IdDonHang == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // POST: Admin/ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHangs.Remove(chiTietDonHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietDonHangExists(string id)
        {
            return _context.ChiTietDonHangs.Any(e => e.IdDonHang == id);
        }
    }
}
