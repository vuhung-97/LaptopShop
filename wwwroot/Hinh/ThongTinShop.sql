
INSERT INTO Loai (ID_Loai, TenLoai)
VALUES 
    ('L01', N'Laptop học tập - văn phòng'),
    ('L02', N'Laptop đồ họa - kỹ thuật'),
    ('L03', N'Laptop gaming'),
    ('L04', N'Laptop mỏng nhẹ'),
    ('L05', N'Laptop cao cấp');

INSERT INTO ThuongHieu (ID_ThuongHieu, TenThuongHieu, Logo)
VALUES 
    ('TH01', N'Dell', 'dell.png'),
    ('TH02', N'HP', 'hp.png'),
    ('TH03', N'Macbook', 'macbook.png'),
    ('TH04', N'Lenovo', 'lenovo.png'),
    ('TH05', N'Asus', 'asus.png'),
	('TH06', 'Acer', 'acer_logo.png');


	--------------------
	-- Laptop Dell
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP001', 'TH01', 'Dell XPS 13', 22000000, 'dell_xps13.jpg', 'TT01', 'L01'),
('LAP002', 'TH01', 'Dell Inspiron 15', 16000000, 'dell_inspiron15.jpg', 'TT02', 'L01'),
('LAP003', 'TH01', 'Dell Alienware M15', 35000000, 'dell_alienware_m15.jpg', 'TT03', 'L02'),
('LAP004', 'TH01', 'Dell Latitude 7420', 24000000, 'dell_latitude7420.jpg', 'TT04', 'L01'),
('LAP005', 'TH01', 'Dell G5 15', 22000000, 'dell_g5_15.jpg', 'TT05', 'L02'),
('LAP006', 'TH01', 'Dell Vostro 15', 17000000, 'dell_vostro15.jpg', 'TT01', 'L01'),
('LAP007', 'TH01', 'Dell XPS 17', 28000000, 'dell_xps17.jpg', 'TT02', 'L02'),
('LAP008', 'TH01', 'Dell G7 15', 30000000, 'dell_g7_15.jpg', 'TT03', 'L02'),
('LAP009', 'TH01', 'Dell Precision 7550', 35000000, 'dell_precision7550.jpg', 'TT04', 'L02'),
('LAP010', 'TH01', 'Dell Alienware Area-51m', 45000000, 'dell_area51m.jpg', 'TT05', 'L03');

-- Laptop HP
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP011', 'TH02', 'HP Spectre x360', 24000000, 'hp_spectre_x360.jpg', 'TT01', 'L01'),
('LAP012', 'TH02', 'HP Envy 13', 20000000, 'hp_envy13.jpg', 'TT02', 'L01'),
('LAP013', 'TH02', 'HP Pavilion 15', 18000000, 'hp_pavilion15.jpg', 'TT03', 'L01'),
('LAP014', 'TH02', 'HP Omen 15', 35000000, 'hp_omen15.jpg', 'TT04', 'L02'),
('LAP015', 'TH02', 'HP Elite Dragonfly', 35000000, 'hp_elite_dragonfly.jpg', 'TT05', 'L01'),
('LAP016', 'TH02', 'HP ProBook 450', 15000000, 'hp_probook450.jpg', 'TT01', 'L01'),
('LAP017', 'TH02', 'HP EliteBook 850', 25000000, 'hp_elitebook850.jpg', 'TT02', 'L02'),
('LAP018', 'TH02', 'HP Spectre x360 14', 27000000, 'hp_spectre_x360_14.jpg', 'TT03', 'L01'),
('LAP019', 'TH02', 'HP Pavilion x360', 19000000, 'hp_pavilion_x360.jpg', 'TT04', 'L03'),
('LAP020', 'TH02', 'HP ZBook Create G7', 40000000, 'hp_zbook_create_g7.jpg', 'TT05', 'L02');

-- Laptop Apple
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP021', 'TH03', 'Apple MacBook Air M1', 28000000, 'macbook_air_m1.jpg', 'TT01', 'L01'),
('LAP022', 'TH03', 'Apple MacBook Pro 13', 35000000, 'macbook_pro_13.jpg', 'TT02', 'L01'),
('LAP023', 'TH03', 'Apple MacBook Pro 16', 42000000, 'macbook_pro_16.jpg', 'TT03', 'L01'),
('LAP024', 'TH03', 'Apple MacBook Pro M1 Pro', 46000000, 'macbook_pro_m1pro.jpg', 'TT04', 'L02'),
('LAP025', 'TH03', 'Apple MacBook Air M2', 32000000, 'macbook_air_m2.jpg', 'TT05', 'L01'),
('LAP026', 'TH03', 'Apple MacBook Pro 14', 38000000, 'macbook_pro_14.jpg', 'TT01', 'L02'),
('LAP027', 'TH03', 'Apple MacBook Pro M1 Max', 50000000, 'macbook_pro_m1max.jpg', 'TT02', 'L03'),
('LAP028', 'TH03', 'Apple MacBook Pro 13 Retina', 35000000, 'macbook_pro_13_retina.jpg', 'TT03', 'L01'),
('LAP029', 'TH03', 'Apple MacBook 12-inch', 24000000, 'macbook_12_inch.jpg', 'TT04', 'L01'),
('LAP030', 'TH03', 'Apple MacBook Air Retina', 27000000, 'macbook_air_retina.jpg', 'TT05', 'L02');

