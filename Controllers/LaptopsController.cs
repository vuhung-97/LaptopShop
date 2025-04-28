using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace LaptopShop.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly ShopLaptopContext _context;

        public LaptopsController(ShopLaptopContext context)
        {
            _context = context;
        }

        // GET: Laptops
        public async Task<IActionResult> Index()
        {
            var shopLaptopContext = _context.Laptops.Include(l => l.IdLoaiNavigation).Include(l => l.IdThongTinNavigation).Include(l => l.IdThuongHieuNavigation);
            return View(await shopLaptopContext.ToListAsync());
        }

        // GET: Laptops/Details/5
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

        // GET: Laptops/Create
        public IActionResult Create()
        {
            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "IdLoai");
            ViewData["IdThongTin"] = new SelectList(_context.ThongTinChiTiets, "IdThongTin", "IdThongTin");
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "IdThuongHieu");
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLaptop,IdThuongHieu,TenLapTop,GiaBan,HinhAnh,IdThongTin,IdLoai")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "IdLoai", laptop.IdLoai);
            ViewData["IdThongTin"] = new SelectList(_context.ThongTinChiTiets, "IdThongTin", "IdThongTin", laptop.IdThongTin);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "IdThuongHieu", laptop.IdThuongHieu);
            return View(laptop);
        }

        // GET: Laptops/Edit/5
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
            ViewData["IdLoai"] = new SelectList(_context.Loais, "IdLoai", "IdLoai", laptop.IdLoai);
            ViewData["IdThongTin"] = new SelectList(_context.ThongTinChiTiets, "IdThongTin", "IdThongTin", laptop.IdThongTin);
            ViewData["IdThuongHieu"] = new SelectList(_context.ThuongHieus, "IdThuongHieu", "IdThuongHieu", laptop.IdThuongHieu);
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdLaptop,IdThuongHieu,TenLapTop,GiaBan,HinhAnh,IdThongTin,IdLoai")] Laptop laptop)
        {
            if (id != laptop.IdLaptop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Laptops/Delete/5
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

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(string id)
        {
            return _context.Laptops.Any(e => e.IdLaptop == id);
        }
    }
}
