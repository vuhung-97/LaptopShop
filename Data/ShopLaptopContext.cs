using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop.Data;

public partial class ShopLaptopContext : DbContext
{
    public ShopLaptopContext()
    {
    }

    public ShopLaptopContext(DbContextOptions<ShopLaptopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<Laptop> Laptops { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThongTinChiTiet> ThongTinChiTiets { get; set; }

    public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-R51A4CO\\HUNGVU;Database=ShopLaptop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.IdDonHang, e.IdLaptop }).HasName("PK__ChiTietD__2470A5988E500BFB");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.IdDonHang).HasColumnName("ID_DonHang");
            entity.Property(e => e.IdLaptop)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Laptop");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IdDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__ID_Do__47DBAE45");

            entity.HasOne(d => d.IdLaptopNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IdLaptop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__ID_La__48CFD27E");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.IdDonHang).HasName("PK__DonHang__99B72639E665AE4B");

            entity.ToTable("DonHang");

            entity.Property(e => e.IdDonHang)
                .ValueGeneratedNever()
                .HasColumnName("ID_DonHang");
            entity.Property(e => e.IdTaiKhoan)
                .HasMaxLength(50)
                .HasColumnName("ID_TaiKhoan");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayDatHang).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdTaiKhoan)
                .HasConstraintName("FK__DonHang__ID_TaiK__44FF419A");
        });

        modelBuilder.Entity<Laptop>(entity =>
        {
            entity.HasKey(e => e.IdLaptop).HasName("PK__Laptop__DC783A1A9F8E5440");

            entity.ToTable("Laptop");

            entity.Property(e => e.IdLaptop)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Laptop");
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.IdLoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID_Loai");
            entity.Property(e => e.IdThongTin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_ThongTin");
            entity.Property(e => e.IdThuongHieu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID_ThuongHieu");
            entity.Property(e => e.TenLapTop)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLoaiNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdLoai)
                .HasConstraintName("FK__Laptop__ID_Loai__3F466844");

            entity.HasOne(d => d.IdThongTinNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdThongTin)
                .HasConstraintName("FK__Laptop__ID_Thong__3E52440B");

            entity.HasOne(d => d.IdThuongHieuNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdThuongHieu)
                .HasConstraintName("FK__Laptop__ID_Thuon__3D5E1FD2");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.IdLoai).HasName("PK__Loai__914C2314AEFE9BB7");

            entity.ToTable("Loai");

            entity.Property(e => e.IdLoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID_Loai");
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdTaiKhoan).HasName("PK__TaiKhoan__0E3EC2101FEFBC0C");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.IdTaiKhoan)
                .HasMaxLength(50)
                .HasColumnName("ID_TaiKhoan");
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Loai).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
        });

        modelBuilder.Entity<ThongTinChiTiet>(entity =>
        {
            entity.HasKey(e => e.IdThongTin).HasName("PK__ThongTin__BB9645AFF6935E06");

            entity.ToTable("ThongTinChiTiet");

            entity.Property(e => e.IdThongTin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_ThongTin");
            entity.Property(e => e.CongGiaoTiep).HasMaxLength(100);
            entity.Property(e => e.Cpu)
                .HasMaxLength(100)
                .HasColumnName("CPU");
            entity.Property(e => e.DoHoa).HasMaxLength(100);
            entity.Property(e => e.HeDieuHanh).HasMaxLength(100);
            entity.Property(e => e.ManHinh).HasMaxLength(100);
            entity.Property(e => e.Ocung)
                .HasMaxLength(100)
                .HasColumnName("OCung");
            entity.Property(e => e.Pin).HasMaxLength(100);
            entity.Property(e => e.Ram)
                .HasMaxLength(100)
                .HasColumnName("RAM");
            entity.Property(e => e.TrongLuong).HasMaxLength(50);
        });

        modelBuilder.Entity<ThuongHieu>(entity =>
        {
            entity.HasKey(e => e.IdThuongHieu).HasName("PK__ThuongHi__AB2A011AAE9E6012");

            entity.ToTable("ThuongHieu");

            entity.Property(e => e.IdThuongHieu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID_ThuongHieu");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TenThuongHieu).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