-- Laptop Lenovo
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP031', 'TH04', 'Lenovo ThinkPad X1 Carbon', 32000000, 'lenovo_thinkpad_x1_carbon.jpg', 'TT01', 'L01'),
('LAP032', 'TH04', 'Lenovo ThinkPad X1 Yoga', 34000000, 'lenovo_thinkpad_x1_yoga.jpg', 'TT02', 'L01'),
('LAP033', 'TH04', 'Lenovo Legion 5', 30000000, 'lenovo_legion5.jpg', 'TT03', 'L02'),
('LAP034', 'TH04', 'Lenovo IdeaPad 3', 15000000, 'lenovo_ideapad3.jpg', 'TT04', 'L01'),
('LAP035', 'TH04', 'Lenovo Yoga 9i', 28000000, 'lenovo_yoga9i.jpg', 'TT05', 'L02'),
('LAP036', 'TH04', 'Lenovo ThinkPad P1', 40000000, 'lenovo_thinkpad_p1.jpg', 'TT01', 'L01'),
('LAP037', 'TH04', 'Lenovo ThinkPad E14', 25000000, 'lenovo_thinkpad_e14.jpg', 'TT02', 'L02'),
('LAP038', 'TH04', 'Lenovo Legion 7i', 42000000, 'lenovo_legion7i.jpg', 'TT03', 'L02'),
('LAP039', 'TH04', 'Lenovo IdeaPad Flex 5', 18000000, 'lenovo_ideapad_flex5.jpg', 'TT04', 'L01'),
('LAP040', 'TH04', 'Lenovo ThinkPad X1 Extreme', 38000000, 'lenovo_thinkpad_x1_extreme.jpg', 'TT05', 'L02');
-- Laptop Asus
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP041', 'TH05', 'Asus ZenBook 13', 22000000, 'asus_zenbook13.jpg', 'TT01', 'L01'),
('LAP042', 'TH05', 'Asus ROG Zephyrus G14', 32000000, 'asus_rog_zephyrus_g14.jpg', 'TT02', 'L02'),
('LAP043', 'TH05', 'Asus TUF Gaming A15', 28000000, 'asus_tuf_gaming_a15.jpg', 'TT03', 'L02'),
('LAP044', 'TH05', 'Asus VivoBook 15', 17000000, 'asus_vivobook15.jpg', 'TT04', 'L01'),
('LAP045', 'TH05', 'Asus ROG Strix G15', 33000000, 'asus_rog_strix_g15.jpg', 'TT05', 'L02'),
('LAP046', 'TH05', 'Asus ZenBook Flip 14', 25000000, 'asus_zenbook_flip14.jpg', 'TT01', 'L01'),
('LAP047', 'TH05', 'Asus TUF Dash F15', 29000000, 'asus_tuf_dash_f15.jpg', 'TT02', 'L02'),
('LAP048', 'TH05', 'Asus VivoBook S14', 20000000, 'asus_vivobook_s14.jpg', 'TT03', 'L01'),
('LAP049', 'TH05', 'Asus ZenBook Pro Duo', 38000000, 'asus_zenbook_pro_duo.jpg', 'TT04', 'L02'),
('LAP050', 'TH05', 'Asus Chromebook Flip', 15000000, 'asus_chromebook_flip.jpg', 'TT05', 'L01');

