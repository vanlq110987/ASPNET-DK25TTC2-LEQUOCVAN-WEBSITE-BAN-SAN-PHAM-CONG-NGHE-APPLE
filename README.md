# 🍎 AppleShop — Website Bán Sản Phẩm Công Nghệ của Apple

> Đồ án môn học · ASP.NET MVC 5 · SQL Server · Bootstrap SB Admin 2

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5-blue?style=flat-square)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.x-purple?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-red?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-3%2F4-blueviolet?style=flat-square)
![Language](https://img.shields.io/badge/Language-C%23-green?style=flat-square)
![Status](https://img.shields.io/badge/Status-Hoàn%20thành-brightgreen?style=flat-square)

---

## 📋 Thông tin đồ án

| Mục | Nội dung |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Sinh viên** | Lê Quốc Văn |
| **MSSV** | 170125090 |
| **Điện thoại** | 0988.534.534 |
| **Lớp** | DK25TTC2 |
| **Trường** | Trường Đại học Trà Vinh (TVU) |
| **Môn học** | Lập trình Web / Phát triển Ứng dụng Web |
| **Giảng viên hướng dẫn** | TS. Nguyễn Nhứt Lam |
| **Thời gian thực hiện** | 22/06/2026 — 01/08/2026 |
| **Công nghệ chính** | ASP.NET MVC 5, C#, SQL Server, Entity Framework, Bootstrap |

---

## 🎯 Giới thiệu đề tài

**AppleShop** là một website thương mại điện tử chuyên bán sản phẩm công nghệ của Apple (iPhone, MacBook, iPad, Apple Watch,...), được xây dựng bằng **ASP.NET MVC 5** theo mô hình **Model-View-Controller**, sử dụng **Entity Framework** làm ORM và **SQL Server** làm cơ sở dữ liệu.

Hệ thống cung cấp đầy đủ hai phân hệ:
- **Phía Khách hàng**: Duyệt sản phẩm, lọc theo kênh phân phối/nhu cầu, quản lý giỏ hàng, đặt hàng trực tuyến và xem lịch sử mua hàng.
- **Phía Admin**: Quản trị toàn bộ sản phẩm, kênh phân phối, đơn hàng, tin tức và phân quyền người dùng thông qua giao diện **SB Admin 2**.

---

## 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ / Thư viện |
|---|---|
| **Backend** | ASP.NET MVC 5, C#, .NET Framework 4.x |
| **ORM** | Entity Framework 6 (Code First / Database First) |
| **Cơ sở dữ liệu** | SQL Server (SSMS), T-SQL |
| **Xác thực & Phân quyền** | ASP.NET Identity (RBAC — Role-based Access Control) |
| **Frontend** | HTML5, CSS3, JavaScript, jQuery, Bootstrap 3/4 |
| **Template Admin** | SB Admin 2 (StartBootstrap) |
| **Soạn thảo nội dung** | CKEditor (WYSIWYG) |
| **AJAX** | jQuery AJAX / `$.post()` — cập nhật giỏ hàng không reload trang |
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
├── README.md
└── README.ORG.md                       ← (file này)
```

---

## 📚 Tài liệu tham khảo

1. Tài liệu chính thức Microsoft — ASP.NET MVC 5: https://docs.microsoft.com/aspnet/mvc
2. Khóa học lập trình **ASP.NET MVC** của TEDU (Tự học IT)
3. Template giao diện **SB Admin 2** — StartBootstrap
4. Tài liệu **Entity Framework 6** — Code First Migrations
5. Tài liệu **ASP.NET Identity** — User Management & Role Authorization
6. Giáo trình Phân tích và Thiết kế Hệ thống Thông tin (UML, ERD)
7. **Nghị định 30/2020/NĐ-CP** — Quy định về trình bày văn bản hành chính
8. Hướng dẫn trình bày đồ án tốt nghiệp — Trường Đại học Trà Vinh (TVU)

---

<div align="center">

**© 2026 Lê Quốc Văn — DK25TTC2 — Trường Đại học Trà Vinh**

*Đồ án được thực hiện vì mục đích học tập, không nhằm mục đích thương mại.*

</div>
