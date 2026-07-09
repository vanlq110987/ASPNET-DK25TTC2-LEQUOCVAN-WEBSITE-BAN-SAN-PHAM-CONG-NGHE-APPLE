/* ============================================================================
   AppleShopDB_Script.sql
   Đồ án: Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple
   Sinh viên: Lê Quốc Văn — DK25TTC2 — Trường Đại học Trà Vinh
   CSDL: SQL Server 2017+ — 21 bảng (6 Identity + 15 nghiệp vụ) + dữ liệu mẫu
   Cách chạy: mở SSMS → New Query → Execute toàn bộ file này.
   ========================================================================== */

IF DB_ID(N'AppleShopDB') IS NULL
BEGIN
    CREATE DATABASE AppleShopDB COLLATE Vietnamese_CI_AS;
END
GO

USE AppleShopDB;
GO

/* Gán owner hợp lệ (sa) để SSMS cho phép tạo Database Diagrams (sơ đồ ERD).
   Nếu thiếu bước này, SSMS báo lỗi "database does not have a valid owner"
   khi mở node Database Diagrams. */
ALTER AUTHORIZATION ON DATABASE::AppleShopDB TO sa;
GO

/* ==========================================================================
   PHẦN 1 — BẢNG ASP.NET IDENTITY (6 bảng)
   ========================================================================== */

IF OBJECT_ID(N'dbo.AspNetRoles') IS NULL
CREATE TABLE dbo.AspNetRoles (
    Id   NVARCHAR(128) NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NOT NULL
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'RoleNameIndex')
    CREATE UNIQUE INDEX RoleNameIndex ON dbo.AspNetRoles ([Name]);
GO

