# BÁO CÁO TIẾN ĐỘ THỰC HIỆN ĐỒ ÁN - TUẦN 4

| Thông tin | Chi tiết |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Công nghệ** | ASP.NET MVC 5, SQL Server, Bootstrap SB Admin 2 |
| **Tuần thực hiện** | Tuần 4 |
| **Thời gian** | 13/07/2026 - 19/07/2026 |
| **Giai đoạn** | Hiện thực hóa Code Chức năng Quản trị (Phía Admin) và Bổ sung Tính năng Cải tiến |

---

## 1. Nội dung công việc đã thực hiện

### 1.1 Tích hợp giao diện quản trị SB Admin 2
- Nhúng toàn bộ CSS/JS của **SB Admin 2** (Bootstrap-based Admin Template) vào `_LayoutAdmin.cshtml`.
- Tạo `AdminArea` (hoặc cấu trúc thư mục riêng `/Views/Admin/`) để tách biệt giao diện quản trị khỏi giao diện khách hàng.
- Thiết lập sidebar navigation với các mục: Dashboard, Sản phẩm, Kênh phân phối, Đơn hàng, Tin tức, Tài khoản.

### 1.2 Chức năng CRUD cho Admin
- **Quản lý Sản phẩm công nghệ Apple (`Laptop`)**:
  - Danh sách sản phẩm có phân trang, tìm kiếm theo tên (iPhone, MacBook, iPad,...).
  - Thêm mới sản phẩm: upload hình ảnh, nhập thông số kỹ thuật,....
  - Sửa thông tin sản phẩm, cập nhật ảnh.
  - Xóa mềm (soft delete) hoặc xóa vĩnh viễn.
- **Quản lý Kênh phân phối**:
  - CRUD đầy đủ: thêm kênh phân phối mới, sửa tên, xóa (kiểm tra ràng buộc khóa ngoại với trước khi xóa).
- **Quản lý Đơn hàng**:
  - Xem danh sách đơn hàng, lọc theo trạng thái (Mới, Đang xử lý, Đã giao, Hủy).
  - Xem chi tiết từng đơn hàng (danh sách sản phẩm, thông tin khách hàng).
  - Cập nhật trạng thái đơn hàng.
- **Quản lý Tin tức (`TinTuc`) thuộc Chủ đề (`ChuDe`)**:
  - Viết, sửa, xóa bài viết; phân loại bài viết theo Chủ đề.
  - Tích hợp **CKEditor** (WYSIWYG) để soạn thảo nội dung HTML cho bài viết.
- **Quản lý Tài khoản và Phân quyền**:
  - Sử dụng `UserManager` và `RoleManager` của ASP.NET Identity.
  - Giao diện gán/thu hồi vai trò (Role) cho từng tài khoản người dùng.
  - Bảo vệ toàn bộ khu vực Admin bằng attribute `[Authorize(Roles = "Admin")]`.

---

## 2. Tài liệu liên quan đã tham khảo

- Tài liệu **ASP.NET Identity** - Quản lý người dùng với `UserManager<ApplicationUser>`.
- Tài liệu **ASP.NET Roles Authorization**: `RoleManager`, `[Authorize(Roles = "...")]`.
- Tài liệu tích hợp **CKEditor** với ASP.NET MVC.
- Tài liệu **SB Admin 2** Bootstrap Template (cấu trúc layout, sidebar, datatable).

---

## 3. Khó khăn khi viết thêm chức năng

Trong quá trình tích hợp thêm các tính năng nâng cao theo đề xuất cải tiến, gặp một số trở ngại đáng kể:

- **Tính năng thông báo nổi (Toast Notification) khi có đơn hàng mới**:
  - Yêu cầu cơ chế kiểm tra đơn hàng mới theo thời gian thực hoặc polling định kỳ từ phía Admin.
  - Thử nghiệm dùng `setInterval` + AJAX polling để kiểm tra đơn hàng mới, nhưng gặp khó khăn trong việc xác định "đơn hàng chưa đọc" (cần thêm cột `IsRead` vào bảng `DonHang` và đồng bộ trạng thái).
  - Tốn nhiều thời gian xử lý logic tránh thông báo trùng lặp.

- **Gợi ý thông minh kiểm tra trùng thông tin khách hàng**:
  - Khi Admin tạo đơn hàng mới, hệ thống cần tự động kiểm tra xem số điện thoại hoặc email đã tồn tại trong bảng chưa, từ đó gợi ý tự động điền thông tin.
  - Logic so khớp mờ (fuzzy matching) cho số điện thoại gặp khó khăn; phải xử lý nhiều trường hợp ngoại lệ (số điện thoại có/không có đầu số quốc tế, khoảng trắng...).

- **Cấu hình Realtime (SignalR)**:
  - Thử nghiệm tích hợp **ASP.NET SignalR** để đẩy thông báo đơn hàng mới theo thời gian thực lên Dashboard Admin.
  - Việc cấu hình Hub, kết nối JavaScript client, và xử lý sự kiện `OnConnectedAsync` tốn rất nhiều thời gian và gây xung đột với cấu hình Routing hiện tại. Tính năng này tạm thời chưa hoàn thiện.

---

## 4. Kết quả đạt được

- [x] Hoàn thiện toàn bộ các màn hình **CRUD của Admin**: Sản phẩm công nghệ Apple, Kênh phân phối, Đơn hàng, Tin tức thuộc Chủ đề.
- [x] **Phân quyền cơ bản** (Role-based Authorization) hoạt động đúng: tài khoản không có quyền Admin bị chặn và chuyển hướng về trang đăng nhập.
- [x] Giao diện Admin **SB Admin 2** được tích hợp đầy đủ, hiển thị đẹp trên desktop.
- [ ] Tính năng thông báo Realtime (SignalR): **chưa hoàn thiện**, dự kiến xem xét lại trong tuần 5.

---

## 5. Kế hoạch tuần tiếp theo (Tuần 5: 20/07/2026 - 01/08/2026)

- Kiểm thử toàn bộ hệ thống (kiểm thử hộp đen).
- Fix các bug phát sinh trong quá trình kiểm thử.
- Đánh giá ưu/nhược điểm, viết phần Kết luận và Hướng phát triển.
- Hoàn thiện báo cáo tổng hợp và chuẩn bị slide thuyết trình.
