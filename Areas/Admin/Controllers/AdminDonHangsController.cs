using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;
using X.PagedList.Extensions;
using Microsoft.Data.SqlClient;

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
        public async Task<IActionResult> Index(string searchString,string trangThai, int? page,DateTime? minDate, DateTime? maxDate, string sortColumn, string sortOrder)
        {
            // Lấy danh sách trạng thái để truyền ra View (Dropdown)
            ViewData["TrangThaiList"] = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Trạng thái --" },
                    new SelectListItem { Value = "choxacnhan", Text = "Chờ xác nhận" },
                    new SelectListItem { Value = "danggiao", Text = "Đang giao" },
                    new SelectListItem { Value = "dagiao", Text = "Đã giao" },
                    new SelectListItem { Value = "dahuy", Text = "Đã hủy" }

                };
            ViewData["TrangThai"] = trangThai;
            ViewData["searchString"] = searchString;
            ViewData["minDate"] = minDate;
            ViewData["maxDate"] = maxDate;
            ViewData["sortColumn"] = sortColumn;
            ViewData["sortOrder"] = sortOrder;
            // Truy vấn đơn hàng
            var donhangsQuery = _context.DonHangs.Include(d => d.IdTaiKhoanNavigation).AsQueryable();

            // Nếu người dùng chọn trạng thái, lọc thêm
            var validStatuses = new[] { "dahuy", "choxacnhan", "dagiao", "danggiao" };
            if (!string.IsNullOrEmpty(trangThai) && validStatuses.Contains(trangThai))
            {
                donhangsQuery = donhangsQuery.Where(d => d.TrangThai == trangThai);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                donhangsQuery = donhangsQuery.Where(l => l.DiaChiGiao.Contains(searchString) ||
                                                       l.IdTaiKhoanNavigation.HoTen.Contains(searchString));
            }

            if (minDate.HasValue)
            {
                donhangsQuery = donhangsQuery.Where(l => l.NgayDat.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                donhangsQuery = donhangsQuery.Where(l => l.NgayDat.Date <= maxDate.Value);
            }
            //thay đổi thứ tự sắp sếp
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "NgayDat":
                        donhangsQuery = (sortOrder == "asc")
                            ? donhangsQuery.OrderBy(l => l.NgayDat)
                            : donhangsQuery.OrderByDescending(l => l.NgayDat);
                        break;
                    case "TongTien":
                        donhangsQuery = (sortOrder == "asc")
                            ? donhangsQuery.OrderBy(l => l.TongTien)
                            : donhangsQuery.OrderByDescending(l => l.TongTien);
                        break;
                    case "TrangThai":
                        donhangsQuery = (sortOrder == "asc")
                            ? donhangsQuery.OrderBy(l => l.TrangThai)
                            : donhangsQuery.OrderByDescending(l => l.TrangThai);
                        break;
                    case "HoTen":
                        donhangsQuery = (sortOrder == "asc")
                            ? donhangsQuery.OrderBy(l => l.IdTaiKhoanNavigation.HoTen)
                            : donhangsQuery.OrderByDescending(l => l.IdTaiKhoanNavigation.HoTen);
                        break;
                    case "DiaChi":
                        donhangsQuery = (sortOrder == "asc")
                            ? donhangsQuery.OrderBy(l => l.IdTaiKhoanNavigation.DiaChi)
                            : donhangsQuery.OrderByDescending(l => l.IdTaiKhoanNavigation.DiaChi);
                        break;
                }
            }

            // Phân trang
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var donhangsPaged = donhangsQuery.ToPagedList(pageNumber, pageSize);

            return View(donhangsPaged);
        }


        // GET: Admin/AdminDonHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = _context.DonHangs
       .Include(d => d.IdTaiKhoanNavigation)
       .Include(d => d.ChiTietDonHangs) // THÊM DÒNG NÀY
           .ThenInclude(ct => ct.IdLaptopNavigation)
               .ThenInclude(l => l.IdThuongHieuNavigation) // Nếu cần thương hiệu
       .Include(d => d.ChiTietDonHangs)
           .ThenInclude(ct => ct.IdLaptopNavigation)
               .ThenInclude(l => l.IdLoaiNavigation) // Nếu cần dòng sản phẩm
       .FirstOrDefault(d => d.IdDonHang == id);
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

        // POST: Admin/AdminDonHangs/trangthai
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ChuyenTrangThai(string id, string newStatus)
        {
            var donHang = _context.DonHangs.FirstOrDefault(d => d.IdDonHang == id);
            if (donHang != null)
            {
                donHang.TrangThai = newStatus;
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { id });
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


        // POST: Admin/AdminDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs) // Nạp danh sách chi tiết đơn hàng
                .FirstOrDefaultAsync(d => d.IdDonHang == id);

            if (donHang != null)
            {
                // Xóa toàn bộ chi tiết đơn hàng
                _context.ChiTietDonHangs.RemoveRange(donHang.ChiTietDonHangs);

                // Xóa đơn hàng
                _context.DonHangs.Remove(donHang);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool DonHangExists(string id)
        {
            return _context.DonHangs.Any(e => e.IdDonHang == id);
        }
    }
}