-- Laptop Acer
INSERT INTO Laptop (ID_Laptop, ID_ThuongHieu, TenLaptop, GiaBan, HinhAnh, ID_ThongTin, ID_Loai)
VALUES 
('LAP051', 'TH06', 'Acer Aspire 5', 16000000, 'acer_aspire5.jpg', 'TT01', 'L01'),
('LAP052', 'TH06', 'Acer Predator Helios 300', 35000000, 'acer_predator_helios300.jpg', 'TT02', 'L02'),
('LAP053', 'TH06', 'Acer Swift 3', 22000000, 'acer_swift3.jpg', 'TT03', 'L01'),
('LAP054', 'TH06', 'Acer Nitro 5', 28000000, 'acer_nitro5.jpg', 'TT04', 'L02'),
('LAP055', 'TH06', 'Acer Aspire 7', 23000000, 'acer_aspire7.jpg', 'TT05', 'L01'),
('LAP056', 'TH06', 'Acer ConceptD 7', 45000000, 'acer_conceptd7.jpg', 'TT01', 'L02'),
('LAP057', 'TH06', 'Acer Chromebook 15', 17000000, 'acer_chromebook15.jpg', 'TT02', 'L01'),
('LAP058', 'TH06', 'Acer Spin 5', 26000000, 'acer_spin5.jpg', 'TT03', 'L01'),
('LAP059', 'TH06', 'Acer Swift 1', 15000000, 'acer_swift1.jpg', 'TT04', 'L01'),
('LAP060', 'TH06', 'Acer Predator Triton 500', 38000000, 'acer_predator_triton500.jpg', 'TT05', 'L02');
-- Thông tin chi tiết cho các Laptop
INSERT INTO ThongTinChiTiet (ID_ThongTin, CPU, RAM, OCung, ManHinh, DoHoa, CongGiaoTiep, HeDieuHanh, Pin, TrongLuong )
VALUES 
('TT01', 'Intel Core i7', '8GB', 'SSD 512GB', '13.3 inch FHD', 'Intel Iris Xe', 'USB-C, HDMI', 'Windows 10', '4 Cell', '1.3kg'),
('TT02', 'Intel Core i5', '8GB', 'SSD 512GB', '15.6 inch Full HD', 'NVIDIA GTX 1650', 'USB 3.0, HDMI', 'Windows 10', '6 Cell', '1.8kg'),
('TT03', 'Intel Core i9', '16GB', 'SSD 1TB', '17.3 inch 4K', 'NVIDIA RTX 3070', 'USB-C, Thunderbolt', 'Windows 10', '8 Cell', '2.5kg'),
('TT04', 'AMD Ryzen 5', '16GB', 'SSD 1TB', '14 inch QHD', 'NVIDIA GTX 1650', 'USB-C, HDMI', 'Windows 10', '6 Cell', '1.7kg'),
('TT05', 'Intel Core i7', '32GB', 'SSD 1TB', '15.6 inch FHD', 'NVIDIA RTX 3060', 'USB-C, HDMI', 'Windows 10', '7 Cell', '2.0kg'),
('TT06', 'AMD Ryzen 5', '8GB', 'SSD 512GB', '13.3 inch Full HD', 'Intel Iris Xe', 'USB 3.0, HDMI', 'Windows 10', '4 Cell', '1.5kg'),
('TT07', 'Intel Core i9', '32GB', 'SSD 1TB', '16 inch Retina', 'NVIDIA RTX 3050', 'USB-C, Thunderbolt', 'macOS', '6 Cell', '2.0kg'),
('TT08', 'Intel Core i5', '8GB', 'SSD 256GB', '15.6 inch Full HD', 'Intel Iris Xe', 'USB-C, HDMI', 'Windows 10', '5 Cell', '1.8kg'),
('TT09', 'Intel Core i7', '16GB', 'SSD 512GB', '14 inch Retina', 'Intel Iris Xe', 'USB 3.0, HDMI', 'macOS', '4 Cell', '1.3kg'),
('TT10', 'Intel Core i5', '8GB', 'SSD 512GB', '13.3 inch Full HD', 'Intel Iris Xe', 'USB-C, HDMI', 'Windows 10', '5 Cell', '1.4kg');





-- Truy xuất tất cả laptop cùng với Thương Hiệu, Loại và Thông Tin Chi Tiết (bao gồm cả laptop thiếu thông tin)
SELECT 
    L.ID_Laptop,
    L.TenLapTop, 
    L.GiaBan, 
    L.HinhAnh, 
    ISNULL(TH.TenThuongHieu, 'Không có Thương Hiệu') AS TenThuongHieu, 
    ISNULL(LO.TenLoai, 'Không có Loại') AS TenLoai,
    ISNULL(TC.CPU, 'Không có CPU') AS CPU, 
    ISNULL(TC.RAM, 'Không có RAM') AS RAM, 
    ISNULL(TC.OCung, 'Không có Ổ Cứng') AS OCung, 
    ISNULL(TC.ManHinh, 'Không có Màn Hình') AS ManHinh, 
    ISNULL(TC.DoHoa, 'Không có Đồ Họa') AS DoHoa, 
    ISNULL(TC.CongGiaoTiep, 'Không có Cổng Giao Tiếp') AS CongGiaoTiep, 
    ISNULL(TC.HeDieuHanh, 'Không có Hệ Điều Hành') AS HeDieuHanh, 
    ISNULL(TC.Pin, 'Không có Pin') AS Pin, 
    ISNULL(TC.TrongLuong, 'Không có Trọng Lượng') AS TrongLuong
FROM 
    Laptop L
LEFT JOIN 
    ThuongHieu TH ON L.ID_ThuongHieu = TH.ID_ThuongHieu
LEFT JOIN 
    Loai LO ON L.ID_Loai = LO.ID_Loai
LEFT JOIN 
    ThongTinChiTiet TC ON L.ID_ThongTin = TC.ID_ThongTin;

	INSERT INTO TaiKhoan (ID_TaiKhoan, HoTen, MatKhau, Loai)
VALUES
    ('admin', 'Admin', 'admin', N'Admin'),
    ('TK02', N'Lê Thị Khách', 'khachhang1', N'KhachHang'),
    ('TK03', N'Trần Minh Khách', 'khachhang2', N'KhachHang');
