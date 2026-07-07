# BÁO CÁO TIẾN ĐỘ THỰC HIỆN ĐỒ ÁN - TUẦN 2

| Thông tin | Chi tiết |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Công nghệ** | ASP.NET MVC 5, SQL Server, Bootstrap |
| **Tuần thực hiện** | Tuần 2 |
| **Thời gian** | 29/06/2026 - 05/07/2026 |
| **Giai đoạn** | Phân tích Thiết kế Hệ thống và Cơ sở Dữ liệu |

---

## 1. Nội dung công việc đã thực hiện

### 1.1 Khảo sát và Phân tích yêu cầu
- Khảo sát thực tế quy trình vận hành của một cửa hàng sản phẩm công nghệ tương tự Apple: quy trình tiếp nhận đơn hàng, quản lý kho, đăng bài viết trên trang bán sản phẩm. Đăng bài tin tức các chương trình khuyến mãi
- Phân tích và xác định **14 chức năng chính** chia làm hai nhóm:
  - **Phía Khách hàng (7 chức năng)**: Xem danh sách sản phẩm, Xem chi tiết sản phẩm, Bộ lọc sản phẩm theo Hãng/Nhu cầu, Quản lý Giỏ hàng, Đặt hàng trực tuyến, Xem lịch sử đơn hàng, Đọc tin tức về các chương trình khuyến mãi và Bình luận.
  - **Phía Quản trị Admin (7 chức năng)**: Quản lý Sản phẩm công nghệ Apple (iPhone, MacBook, iPad,...), Quản lý kênh phân phối sỉ, Quản lý Đơn hàng, Quản lý Tin tức thuộc các Chủ đề, Quản lý Tài khoản người dùng, Phân quyền vai trò quản trị.
- Xác định **yêu cầu phi chức năng**:
  - Bảo mật: Phân quyền dựa trên Roles, mã hóa mật khẩu qua ASP.NET Identity.
  - Hiệu năng: Trang tải nhanh, tối ưu truy vấn SQL.
  - Tương thích: Giao diện responsive trên desktop, tablet, mobile.

### 1.2 Thiết kế hệ thống
- Vẽ **sơ đồ Use-case tổng quát mức 0** với 2 tác nhân chính:
  - **Khách hàng**: tương tác với 7 Use Case bao gồm: xem danh sách sản phẩm, tìm kiếm sản phẩm, xem chi tiết sản phẩm, thêm vào giỏ hàng,  đến đặt hàng/thanh toán, bình luận về sản phẩm và xem lịch sử đơn hàng.
  - **Admin**: tương tác với 7 Use Case sử dụng quản trị bao gồm: quản lý các sản phẩm Apple, quản ký kênh phân phối, quản lý đơn hàng, Quản lý tin tức, phân quyền người dùng cuối, phân quyền người dùng quản trị trong back-end và xem các báo cáo thống kê bán hàng.
- Thiết kế **mô hình quan hệ thực thể (ERD)** cho toàn bộ hệ thống trên SQL Server, gồm 16 bảng chính:

### Bảng ASP.NET Core Identity (mặc định - 6 bảng)

| STT | Tên bảng              | Mục đích chính                          |
|-----|------------------------|-----------------------------------------|
| 1   | AspNetUsers            | Người dùng (Khách hàng + Admin)        |
| 2   | AspNetRoles            | Vai trò (Customer, Admin, Manager...)  |
| 3   | AspNetUserRoles        | Liên kết Người dùng - Vai trò          |
| 4   | AspNetUserClaims       | Claim chi tiết của người dùng          |
| 5   | AspNetUserLogins       | Đăng nhập bên thứ 3 (Google, Apple, ...) |
| 6   | AspNetRoleClaims       | Claim của Vai trò                       |

### Bảng nghiệp vụ (15 bảng)

| STT | Tên bảng                | Mục đích chính                                      | Use Case hỗ trợ                              |
|-----|-------------------------|-----------------------------------------------------|----------------------------------------------|
| 7   | Categories              | Danh mục sản phẩm (iPhone, MacBook, iPad...)       | Xem/Tìm kiếm sản phẩm                        |
| 8   | Products                | Thông tin sản phẩm Apple (iPhone 16 Pro, MacBook Air M3...) | Xem chi tiết, quản lý sản phẩm          |
| 9   | ProductVariants         | Biến thể sản phẩm (màu sắc, dung lượng, cấu hình)  | Xem chi tiết, thêm vào giỏ hàng, quản lý sản phẩm |
| 10  | ProductImages           | Hình ảnh sản phẩm/biến thể                         | Xem chi tiết sản phẩm                        |
| 11  | DistributionChannels    | Kênh phân phối (Apple Store, FPT, CellphoneS, Website...) | Quản lý kênh phân phối                  |
| 12  | Inventories             | Tồn kho theo biến thể + kênh phân phối             | Quản lý sản phẩm + kênh                      |
| 13  | Carts                   | Giỏ hàng của khách hàng                            | Thêm vào giỏ hàng                            |
| 14  | CartItems               | Chi tiết sản phẩm trong giỏ hàng                   | Thêm vào giỏ hàng                            |
| 15  | Orders                  | Đơn hàng                                           | Đặt hàng, Quản lý đơn hàng, Lịch sử đơn hàng |
| 16  | OrderItems              | Chi tiết sản phẩm trong đơn hàng                   | Đặt hàng, Quản lý đơn hàng                   |
| 17  | Payments                | Thanh toán đơn hàng                                | Thanh toán / Đặt hàng                       |
| 18  | ProductReviews          | Bình luận & đánh giá sản phẩm                      | Bình luận sản phẩm                          |
| 19  | NewsArticles            | Tin tức, bài viết, khuyến mãi                      | Quản lý tin tức                              |
| 20  | Permissions             | Danh sách quyền chi tiết (Products.Create, Orders.Approve...) | Phân quyền cho Admin & User           |
| 21  | RolePermissions         | Liên kết Vai trò - Quyền (RBAC chi tiết)          | Phân quyền người dùng                     |

