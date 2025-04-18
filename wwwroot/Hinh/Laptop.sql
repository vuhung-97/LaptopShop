Use ShopLaptop


CREATE TABLE ThuongHieu (
    ID_ThuongHieu varchar(20) PRIMARY KEY,
    TenThuongHieu NVARCHAR(100),
	Logo varchar(50)
);

CREATE TABLE Loai(
	ID_Loai varchar(20) PRIMARY KEY,
	TenLoai NVARCHAR(100)
	)
CREATE TABLE ThongTinChiTiet (
    ID_ThongTin varchar(50) PRIMARY KEY,
    CPU NVARCHAR(100),
    RAM NVARCHAR(100),
    OCung NVARCHAR(100),
    ManHinh NVARCHAR(100),
    DoHoa NVARCHAR(100),
    CongGiaoTiep NVARCHAR(100),
    HeDieuHanh NVARCHAR(100),
    Pin NVARCHAR(100),
    TrongLuong NVARCHAR(50),
	)

CREATE TABLE Laptop (
    ID_Laptop varchar(50) PRIMARY KEY,
    ID_ThuongHieu varchar(20),
    TenLapTop varchar(50),
    GiaBan DECIMAL(18, 2),
    HinhAnh NVARCHAR(255), --đường dẫn đến file
    ID_ThongTin varchar(50),
	ID_Loai varchar(20),
    FOREIGN KEY (ID_ThuongHieu) REFERENCES ThuongHieu(ID_ThuongHieu),
	 FOREIGN KEY (ID_ThongTin) REFERENCES ThongTinChiTiet(ID_ThongTin),
	FOREIGN KEY (ID_Loai) REFERENCES Loai(ID_Loai)
);

CREATE TABLE TaiKhoan (
    ID_TaiKhoan nvarchar(50) PRIMARY KEY,
    HoTen NVARCHAR(100),
    MatKhau NVARCHAR(100),
    Loai NVARCHAR(50),
);
CREATE TABLE DonHang (
    ID_DonHang INT PRIMARY KEY,
    ID_TaiKhoan nvarchar(50) null,
    NgayDatHang DATETIME,
    TongTien DECIMAL(18,2),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ID_TaiKhoan) REFERENCES TaiKhoan(ID_TaiKhoan)
);
CREATE TABLE ChiTietDonHang (
    ID_DonHang INT,
    ID_Laptop varchar(50),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    PRIMARY KEY (ID_DonHang, ID_Laptop),
    FOREIGN KEY (ID_DonHang) REFERENCES DonHang(ID_DonHang),
    FOREIGN KEY (ID_Laptop) REFERENCES Laptop(ID_Laptop)
);





