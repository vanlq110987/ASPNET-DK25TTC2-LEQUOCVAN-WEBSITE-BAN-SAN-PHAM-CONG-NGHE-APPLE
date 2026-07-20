# 🍎 AppleShop — Website Bán Sản Phẩm Công Nghệ của Apple

> Đồ án môn học · ASP.NET MVC 5 · SQL Server · Bootstrap SB Admin 2

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5.2.9-blue?style=flat-square)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-purple?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2017%2B-red?style=flat-square)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.4.4-orange?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-4.6-blueviolet?style=flat-square)
![Language](https://img.shields.io/badge/Language-C%23-green?style=flat-square)
![Status](https://img.shields.io/badge/Status-Hoàn%20thành-brightgreen?style=flat-square)

📦 **Repository:** https://github.com/vanlq110987/ASPNET-DK25TTC2-LEQUOCVAN-WEBSITE-BAN-SAN-PHAM-CONG-NGHE-APPLE

---

## 📋 Thông tin đồ án

| Mục | Nội dung |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Sinh viên** | Lê Quốc Văn |
| **MSSV** | 170125090 |
| **Lớp** | DK25TTC2 |
| **Trường** | Trường Đại học Trà Vinh (TVU) |
| **Môn học** | Lập trình Web / Phát triển Ứng dụng Web |
| **Giảng viên hướng dẫn** | TS. Nguyễn Nhứt Lam |
| **Thời gian thực hiện** | 22/06/2026 — 01/08/2026 |
| **Công nghệ chính** | ASP.NET MVC 5, C#, SQL Server, Entity Framework 6, Bootstrap |

---

## 🎯 Giới thiệu đề tài

**AppleShop** là một website thương mại điện tử chuyên bán sản phẩm công nghệ của Apple (iPhone, MacBook, iPad, Apple Watch,...), được xây dựng bằng **ASP.NET MVC 5** theo mô hình **Model-View-Controller**, sử dụng **Entity Framework 6** làm ORM và **SQL Server** làm cơ sở dữ liệu.

Hệ thống cung cấp đầy đủ hai phân hệ:
- **Phía Khách hàng**: Duyệt sản phẩm, lọc theo kênh phân phối/nhu cầu/giá, quản lý giỏ hàng (AJAX), đặt hàng trực tuyến (COD), xem lịch sử mua hàng, đọc tin tức và bình luận sản phẩm.
- **Phía Admin**: Quản trị toàn bộ sản phẩm, kênh phân phối, đơn hàng, tin tức và phân quyền người dùng thông qua giao diện **SB Admin 2** với Dashboard thống kê và thông báo đơn hàng mới.

---

## 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ / Thư viện |
|---|---|
| **Backend** | ASP.NET MVC 5.2.9, C#, .NET Framework 4.8 |
| **ORM** | Entity Framework 6.4.4 (mapping vào CSDL tạo bằng script) |
| **Cơ sở dữ liệu** | SQL Server 2017+ (SSMS), T-SQL |
| **Xác thực & Phân quyền** | ASP.NET Identity 2.2 (RBAC — Role-based Access Control, OWIN Cookie) |
| **Frontend** | HTML5, CSS3, JavaScript, jQuery 3.7, Bootstrap 4.6 |
| **Template Admin** | SB Admin 2 v4.1.4 (StartBootstrap) |
| **Soạn thảo nội dung** | CKEditor 4 (WYSIWYG) — bài viết tin tức & mô tả sản phẩm |
| **Biểu đồ thống kê** | Chart.js 2.9 — doanh thu 7 ngày trên Dashboard |
| **AJAX** | jQuery AJAX / `$.post()` — cập nhật giỏ hàng không reload trang; polling thông báo đơn mới |
| **Môi trường phát triển** | Visual Studio 2019/2022, IIS Express |

> ⚠️ **Lưu ý:** Bootstrap, jQuery, Font Awesome, SB Admin 2, CKEditor và Chart.js được nạp qua **CDN** — máy chạy demo cần có kết nối Internet.

---

## 📁 Cấu trúc thư mục repository

```
ASPNET-DK25TTC2-LEQUOCVAN-WEBSITE-BAN-SAN-PHAM-CONG-NGHE-APPLE/
│
├── scr/                                ← Mã nguồn + CSDL
│   ├── AppleShopDB_Script.sql          ← Script tạo CSDL (21 bảng + dữ liệu mẫu)
│   └── AppleShop/                      ← Dự án ASP.NET MVC 5
│       ├── AppleShop.sln               ← Solution mở bằng Visual Studio
│       ├── AppleShop.csproj
│       ├── Web.config                  ← Connection string, cấu hình EF/Identity
│       ├── Global.asax / Global.asax.cs
│       ├── Startup.cs                  ← OWIN Startup
│       │
│       ├── App_Start/
│       │   ├── BundleConfig.cs         ← Bundle CSS/JS tự viết
│       │   ├── FilterConfig.cs
│       │   ├── IdentityConfig.cs       ← UserManager, RoleManager + IdentitySeeder
│       │   ├── RouteConfig.cs          ← Routing (/san-pham/{slug}, /tin-tuc/{slug})
│       │   └── Startup.Auth.cs         ← Cấu hình OWIN Cookie Authentication
│       │
│       ├── Areas/Admin/                ← Khu vực quản trị (SB Admin 2)
│       │   ├── AdminAreaRegistration.cs
│       │   ├── Controllers/
│       │   │   ├── AdminControllerBase.cs   ← [Authorize(Roles="Admin")]
│       │   │   ├── HomeController.cs        ← Dashboard + polling đơn mới
│       │   │   ├── ProductController.cs     ← CRUD sản phẩm, upload ảnh, xóa mềm
│       │   │   ├── ChannelController.cs     ← CRUD kênh phân phối (kiểm tra FK)
│       │   │   ├── OrderController.cs       ← Lọc, xem, cập nhật trạng thái đơn
│       │   │   ├── NewsController.cs        ← CRUD tin tức + chủ đề (CKEditor)
│       │   │   └── AccountController.cs     ← CRUD tài khoản, khóa/mở, phân quyền Role
│       │   └── Views/ (Home, Product, Channel, Order, News, Account, Shared/_LayoutAdmin.cshtml)
│       │
│       ├── Controllers/                ← Controllers phía Khách hàng
│       │   ├── HomeController.cs
│       │   ├── ProductController.cs    ← Danh sách + bộ lọc + chi tiết + bình luận
│       │   ├── CartController.cs       ← Giỏ hàng Session + AJAX JsonResult
│       │   ├── OrderController.cs      ← Checkout (transaction), History
│       │   ├── NewsController.cs
│       │   └── AccountController.cs    ← Đăng ký / Đăng nhập (Identity)
│       │
│       ├── Models/                     ← 15 entity nghiệp vụ + IdentityModels
│       │   ├── IdentityModels.cs       ← ApplicationUser + ApplicationDbContext
│       │   ├── Category.cs, Product.cs, ProductVariant.cs, ProductImage.cs
│       │   ├── DistributionChannel.cs, Inventory.cs
│       │   ├── Cart.cs, CartItem.cs, Customer.cs
│       │   ├── Order.cs, OrderItem.cs, Payment.cs
│       │   ├── ProductReview.cs, NewsCategory.cs, NewsArticle.cs
│       │   └── ViewModels/             ← LoginVM, RegisterVM, CheckoutVM, DashboardVM,...
│       │
│       ├── Helpers/SlugHelper.cs       ← Tạo slug an toàn từ tiếng Việt
│       ├── Views/                      ← Views phía Khách hàng (Bootstrap 4)
│       ├── Content/Site.css            ← CSS tùy biến + thư mục uploads/
│       └── Scripts/site.js             ← AJAX giỏ hàng
│
├── soft/                               ← Bộ cài phần mềm cần thiết
│   ├── README.md                       ← Hướng dẫn thứ tự cài đặt
│   ├── vs_community_2022_setup.exe     ← Visual Studio 2022 Community (bootstrapper)
│   ├── SQL2022-Express_setup.exe       ← SQL Server 2022 Express
│   └── download-offline-installers.ps1 ← Script tải SSMS + .NET 4.8 Dev Pack (offline)
│
├── Thesis/                             ← Tài liệu Đồ án
│   ├── doc/                            ← Tài liệu dạng .DOC (báo cáo, bìa)
│   ├── pdf/                            ← Tài liệu dạng .PDF
│   ├── html/                           ← Tài liệu dạng web
│   ├── abs/                            ← Báo cáo thuyết trình (.PPT, .AVI, ...)
│   └── refs/                           ← Tài liệu tham khảo
│
├── Progress_Report/                    ← Báo cáo tiến độ hàng tuần (đặc tả yêu cầu)
│   ├── Tuan_1.md   ← Nghiên cứu lý thuyết
│   ├── Tuan_2.md   ← Phân tích thiết kế + ERD 21 bảng
│   ├── Tuan_3.md   ← Chức năng phía Khách hàng
│   ├── Tuan_4.md   ← Chức năng phía Admin
│   └── Tuan_5.md   ← Kiểm thử & hoàn thiện
│
├── .gitignore
└── README.md                            ← (file này)
```

---

## ⚙️ Cài đặt và chạy dự án

### Yêu cầu hệ thống

| Phần mềm | Phiên bản yêu cầu |
|---|---|
| Windows | 10 / 11 |
| Visual Studio | 2019 hoặc 2022 (workload **ASP.NET and web development**) |
| SQL Server | 2017 / 2019 / 2022 / Express |
| SQL Server Management Studio | 18+ |
| .NET Framework | 4.8 (tích hợp sẵn trong Windows/VS) |
| Internet | Cần cho CDN (Bootstrap, SB Admin 2, CKEditor,...) và NuGet restore |

### Các bước cài đặt

**Bước 1 — Tải mã nguồn**
```bash
git clone https://github.com/vanlq110987/ASPNET-DK25TTC2-LEQUOCVAN-WEBSITE-BAN-SAN-PHAM-CONG-NGHE-APPLE.git
```

**Bước 2 — Tạo cơ sở dữ liệu**

Mở **SQL Server Management Studio**, kết nối đến server cục bộ, mở và chạy toàn bộ file:
```
scr/AppleShopDB_Script.sql
```
Script tự tạo database `AppleShopDB` (nếu chưa có), 21 bảng và dữ liệu mẫu (danh mục, sản phẩm, kênh phân phối, tin tức, bình luận).

**Bước 3 — Cấu hình kết nối CSDL**

Mở file `scr/AppleShop/Web.config`, cập nhật `connectionStrings` theo SQL Server cục bộ:
```xml
<connectionStrings>
  <add name="AppleShopContext"
       connectionString="Data Source=.;Initial Catalog=AppleShopDB;Integrated Security=True;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```
> Nếu dùng SQL Server Express: đổi `Data Source=.` thành `Data Source=.\SQLEXPRESS`.

**Bước 4 — Mở solution và Restore NuGet Packages**

Mở `scr/AppleShop/AppleShop.sln` trong Visual Studio → chuột phải Solution → **Restore NuGet Packages** (dự án dùng PackageReference, VS tự restore khi build).

**Bước 5 — Chạy dự án**

Nhấn **F5** hoặc **Ctrl+F5**. Trình duyệt tự mở tại `http://localhost:<port>/`.

> Lần chạy đầu tiên, ứng dụng tự tạo 2 vai trò (**Admin**, **Customer**) và 2 tài khoản mặc định (IdentitySeeder).

---

## 🔐 Tài khoản đăng nhập mặc định

| Vai trò | Email | Mật khẩu |
|---|---|---|
| **Admin** | admin@appleshop.vn | Admin@123456 |
| **Khách hàng** | customer@gmail.com | Customer@123 |

> Tài khoản đăng ký mới mặc định được gán vai trò **Customer**. Chỉ Admin mới có thể cấp/thu hồi vai trò trong trang quản trị (`/Admin/Account`).

---

## 🔗 Các link truy cập

### Phía Khách hàng
| Trang | Đường dẫn |
|---|---|
| Trang chủ | `/` |
| Danh sách sản phẩm (kèm bộ lọc) | `/Product` |
| Chi tiết sản phẩm | `/san-pham/{slug}` |
| Giỏ hàng | `/Cart` |
| Đặt hàng | `/Order/Checkout` |
| Lịch sử đơn hàng | `/Order/History` |
| Tin tức | `/News` |
| Chi tiết bài viết | `/tin-tuc/{slug}` |
| Đăng ký | `/Account/Register` |
| Đăng nhập | `/Account/Login` |

### Phía Admin (yêu cầu vai trò Admin)
| Trang | Đường dẫn |
|---|---|
| Dashboard | `/Admin` |
| Quản lý Sản phẩm | `/Admin/Product` |
| Quản lý Kênh phân phối | `/Admin/Channel` |
| Quản lý Đơn hàng | `/Admin/Order` |
| Quản lý Tin tức & Chủ đề | `/Admin/News` |
| Quản lý Tài khoản & Phân quyền | `/Admin/Account` |

---

## ✅ Chức năng chính (14 chức năng)

### Phía Khách hàng (7 chức năng)

| STT | Chức năng | Mô tả |
|---|---|---|
| 1 | Xem danh sách sản phẩm | Phân trang (9 SP/trang), hiển thị tên, ảnh, giá và giá sale |
| 2 | Xem chi tiết sản phẩm | Thông số kỹ thuật, chọn biến thể (màu/dung lượng), kênh còn hàng |
| 3 | Bộ lọc sản phẩm | Lọc theo danh mục, kênh phân phối, khoảng giá, nhu cầu sử dụng; sắp xếp; tìm kiếm |
| 4 | Quản lý Giỏ hàng | Thêm, cập nhật số lượng (AJAX không reload), xóa sản phẩm — lưu Session |
| 5 | Đặt hàng trực tuyến | Nhập thông tin giao hàng, lưu đơn bằng transaction, thanh toán COD |
| 6 | Xem lịch sử đơn hàng | Danh sách + chi tiết đơn đã đặt theo tài khoản đăng nhập |
| 7 | Đọc tin tức & Bình luận | Xem bài viết theo chủ đề, gửi bình luận + đánh giá sao sản phẩm |

### Phía Quản trị Admin (7 chức năng)

| STT | Chức năng | Mô tả |
|---|---|---|
| 1 | Quản lý Sản phẩm Apple | CRUD + tìm kiếm + phân trang; upload ảnh; xóa mềm / xóa vĩnh viễn |
| 2 | Quản lý Kênh phân phối | CRUD; chặn xóa khi còn tồn kho tham chiếu (ràng buộc FK) |
| 3 | Quản lý Đơn hàng | Lọc theo trạng thái (Mới/Xử lý/Giao/Hủy), cập nhật trạng thái, ghi nhận thanh toán COD |
| 4 | Quản lý Tin tức & Chủ đề | Viết/sửa/xóa bài bằng CKEditor; quản lý chủ đề bài viết |
| 5 | Quản lý Tài khoản người dùng | Thêm/sửa/xóa tài khoản, đổi mật khẩu, tìm kiếm, khóa/mở khóa; xóa tài khoản vẫn giữ lịch sử đơn hàng |
| 6 | Phân quyền vai trò quản trị | Gán/thu hồi Role (Admin/Customer) qua ASP.NET Identity; không thể tự khóa/xóa/phân quyền chính mình (nút mờ đi, chặn cả phía server); bảo vệ Admin cuối cùng |
| 7 | Xem báo cáo thống kê | Dashboard: đơn mới, doanh thu, top 5 bán chạy, biểu đồ 7 ngày, thông báo đơn mới (polling) |

---

## 🗄️ Cơ sở dữ liệu — `AppleShopDB` (21 bảng)

### Bảng ASP.NET Identity (6 bảng)

| STT | Tên bảng | Mục đích |
|---|---|---|
| 1 | `AspNetUsers` | Tài khoản người dùng (mở rộng: FullName, Address, CreatedAt) |
| 2 | `AspNetRoles` | Vai trò (Admin, Customer) |
| 3 | `AspNetUserRoles` | Liên kết Người dùng — Vai trò |
| 4 | `AspNetUserClaims` | Claim chi tiết của người dùng |
| 5 | `AspNetUserLogins` | Đăng nhập bên thứ 3 (Google, Apple,...) |
| 6 | `AspNetRoleClaims` | Claim của Vai trò |

### Bảng nghiệp vụ (15 bảng — tự thiết kế)

| STT | Tên bảng | Mục đích |
|---|---|---|
| 7 | `Categories` | Danh mục sản phẩm (iPhone, MacBook, iPad, Apple Watch, Phụ kiện) |
| 8 | `Products` | Thông tin sản phẩm Apple (giá, giá sale, nhu cầu, slug, ảnh) |
| 9 | `ProductVariants` | Biến thể: màu sắc, dung lượng, cấu hình, chênh lệch giá, SKU |
| 10 | `ProductImages` | Hình ảnh sản phẩm/biến thể |
| 11 | `DistributionChannels` | Kênh phân phối (Apple Store, FPT Shop, CellphoneS, TopZone,...) |
| 12 | `Inventories` | Tồn kho theo biến thể × kênh phân phối (unique constraint) |
| 13 | `Carts` | Giỏ hàng lưu CSDL (theo UserId / SessionId) |
| 14 | `CartItems` | Chi tiết sản phẩm trong giỏ hàng |
| 15 | `Customers` | Thông tin người nhận hàng |
| 16 | `Orders` | Đơn hàng (mã đơn, trạng thái, tổng tiền, cờ IsRead cho thông báo) |
| 17 | `OrderItems` | Chi tiết đơn — snapshot tên/giá tại thời điểm đặt |
| 18 | `Payments` | Thanh toán đơn hàng (COD) |
| 19 | `ProductReviews` | Bình luận & đánh giá sao (1–5) |
| 20 | `NewsCategories` | Chủ đề phân loại bài viết |
| 21 | `NewsArticles` | Bài viết tin tức, khuyến mãi (HTML từ CKEditor) |

> **Ghi chú:** CSDL được tạo hoàn toàn bằng script `scr/AppleShopDB_Script.sql`; EF6 map vào schema có sẵn (initializer = null, không dùng migration).

---

## 📅 Tiến độ thực hiện

| Tuần | Thời gian | Giai đoạn | Nội dung chính | Trạng thái |
|---|---|---|---|---|
| Tuần 1 | 22/06 – 28/06 | Nghiên cứu lý thuyết | Tìm hiểu ASP.NET MVC 5, HTTP, HTML/CSS/JS/jQuery/Bootstrap; cài đặt môi trường; hoàn thành đề cương và Chương 1 báo cáo | ✅ Hoàn thành |
| Tuần 2 | 29/06 – 05/07 | Phân tích & Thiết kế | Khảo sát yêu cầu (14 chức năng), vẽ Use-case, thiết kế ERD 21 bảng, xây dựng từ điển dữ liệu | ✅ Hoàn thành |
| Tuần 3 | 06/07 – 12/07 | Code phía Khách hàng | Trang chủ, danh sách sản phẩm, bộ lọc, giỏ hàng (AJAX), đặt hàng, lịch sử mua hàng | ✅ Hoàn thành |
| Tuần 4 | 13/07 – 19/07 | Code phía Admin | Tích hợp SB Admin 2; CRUD sản phẩm, kênh phân phối, đơn hàng, tin tức; phân quyền Identity; thông báo đơn mới | ✅ Hoàn thành |
| Tuần 5 | 20/07 – 01/08 | Kiểm thử & Hoàn thiện | Black-box testing 11 test case; fix bug slug/tiền tệ; viết báo cáo + slide thuyết trình | ✅ Hoàn thành |

---

## 🧪 Kết quả kiểm thử

Kiểm thử hộp đen (Black-box Testing) trên môi trường **Localhost (IIS Express)** — 11/11 test case đạt:

| STT | Chức năng kiểm thử | Kết quả |
|---|---|---|
| 1 | Đăng ký tài khoản mới | ✅ Đạt |
| 2 | Đăng nhập / Đăng xuất | ✅ Đạt |
| 3 | Xem danh sách & chi tiết sản phẩm | ✅ Đạt |
| 4 | Bộ lọc sản phẩm theo kênh phân phối/Nhu cầu | ✅ Đạt |
| 5 | Thêm sản phẩm vào giỏ hàng | ✅ Đạt |
| 6 | Cập nhật số lượng / Xóa giỏ hàng | ✅ Đạt |
| 7 | Đặt hàng và xem lịch sử đơn hàng | ✅ Đạt |
| 8 | Admin — CRUD Sản phẩm công nghệ Apple | ✅ Đạt |
| 9 | Admin — CRUD Đơn hàng | ✅ Đạt |
| 10 | Admin — Phân quyền Role | ✅ Đạt |
| 11 | Hiển thị responsive trên mobile | ✅ Đạt |

---

## 📊 Đánh giá hệ thống

### Ưu điểm
- Hệ thống vận hành **ổn định**, luồng mua hàng từ đầu đến cuối được bao trong transaction, không có lỗi nghiêm trọng.
- Giao diện **thân thiện, trực quan**, responsive đa thiết bị (máy tính, tablet, điện thoại).
- Phân quyền rõ ràng giữa Khách hàng và Admin nhờ ASP.NET Identity (`[Authorize(Roles = "Admin")]` toàn khu vực quản trị).
- Toàn bộ form POST có **Anti-Forgery Token** chống CSRF.
- Code có cấu trúc rõ ràng theo mô hình MVC, slug URL thân thiện SEO, dễ bảo trì và mở rộng.

### Nhược điểm & Hướng phát triển

| Nhược điểm hiện tại | Hướng khắc phục |
|---|---|
| Chưa áp dụng HTTPS trên môi trường triển khai | Bật HTTPS + HSTS khi deploy IIS/Azure |
| Thông báo đơn hàng mới dùng AJAX polling (30s) | Nâng cấp lên **ASP.NET SignalR** realtime |
| Chỉ hỗ trợ thanh toán COD | Tích hợp cổng thanh toán **VNPay / MoMo** |
| Giỏ hàng lưu Session (mất khi hết phiên) | Đồng bộ giỏ hàng vào bảng `Carts`/`CartItems` theo tài khoản |
| Chưa có mobile app | Phát triển thêm **React Native / Flutter App** |
| Khảo sát thực tế còn hạn chế | Mở rộng UX research với người dùng thực |

---

## 🆕 Nhật ký cập nhật

| Ngày | Nội dung |
|---|---|
| 20/07/2026 | Hoàn thành **Tuần 5**: kiểm thử hộp đen 11/11 test case, viết Kết luận & Hướng phát triển, hoàn thiện báo cáo và slide thuyết trình (xem `Progress_Report/Tuan_5.md`) |
| 20/07/2026 | Dọn dẹp tài liệu: gộp nội dung `README.ORG.md` vào `README.md` rồi xóa file trùng lặp |
| 09/07/2026 | **Quản lý tài khoản nâng cao**: Admin thêm/sửa/xóa tài khoản người dùng, đổi mật khẩu, chọn vai trò khi tạo; xóa tài khoản vẫn giữ lịch sử đơn hàng (gỡ liên kết UserId) |
| 09/07/2026 | **Bảo vệ tài khoản đang đăng nhập**: nút Khóa / Xóa / Phân quyền của chính mình bị mờ (disabled) trên giao diện và chặn cả phía server; không cho xóa Admin cuối cùng |
| 09/07/2026 | Fix routing: thêm `namespaces` cho route `/san-pham/{slug}`, `/tin-tuc/{slug}` (hết lỗi trùng tên ProductController giữa 2 khu vực) |
| 09/07/2026 | Fix font tiếng Việt: khai báo `fileEncoding/requestEncoding/responseEncoding=utf-8` + thêm BOM UTF-8 cho toàn bộ file nguồn |
| 09/07/2026 | Fix runtime: đồng bộ bindingRedirect (Antlr3.Runtime 3.4.1.9004, WebGrease 1.5.2.14234), ghim `Microsoft.Owin.Security.OAuth 4.2.2` |
| 09/07/2026 | Fix SSMS: gán owner `sa` cho AppleShopDB trong script để tạo được Database Diagrams (sơ đồ ERD) |
| 09/07/2026 | Thêm thư mục `soft/`: bộ cài VS 2022 Community, SQL Server 2022 Express, script tải SSMS + .NET 4.8 Dev Pack |

---

## 📚 Tài liệu tham khảo

1. Tài liệu chính thức Microsoft — ASP.NET MVC 5: https://learn.microsoft.com/aspnet/mvc
2. Khóa học lập trình **ASP.NET MVC** của TEDU (Tự học IT)
3. Template giao diện **SB Admin 2** — StartBootstrap: https://startbootstrap.com/theme/sb-admin-2
4. Tài liệu **Entity Framework 6**: https://learn.microsoft.com/ef/ef6/
5. Tài liệu **ASP.NET Identity 2** — User Management & Role Authorization
6. Giáo trình Phân tích và Thiết kế Hệ thống Thông tin (UML, ERD)
7. **Nghị định 30/2020/NĐ-CP** — Quy định về trình bày văn bản hành chính
8. Hướng dẫn trình bày đồ án tốt nghiệp — Trường Đại học Trà Vinh (TVU)

---

<div align="center">

**© 2026 Lê Quốc Văn — DK25TTC2 — Trường Đại học Trà Vinh**

*Đồ án được thực hiện vì mục đích học tập, không nhằm mục đích thương mại.*

</div>