IF OBJECT_ID(N'dbo.AspNetUsers') IS NULL
CREATE TABLE dbo.AspNetUsers (
    Id                   NVARCHAR(128) NOT NULL PRIMARY KEY,
    Email                NVARCHAR(256) NULL,
    EmailConfirmed       BIT NOT NULL DEFAULT 0,
    PasswordHash         NVARCHAR(MAX) NULL,
    SecurityStamp        NVARCHAR(MAX) NULL,
    PhoneNumber          NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    TwoFactorEnabled     BIT NOT NULL DEFAULT 0,
    LockoutEndDateUtc    DATETIME NULL,
    LockoutEnabled       BIT NOT NULL DEFAULT 0,
    AccessFailedCount    INT NOT NULL DEFAULT 0,
    UserName             NVARCHAR(256) NOT NULL,
    -- Cột mở rộng của ApplicationUser
    FullName             NVARCHAR(100) NULL,
    Address              NVARCHAR(255) NULL,
    CreatedAt            DATETIME NOT NULL DEFAULT GETDATE()
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UserNameIndex')
    CREATE UNIQUE INDEX UserNameIndex ON dbo.AspNetUsers (UserName);
GO

IF OBJECT_ID(N'dbo.AspNetUserRoles') IS NULL
CREATE TABLE dbo.AspNetUserRoles (
    UserId NVARCHAR(128) NOT NULL,
    RoleId NVARCHAR(128) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.AspNetUserClaims') IS NULL
CREATE TABLE dbo.AspNetUserClaims (
    Id         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId     NVARCHAR(128) NOT NULL,
    ClaimType  NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_UserClaims_Users FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.AspNetUserLogins') IS NULL
CREATE TABLE dbo.AspNetUserLogins (
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey   NVARCHAR(128) NOT NULL,
    UserId        NVARCHAR(128) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey, UserId),
    CONSTRAINT FK_UserLogins_Users FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.AspNetRoleClaims') IS NULL
CREATE TABLE dbo.AspNetRoleClaims (
    Id         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleId     NVARCHAR(128) NOT NULL,
    ClaimType  NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_RoleClaims_Roles FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles (Id) ON DELETE CASCADE
);
GO

/* ==========================================================================
   PHẦN 2 — BẢNG NGHIỆP VỤ (15 bảng)
   ========================================================================== */

-- 1. Danh mục sản phẩm (iPhone, MacBook, iPad, ...)
IF OBJECT_ID(N'dbo.Categories') IS NULL
CREATE TABLE dbo.Categories (
    CategoryId   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name         NVARCHAR(100) NOT NULL,
    Slug         NVARCHAR(120) NOT NULL UNIQUE,
    Description  NVARCHAR(500) NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsActive     BIT NOT NULL DEFAULT 1
);
GO

-- 2. Sản phẩm Apple
IF OBJECT_ID(N'dbo.Products') IS NULL
CREATE TABLE dbo.Products (
    ProductId        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CategoryId       INT NOT NULL,
    Name             NVARCHAR(200) NOT NULL,
    Slug             NVARCHAR(220) NOT NULL UNIQUE,
    ShortDescription NVARCHAR(500) NULL,
    Description      NVARCHAR(MAX) NULL,          -- HTML từ CKEditor
    Price            DECIMAL(18,0) NOT NULL DEFAULT 0,
    SalePrice        DECIMAL(18,0) NULL,
    ImageUrl         NVARCHAR(500) NULL,
    Need             NVARCHAR(50)  NULL,          -- Nhu cầu: Học tập / Đồ họa / Văn phòng / Gaming
    IsFeatured       BIT NOT NULL DEFAULT 0,
    IsActive         BIT NOT NULL DEFAULT 1,
    ViewCount        INT NOT NULL DEFAULT 0,
    CreatedAt        DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt        DATETIME NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories (CategoryId)
);
GO

-- 3. Biến thể sản phẩm (màu sắc, dung lượng, cấu hình)
IF OBJECT_ID(N'dbo.ProductVariants') IS NULL
CREATE TABLE dbo.ProductVariants (
    VariantId       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId       INT NOT NULL,
    Color           NVARCHAR(50) NULL,
    Storage         NVARCHAR(50) NULL,
    SpecSummary     NVARCHAR(255) NULL,
    PriceAdjustment DECIMAL(18,0) NOT NULL DEFAULT 0,
    Sku             NVARCHAR(50) NOT NULL UNIQUE,
    IsActive        BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Variants_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products (ProductId) ON DELETE CASCADE
);
GO

-- 4. Hình ảnh sản phẩm / biến thể
IF OBJECT_ID(N'dbo.ProductImages') IS NULL
CREATE TABLE dbo.ProductImages (
    ImageId   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId INT NOT NULL,
    VariantId INT NULL,
    Url       NVARCHAR(500) NOT NULL,
    SortOrder INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Images_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products (ProductId) ON DELETE CASCADE,
    CONSTRAINT FK_Images_Variants FOREIGN KEY (VariantId) REFERENCES dbo.ProductVariants (VariantId)
);
GO

-- 5. Kênh phân phối (Apple Store, FPT Shop, CellphoneS, ...)
IF OBJECT_ID(N'dbo.DistributionChannels') IS NULL
CREATE TABLE dbo.DistributionChannels (
    ChannelId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name      NVARCHAR(100) NOT NULL,
    Slug      NVARCHAR(120) NOT NULL UNIQUE,
    Website   NVARCHAR(255) NULL,
    Hotline   NVARCHAR(20)  NULL,
    Address   NVARCHAR(255) NULL,
    IsActive  BIT NOT NULL DEFAULT 1
);
GO

-- 6. Tồn kho theo biến thể × kênh phân phối
IF OBJECT_ID(N'dbo.Inventories') IS NULL
CREATE TABLE dbo.Inventories (
    InventoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    VariantId   INT NOT NULL,
    ChannelId   INT NOT NULL,
    Quantity    INT NOT NULL DEFAULT 0,
    UpdatedAt   DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Inventories UNIQUE (VariantId, ChannelId),
    CONSTRAINT FK_Inventories_Variants FOREIGN KEY (VariantId) REFERENCES dbo.ProductVariants (VariantId) ON DELETE CASCADE,
    CONSTRAINT FK_Inventories_Channels FOREIGN KEY (ChannelId) REFERENCES dbo.DistributionChannels (ChannelId) ON DELETE CASCADE
);
GO

-- 7. Giỏ hàng (lưu theo UserId hoặc SessionId)
IF OBJECT_ID(N'dbo.Carts') IS NULL
CREATE TABLE dbo.Carts (
    CartId    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId    NVARCHAR(128) NULL,
    SessionId NVARCHAR(100) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    CONSTRAINT FK_Carts_Users FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id)
);
GO

-- 8. Chi tiết giỏ hàng
IF OBJECT_ID(N'dbo.CartItems') IS NULL
CREATE TABLE dbo.CartItems (
    CartItemId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CartId     INT NOT NULL,
    VariantId  INT NOT NULL,
    Quantity   INT NOT NULL DEFAULT 1,
    UnitPrice  DECIMAL(18,0) NOT NULL DEFAULT 0,
    CONSTRAINT FK_CartItems_Carts    FOREIGN KEY (CartId)    REFERENCES dbo.Carts (CartId) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Variants FOREIGN KEY (VariantId) REFERENCES dbo.ProductVariants (VariantId)
);
GO

-- 9. Khách hàng / người nhận hàng
IF OBJECT_ID(N'dbo.Customers') IS NULL
CREATE TABLE dbo.Customers (
    CustomerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId     NVARCHAR(128) NULL,
    FullName   NVARCHAR(100) NOT NULL,
    Phone      NVARCHAR(20)  NOT NULL,
    Email      NVARCHAR(256) NULL,
    Address    NVARCHAR(255) NOT NULL,
    CreatedAt  DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Customers_Users FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id)
);
GO

-- 10. Đơn hàng  (Status: 0 Mới | 1 Đang xử lý | 2 Đã giao | 3 Hủy)
IF OBJECT_ID(N'dbo.Orders') IS NULL
CREATE TABLE dbo.Orders (
    OrderId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderCode   NVARCHAR(20) NOT NULL UNIQUE,
    UserId      NVARCHAR(128) NULL,
    CustomerId  INT NOT NULL,
    OrderDate   DATETIME NOT NULL DEFAULT GETDATE(),
    Status      TINYINT NOT NULL DEFAULT 0,
    TotalAmount DECIMAL(18,0) NOT NULL DEFAULT 0,
    Note        NVARCHAR(500) NULL,
    IsRead      BIT NOT NULL DEFAULT 0,          -- phục vụ thông báo đơn hàng mới cho Admin
    CONSTRAINT FK_Orders_Users     FOREIGN KEY (UserId)     REFERENCES dbo.AspNetUsers (Id),
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (CustomerId)
);
GO

-- 11. Chi tiết đơn hàng (snapshot tên/giá tại thời điểm đặt)
IF OBJECT_ID(N'dbo.OrderItems') IS NULL
CREATE TABLE dbo.OrderItems (
    OrderItemId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId     INT NOT NULL,
    VariantId   INT NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    VariantDesc NVARCHAR(200) NULL,
    Quantity    INT NOT NULL DEFAULT 1,
    UnitPrice   DECIMAL(18,0) NOT NULL DEFAULT 0,
    CONSTRAINT FK_OrderItems_Orders   FOREIGN KEY (OrderId)   REFERENCES dbo.Orders (OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Variants FOREIGN KEY (VariantId) REFERENCES dbo.ProductVariants (VariantId)
);
GO

-- 12. Thanh toán  (Status: 0 Chưa thanh toán | 1 Đã thanh toán)
IF OBJECT_ID(N'dbo.Payments') IS NULL
CREATE TABLE dbo.Payments (
    PaymentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId   INT NOT NULL,
    Method    NVARCHAR(20) NOT NULL DEFAULT N'COD',
    Amount    DECIMAL(18,0) NOT NULL DEFAULT 0,
    Status    TINYINT NOT NULL DEFAULT 0,
    PaidAt    DATETIME NULL,
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders (OrderId) ON DELETE CASCADE
);
GO

-- 13. Bình luận & đánh giá sản phẩm
IF OBJECT_ID(N'dbo.ProductReviews') IS NULL
CREATE TABLE dbo.ProductReviews (
    ReviewId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId    INT NOT NULL,
    UserId       NVARCHAR(128) NULL,
    ReviewerName NVARCHAR(100) NOT NULL,
    Rating       INT NOT NULL DEFAULT 5,
    Comment      NVARCHAR(1000) NOT NULL,
    CreatedAt    DATETIME NOT NULL DEFAULT GETDATE(),
    IsApproved   BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_Reviews_Rating CHECK (Rating BETWEEN 1 AND 5),
    CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products (ProductId) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Users    FOREIGN KEY (UserId)    REFERENCES dbo.AspNetUsers (Id)
);
GO

-- 14. Chủ đề tin tức
IF OBJECT_ID(N'dbo.NewsCategories') IS NULL
CREATE TABLE dbo.NewsCategories (
    NewsCategoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name           NVARCHAR(100) NOT NULL,
    Slug           NVARCHAR(120) NOT NULL UNIQUE
);
GO

-- 15. Bài viết tin tức / khuyến mãi
IF OBJECT_ID(N'dbo.NewsArticles') IS NULL
CREATE TABLE dbo.NewsArticles (
    ArticleId      INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NewsCategoryId INT NOT NULL,
    Title          NVARCHAR(255) NOT NULL,
    Slug           NVARCHAR(270) NOT NULL UNIQUE,
    Summary        NVARCHAR(500) NULL,
    Content        NVARCHAR(MAX) NULL,           -- HTML từ CKEditor
    ThumbnailUrl   NVARCHAR(500) NULL,
    AuthorName     NVARCHAR(100) NULL,
    ViewCount      INT NOT NULL DEFAULT 0,
    IsPublished    BIT NOT NULL DEFAULT 1,
    CreatedAt      DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt      DATETIME NULL,
    CONSTRAINT FK_News_NewsCategories FOREIGN KEY (NewsCategoryId) REFERENCES dbo.NewsCategories (NewsCategoryId)
);
GO

/* ==========================================================================
   PHẦN 3 — DỮ LIỆU MẪU
   (Tài khoản Admin/Customer được ứng dụng tự tạo khi chạy lần đầu
    thông qua IdentitySeeder — xem App_Start/IdentityConfig.cs)
   ========================================================================== */

-- Danh mục
IF NOT EXISTS (SELECT 1 FROM dbo.Categories)
BEGIN
    INSERT INTO dbo.Categories (Name, Slug, Description, DisplayOrder) VALUES
    (N'iPhone',      N'iphone',      N'Điện thoại thông minh iPhone',            1),
    (N'MacBook',     N'macbook',     N'Máy tính xách tay MacBook Air / Pro',     2),
    (N'iPad',        N'ipad',        N'Máy tính bảng iPad',                      3),
    (N'Apple Watch', N'apple-watch', N'Đồng hồ thông minh Apple Watch',          4),
    (N'Phụ kiện',    N'phu-kien',    N'AirPods, sạc, ốp lưng và phụ kiện khác',  5);
END
GO

-- Kênh phân phối
IF NOT EXISTS (SELECT 1 FROM dbo.DistributionChannels)
BEGIN
    INSERT INTO dbo.DistributionChannels (Name, Slug, Website, Hotline, Address) VALUES
    (N'Apple Store Online',  N'apple-store-online', N'https://www.apple.com/vn',      N'1800-1192', N'Trực tuyến'),
    (N'FPT Shop',            N'fpt-shop',           N'https://fptshop.com.vn',        N'1800-6601', N'261-263 Khánh Hội, Q.4, TP.HCM'),
    (N'CellphoneS',          N'cellphones',         N'https://cellphones.com.vn',     N'1800-2097', N'350-352 Võ Văn Kiệt, Q.1, TP.HCM'),
    (N'TopZone',             N'topzone',            N'https://www.topzone.vn',        N'1800-1060', N'128 Trần Quang Khải, Q.1, TP.HCM'),
    (N'Thế Giới Di Động',    N'the-gioi-di-dong',   N'https://www.thegioididong.com', N'1800-1060', N'128 Trần Quang Khải, Q.1, TP.HCM');
END
GO

-- Sản phẩm
IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (CategoryId, Name, Slug, ShortDescription, Description, Price, SalePrice, ImageUrl, Need, IsFeatured) VALUES
    (1, N'iPhone 16 Pro Max', N'iphone-16-pro-max',
        N'Chip A18 Pro, khung Titan, camera 48MP, màn hình 6.9 inch ProMotion.',
        N'<p>iPhone 16 Pro Max là chiếc iPhone cao cấp nhất với chip <strong>A18 Pro</strong>, khung Titan chuẩn hàng không vũ trụ, hệ thống camera Pro 48MP và nút Camera Control hoàn toàn mới.</p>',
        34990000, 33490000, N'https://placehold.co/600x600/1d1d1f/ffffff?text=iPhone+16+Pro+Max', N'Cao cấp', 1),
    (1, N'iPhone 16', N'iphone-16',
        N'Chip A18, Dynamic Island, camera kép 48MP, 6 màu sắc trẻ trung.',
        N'<p>iPhone 16 mang đến hiệu năng vượt trội với chip <strong>A18</strong>, camera Fusion 48MP và thời lượng pin cả ngày.</p>',
        22990000, 21490000, N'https://placehold.co/600x600/0071e3/ffffff?text=iPhone+16', N'Văn phòng', 1),
    (1, N'iPhone 15', N'iphone-15',
        N'Chip A16 Bionic, Dynamic Island, camera 48MP — lựa chọn tiết kiệm.',
        N'<p>iPhone 15 với Dynamic Island và camera chính 48MP, mức giá dễ tiếp cận hơn cho người dùng phổ thông.</p>',
        18990000, 17490000, N'https://placehold.co/600x600/f5f5f7/1d1d1f?text=iPhone+15', N'Học tập', 0),
    (2, N'MacBook Air M3 13 inch', N'macbook-air-m3-13-inch',
        N'Chip Apple M3, mỏng nhẹ 1.24kg, pin 18 giờ — hoàn hảo cho học tập, văn phòng.',
        N'<p>MacBook Air M3 siêu mỏng nhẹ, hiệu năng chip <strong>Apple M3</strong> mạnh mẽ, màn hình Liquid Retina 13.6 inch, thời lượng pin lên đến 18 giờ.</p>',
        27990000, 26490000, N'https://placehold.co/600x600/e8e8ed/1d1d1f?text=MacBook+Air+M3', N'Học tập', 1),
    (2, N'MacBook Pro 14 M4 Pro', N'macbook-pro-14-m4-pro',
        N'Chip M4 Pro, màn hình Liquid Retina XDR 14 inch — dành cho đồ họa, lập trình chuyên nghiệp.',
        N'<p>MacBook Pro 14 inch với chip <strong>M4 Pro</strong> dành cho dân chuyên: dựng phim 8K, đồ họa 3D, lập trình đa nền tảng. Màn hình Liquid Retina XDR 1600 nits.</p>',
        52990000, NULL, N'https://placehold.co/600x600/2c2c2e/ffffff?text=MacBook+Pro+14', N'Đồ họa', 1),
    (2, N'MacBook Air M2 15 inch', N'macbook-air-m2-15-inch',
        N'Màn hình lớn 15.3 inch, chip M2, giá tốt cho sinh viên và nhân viên văn phòng.',
        N'<p>MacBook Air 15 inch M2 — màn hình rộng rãi, loa 6 củ sống động, vẫn giữ thiết kế siêu mỏng nhẹ đặc trưng.</p>',
        29990000, 27990000, N'https://placehold.co/600x600/f5f5f7/1d1d1f?text=MacBook+Air+15', N'Văn phòng', 0),
    (3, N'iPad Pro M4 11 inch', N'ipad-pro-m4-11-inch',
        N'Chip M4, màn hình Ultra Retina XDR OLED, hỗ trợ Apple Pencil Pro.',
        N'<p>iPad Pro M4 mỏng nhất từ trước đến nay với màn hình <strong>Ultra Retina XDR OLED</strong>, hiệu năng ngang laptop chuyên nghiệp.</p>',
        28990000, NULL, N'https://placehold.co/600x600/1d1d1f/ffffff?text=iPad+Pro+M4', N'Đồ họa', 1),
    (3, N'iPad Air M2', N'ipad-air-m2',
        N'Chip M2, màn hình 11 inch Liquid Retina, hỗ trợ Apple Pencil và Magic Keyboard.',
        N'<p>iPad Air M2 cân bằng hoàn hảo giữa hiệu năng và giá thành, phù hợp học tập và giải trí.</p>',
        16990000, 15990000, N'https://placehold.co/600x600/0071e3/ffffff?text=iPad+Air+M2', N'Học tập', 0),
    (3, N'iPad 10', N'ipad-10',
        N'Chip A14 Bionic, màn hình 10.9 inch, thiết kế trẻ trung nhiều màu sắc.',
        N'<p>iPad thế hệ 10 — chiếc iPad quốc dân cho học tập trực tuyến và giải trí gia đình.</p>',
        9990000, 8990000, N'https://placehold.co/600x600/f5f5f7/1d1d1f?text=iPad+10', N'Học tập', 0),
    (4, N'Apple Watch Series 10', N'apple-watch-series-10',
        N'Màn hình lớn hơn, mỏng hơn, đo điện tâm đồ ECG, phát hiện ngưng thở khi ngủ.',
        N'<p>Apple Watch Series 10 kỷ niệm 10 năm Apple Watch với màn hình lớn nhất từ trước đến nay và cảm biến sức khỏe toàn diện.</p>',
        10990000, 10490000, N'https://placehold.co/600x600/1d1d1f/ffffff?text=Watch+S10', N'Cao cấp', 1),
    (4, N'Apple Watch SE 2', N'apple-watch-se-2',
        N'Đầy đủ tính năng theo dõi sức khỏe cơ bản với mức giá dễ tiếp cận.',
        N'<p>Apple Watch SE 2 — cửa ngõ vào hệ sinh thái Apple Watch với đầy đủ tính năng thiết yếu.</p>',
        6390000, 5990000, N'https://placehold.co/600x600/f5f5f7/1d1d1f?text=Watch+SE+2', N'Văn phòng', 0),
    (5, N'AirPods Pro 2 (USB-C)', N'airpods-pro-2-usb-c',
        N'Chống ồn chủ động gấp 2 lần, âm thanh thích ứng, hộp sạc USB-C.',
        N'<p>AirPods Pro 2 với chip H2 — chống ồn chủ động đỉnh cao, chế độ xuyên âm thích ứng thông minh.</p>',
        6190000, 5690000, N'https://placehold.co/600x600/f5f5f7/1d1d1f?text=AirPods+Pro+2', N'Văn phòng', 1),
    (5, N'Sạc nhanh Apple 20W USB-C', N'sac-nhanh-apple-20w-usb-c',
        N'Củ sạc nhanh chính hãng 20W cho iPhone và iPad.',
        N'<p>Củ sạc Apple 20W USB-C chính hãng — sạc nhanh an toàn cho mọi thiết bị Apple.</p>',
        549000, 490000, N'https://placehold.co/600x600/ffffff/1d1d1f?text=Adapter+20W', N'Văn phòng', 0);
END
GO

-- Biến thể sản phẩm
IF NOT EXISTS (SELECT 1 FROM dbo.ProductVariants)
BEGIN
    INSERT INTO dbo.ProductVariants (ProductId, Color, Storage, SpecSummary, PriceAdjustment, Sku) VALUES
    (1,  N'Titan Sa Mạc',  N'256GB', N'A18 Pro · 8GB RAM',        0,        N'IP16PM-256-DS'),
    (1,  N'Titan Sa Mạc',  N'512GB', N'A18 Pro · 8GB RAM',        6000000,  N'IP16PM-512-DS'),
    (1,  N'Titan Tự Nhiên',N'256GB', N'A18 Pro · 8GB RAM',        0,        N'IP16PM-256-NT'),
    (2,  N'Xanh Lưu Ly',   N'128GB', N'A18 · 8GB RAM',            0,        N'IP16-128-BL'),
    (2,  N'Hồng',          N'256GB', N'A18 · 8GB RAM',            3000000,  N'IP16-256-PK'),
    (3,  N'Đen',           N'128GB', N'A16 Bionic · 6GB RAM',     0,        N'IP15-128-BK'),
    (3,  N'Xanh Dương',    N'256GB', N'A16 Bionic · 6GB RAM',     3000000,  N'IP15-256-BL'),
    (4,  N'Bạc',           N'256GB', N'M3 · 8GB RAM · 8 GPU',     0,        N'MBA13M3-256-SL'),
    (4,  N'Xám',           N'512GB', N'M3 · 16GB RAM · 10 GPU',   7000000,  N'MBA13M3-512-SG'),
    (5,  N'Đen',           N'512GB', N'M4 Pro · 24GB RAM',        0,        N'MBP14M4P-512-BK'),
    (5,  N'Bạc',           N'1TB',   N'M4 Pro · 24GB RAM',        9000000,  N'MBP14M4P-1TB-SL'),
    (6,  N'Xanh Đêm',      N'256GB', N'M2 · 8GB RAM',             0,        N'MBA15M2-256-MN'),
    (7,  N'Đen',           N'256GB', N'M4 · 8GB RAM',             0,        N'IPP11M4-256-BK'),
    (7,  N'Bạc',           N'512GB', N'M4 · 8GB RAM',             6000000,  N'IPP11M4-512-SL'),
    (8,  N'Xanh',          N'128GB', N'M2 · 8GB RAM',             0,        N'IPAM2-128-BL'),
    (9,  N'Vàng',          N'64GB',  N'A14 Bionic · 4GB RAM',     0,        N'IP10-64-YL'),
    (10, N'Đen Jet',       N'46mm',  N'GPS · Nhôm',               0,        N'AWS10-46-JB'),
    (10, N'Vàng Hồng',     N'42mm',  N'GPS · Nhôm',               -1000000, N'AWS10-42-RG'),
    (11, N'Trắng',         N'44mm',  N'GPS · Nhôm',               0,        N'AWSE2-44-WH'),
    (12, N'Trắng',         NULL,     N'Chống ồn chủ động · H2',   0,        N'APP2-USBC'),
    (13, N'Trắng',         NULL,     N'20W USB-C',                0,        N'ADP-20W');
END
GO

-- Tồn kho (mỗi biến thể có hàng ở 2–3 kênh)
IF NOT EXISTS (SELECT 1 FROM dbo.Inventories)
BEGIN
    INSERT INTO dbo.Inventories (VariantId, ChannelId, Quantity)
    SELECT v.VariantId, c.ChannelId, 20 + (v.VariantId * 3 + c.ChannelId * 7) % 30
    FROM dbo.ProductVariants v
    CROSS JOIN dbo.DistributionChannels c
    WHERE (v.VariantId + c.ChannelId) % 2 = 0;
END
GO

-- Chủ đề tin tức
IF NOT EXISTS (SELECT 1 FROM dbo.NewsCategories)
BEGIN
    INSERT INTO dbo.NewsCategories (Name, Slug) VALUES
    (N'Khuyến mãi',        N'khuyen-mai'),
    (N'Tin công nghệ',     N'tin-cong-nghe'),
    (N'Thủ thuật & Mẹo',   N'thu-thuat-meo');
END
GO

-- Bài viết tin tức
IF NOT EXISTS (SELECT 1 FROM dbo.NewsArticles)
BEGIN
    INSERT INTO dbo.NewsArticles (NewsCategoryId, Title, Slug, Summary, Content, ThumbnailUrl, AuthorName) VALUES
    (1, N'Giảm đến 1.5 triệu cho iPhone 16 Series trong tháng 7',
        N'giam-den-1-5-trieu-cho-iphone-16-series-trong-thang-7',
        N'Chương trình ưu đãi lớn nhất mùa hè dành cho toàn bộ dòng iPhone 16.',
        N'<p>Từ ngày 01/07 đến 31/07, AppleShop giảm giá trực tiếp đến <strong>1.500.000đ</strong> cho toàn bộ dòng iPhone 16, áp dụng đồng thời trả góp 0%.</p>',
        N'https://placehold.co/800x450/0071e3/ffffff?text=Sale+iPhone+16', N'AppleShop'),
    (1, N'Back to School — MacBook Air giảm sốc cho sinh viên',
        N'back-to-school-macbook-air-giam-soc-cho-sinh-vien',
        N'Sinh viên mua MacBook Air M2/M3 được giảm thêm 500.000đ khi xuất trình thẻ sinh viên.',
        N'<p>Chương trình <strong>Back to School 2026</strong>: giảm thêm 500.000đ và tặng balo thời trang khi mua MacBook Air kèm thẻ sinh viên.</p>',
        N'https://placehold.co/800x450/34c759/ffffff?text=Back+to+School', N'AppleShop'),
    (2, N'Apple ra mắt chip M4 Pro — hiệu năng đồ họa tăng 40%',
        N'apple-ra-mat-chip-m4-pro-hieu-nang-do-hoa-tang-40',
        N'Thế hệ chip Apple Silicon mới nhất đưa MacBook Pro lên tầm cao mới.',
        N'<p>Chip <strong>M4 Pro</strong> sản xuất trên tiến trình 3nm thế hệ 2, GPU 20 lõi cho hiệu năng đồ họa vượt trội 40% so với M3 Pro.</p>',
        N'https://placehold.co/800x450/2c2c2e/ffffff?text=Chip+M4+Pro', N'AppleShop'),
    (3, N'5 mẹo tiết kiệm pin iPhone ai cũng nên biết',
        N'5-meo-tiet-kiem-pin-iphone-ai-cung-nen-biet',
        N'Tối ưu thời lượng pin iPhone với các thiết lập đơn giản.',
        N'<p>1. Bật chế độ Nguồn điện thấp. 2. Giảm độ sáng tự động. 3. Tắt làm mới ứng dụng nền. 4. Dùng chế độ tối. 5. Kiểm tra tình trạng pin định kỳ.</p>',
        N'https://placehold.co/800x450/ff9500/ffffff?text=Battery+Tips', N'AppleShop');
END
GO

-- Bình luận mẫu
IF NOT EXISTS (SELECT 1 FROM dbo.ProductReviews)
BEGIN
    INSERT INTO dbo.ProductReviews (ProductId, ReviewerName, Rating, Comment) VALUES
    (1, N'Nguyễn Minh Khang', 5, N'Máy đẹp, camera chụp đêm quá đỉnh. Giao hàng nhanh!'),
    (1, N'Trần Thị Hồng',     4, N'Sản phẩm tốt nhưng giá còn cao, mong có thêm khuyến mãi.'),
    (4, N'Lê Hoàng Phúc',     5, N'MacBook Air M3 chạy mượt, pin trâu, rất hợp cho sinh viên IT.'),
    (7, N'Phạm Quỳnh Anh',    5, N'Màn hình OLED đẹp xuất sắc, vẽ với Apple Pencil Pro rất sướng.'),
    (12, N'Võ Thành Đạt',     4, N'Chống ồn tốt, đeo lâu hơi cấn tai một chút.');
END
GO

PRINT N'>>> Khởi tạo AppleShopDB hoàn tất: 21 bảng (6 Identity + 15 nghiệp vụ) cùng dữ liệu mẫu.';
PRINT N'>>> Tài khoản Admin/Customer sẽ được ứng dụng tự tạo khi chạy lần đầu (IdentitySeeder).';
PRINT N'>>> Tạo sơ đồ ERD: Object Explorer → AppleShopDB → chuột phải "Database Diagrams"';
PRINT N'>>> → Yes (cài diagram support) → New Database Diagram → Add toàn bộ 21 bảng → Save.';
GO