> **Ghi chú:**
> Thiết kế này hỗ trợ đầy đủ **14 Use Case** của Khách hàng và Admin trong hệ thống Thương mại điện tử Apple.
> Các bảng được thiết kế tối ưu cho SQL Server và tích hợp sẵn với ASP.NET Core Identity. 

---

## 2. Tài liệu liên quan đã tham khảo

- Giáo trình Phân tích và Thiết kế hệ thống thông tin (UML - Use-case, ERD).
- Tài liệu quản trị **SQL Server** - Microsoft Docs.
- Tài liệu đặc tả bảng **ASP.NET Identity** (AspNetUsers, AspNetRoles, AspNetUserRoles,...).

---

## 3. Khó khăn gặp phải

- **Thiết kế mô hình dữ liệu mở rộng cho sản phẩm Apple**: Việc phân tách bảng `Products` và `ProductVariants` để quản lý nhiều cấu hình (màu sắc, dung lượng, RAM, chip…) gặp khó khăn trong việc đảm bảo tính nhất quán dữ liệu và hiệu năng truy vấn khi kết hợp với bảng `Inventories` theo từng kênh phân phối.
- **Quản lý tồn kho theo kênh phân phối**: Thiết kế quan hệ nhiều-nhiều giữa `ProductVariants` và `DistributionChannels` thông qua bảng `Inventories` đòi hỏi phải xử lý tốt các ràng buộc toàn vẹn dữ liệu, đồng thời hỗ trợ cập nhật số lượng tồn kho theo thời gian thực.
- **Mở rộng ASP.NET Core Identity**: Hệ thống phân quyền mặc định chỉ cung cấp vai trò cơ bản. Để đáp ứng yêu cầu **phân quyền chi tiết** cho cả Admin (quản lý sản phẩm, đơn hàng, tin tức…) và người dùng, cần thiết kế thêm bảng `Permissions` và `RolePermissions` để xây dựng mô hình RBAC linh hoạt mà vẫn giữ nguyên cấu trúc Identity.
- **Cân bằng giữa chuẩn hóa và hiệu năng**: Việc tách nhỏ bảng (`ProductVariants`, `Inventories`, `OrderItems`…) giúp giảm dư thừa nhưng làm tăng số lượng JOIN khi truy vấn báo cáo thống kê và lịch sử đơn hàng.

---

## 4. Kết quả đạt được

- [x] Hoàn thành **sơ đồ Use-case tổng quát mức 0** với 2 tác nhân chính (Khách hàng và Admin), mỗi tác nhân có 7 use case.
- [x] Hoàn thành **mô hình ERD** chi tiết cho hệ thống với tổng cộng **21 bảng** (6 bảng ASP.NET Core Identity + 15 bảng nghiệp vụ).
- [x] Thiết kế đầy đủ các bảng nghiệp vụ chính: `Categories`, `Products`, `ProductVariants`, `ProductImages`, `DistributionChannels`, `Inventories`, `Carts`, `CartItems`, `Orders`, `OrderItems`, `Payments`, `ProductReviews`, `NewsArticles`, `Permissions`, `RolePermissions`.
- [x] Xây dựng được mô hình phân quyền chi tiết (RBAC) thông qua bảng `Permissions` và `RolePermissions`, đáp ứng yêu cầu phân quyền cho cả người dùng cuối và quản trị viên back-end.
- [x] Hoàn thành tài liệu mô tả các bảng dữ liệu và mối quan hệ giữa chúng dưới dạng Markdown, sẵn sàng để phát triển tiếp script SQL và From điển dữ liệu chi tiết.

---

## 5. Kế hoạch tuần tiếp theo (Tuần 3: 06/07/2026 - 12/07/2026)

- Tạo dự án ASP.NET MVC 5 trong Visual Studio, cấu hình kết nối CSDL trong `Web.config`.
- Viết code các chức năng phía Khách hàng: Trang chủ, danh sách sản phẩm Apple, chi tiết sản phẩm.
- Xây dựng logic Giỏ hàng và chức năng Đặt hàng trực tuyến.
