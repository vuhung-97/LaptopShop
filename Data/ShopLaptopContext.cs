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

        => optionsBuilder.UseSqlServer("Data Source=NONAME\\MSSQLSERVER99;Initial Catalog=ShopLaptop;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.IdDonHang, e.IdLaptop }).HasName("PK__ChiTietD__2470A5988B42F52F");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.IdDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_DonHang");
            entity.Property(e => e.IdLaptop)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Laptop");

            entity.HasOne(d => d.IdDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IdDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__ID_Do__4E88ABD4");

            entity.HasOne(d => d.IdLaptopNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IdLaptop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietDonHang_Laptop");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.IdDonHang);

            entity.ToTable("DonHang", tb => tb.HasTrigger("trg_ThemDonHang"));

            entity.Property(e => e.IdDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_DonHang");
            entity.Property(e => e.DiaChiGiao).HasMaxLength(500);
            entity.Property(e => e.IdTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_TaiKhoan");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("ChoXacNhan");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdTaiKhoan)
                .HasConstraintName("FK__DonHang__ID_TaiK__4AB81AF0");
        });

        modelBuilder.Entity<Laptop>(entity =>
        {
            entity.HasKey(e => e.IdLaptop);

            entity.ToTable("Laptop", tb => tb.HasTrigger("trg_LaptopID"));

            entity.Property(e => e.IdLaptop)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Laptop");
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
            entity.Property(e => e.SoLuong).HasDefaultValue(0);
            entity.Property(e => e.TenLapTop).HasMaxLength(50);

            entity.HasOne(d => d.IdLoaiNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdLoai)
                .HasConstraintName("FK__Laptop__ID_Loai__403A8C7D");

            entity.HasOne(d => d.IdThongTinNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdThongTin)
                .HasConstraintName("FK__Laptop__ID_Thong__3F466844");

            entity.HasOne(d => d.IdThuongHieuNavigation).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.IdThuongHieu)
                .HasConstraintName("FK__Laptop__ID_Thuon__3E52440B");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.IdLoai).HasName("PK__Loai__914C2314A246C954");

            entity.ToTable("Loai");

            entity.Property(e => e.IdLoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID_Loai");
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdTaiKhoan);

            entity.ToTable("TaiKhoan", tb => tb.HasTrigger("trg_TaiKhoanID"));

            entity.HasIndex(e => e.HoTen, "UQ__TaiKhoan__27AEE13C7782E4D8").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TaiKhoan__A9D105348C942FEE").IsUnique();

            entity.Property(e => e.IdTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_TaiKhoan");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.Loai)
                .HasMaxLength(50)
                .HasDefaultValue("KhachHang");
            entity.Property(e => e.MatKhau).HasMaxLength(100);
        });

        modelBuilder.Entity<ThongTinChiTiet>(entity =>
        {
            entity.HasKey(e => e.IdThongTin).HasName("PK__ThongTin__BB9645AF575C81FA");

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
            entity.HasKey(e => e.IdThuongHieu).HasName("PK__ThuongHi__AB2A011AB9267087");

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
        modelBuilder.HasSequence("DonHangSequence");
        modelBuilder.HasSequence("LaptopSequence");
        modelBuilder.HasSequence("TaiKhoanSequence");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
