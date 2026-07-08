# BÁO CÁO TIẾN ĐỘ THỰC HIỆN ĐỒ ÁN - TUẦN 3

| Thông tin | Chi tiết |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Công nghệ** | ASP.NET MVC 5, SQL Server, Bootstrap |
| **Tuần thực hiện** | Tuần 3 |
| **Thời gian** | 06/07/2026 - 12/07/2026 |
| **Giai đoạn** | Hiện thực hóa Code Chức năng Cốt lõi (Phía Khách hàng) |

---

## 1. Nội dung công việc đã thực hiện

### 1.1 Khởi tạo và cấu hình dự án
- Tạo dự án **ASP.NET MVC 5** mới trong Visual Studio với template có tích hợp sẵn ASP.NET Identity.
- Cấu hình chuỗi kết nối cơ sở dữ liệu trong file `Web.config`:
  ```xml
  <connectionStrings>
    <add name="AppleShopContext"
         connectionString="Data Source=.;Initial Catalog=AppleShopDB;Integrated Security=True"
         providerName="System.Data.SqlClient" />
  </connectionStrings>
  ```
- Cài đặt các package NuGet cần thiết: `EntityFramework`, `Microsoft.AspNet.Identity.EntityFramework`, `Bootstrap`, `jQuery`.
- Tạo cấu trúc thư mục dự án theo chuẩn MVC: `Models/`, `Views/`, `Controllers/`, `Content/`, `Scripts/`.

### 1.2 Chức năng phía Khách hàng
- **Trang chủ (`HomeController`)**: Hiển thị banner, sản phẩm nổi bật, sản phẩm mới nhất, tin tức.
- **Danh sách sản phẩm (`ProductController`)**: Phân trang danh sách sản phẩm công nghệ Apple theo danh mục ( iPhone, MacBook, iPad,...), hiển thị tên, hình ảnh, giá và giá sale (nếu có).
- **Bộ lọc sản phẩm**: Lọc theo giá, sản phẩm bán chạy, theo danh mục sản phẩm, theo cấu hình sản phẩm và theo Nhu cầu (học tập, đồ họa, văn phòng nếu chọn Macbook)...; sử dụng LINQ để truy vấn động.
- **Chi tiết sản phẩm**: Hiển thị đầy đủ thông số kỹ thuật, bình luận của khách hàng.
- **Giỏ hàng (`CartController`)**:
  - Lưu trữ giỏ hàng tạm thời trong `Session` (danh sách `CartItem`).
  - Thêm sản phẩm vào giỏ, cập nhật số lượng, xóa sản phẩm.
  - Tính tổng tiền động phía client bằng jQuery.
- **Đặt hàng trực tuyến (`OrderController`)**: Thu thập thông tin giao hàng, lưu `DonHang` và danh sách `ChiTietDonHang` vào database, xóa Session giỏ hàng sau khi đặt thành công.
- **Lịch sử mua hàng**: Hiển thị danh sách đơn hàng đã đặt của tài khoản đang đăng nhập, lọc theo `UserId`.

---

## 2. Tài liệu liên quan đã tham khảo

- Tài liệu kỹ thuật **ASP.NET MVC Routing**: Cấu hình `RouteConfig.cs`, Attribute Routing.
- Tài liệu **ActionResult** types: `ViewResult`, `JsonResult`, `RedirectResult`, `PartialViewResult`.
- Tài liệu **Session trong ASP.NET**: Lưu trữ đối tượng phức tạp (`List<CartItem>`) vào `Session` thông qua serialization JSON.
- Tài liệu **LINQ to Entities** (Entity Framework 6): Truy vấn dữ liệu với `Where`, `OrderBy`, `Include`, `Skip`, `Take`.

---

## 3. Khó khăn gặp phải

- **Đồng bộ giỏ hàng bằng AJAX**: Khi người dùng thay đổi số lượng sản phẩm trực tiếp trên giao diện (ô input số lượng), nếu dùng form submit thông thường sẽ tải lại toàn bộ trang gây trải nghiệm kém. Cần sử dụng **AJAX/jQuery** để gửi request cập nhật số lượng lên `CartController` (trả về `JsonResult`) và cập nhật lại tổng tiền trên giao diện mà không reload trang.
  ```javascript
  // Ví dụ cập nhật số lượng bằng AJAX
  $(".qty-input").on("change", function () {
      var productId = $(this).data("id");
      var qty = $(this).val();
      $.post("/Cart/UpdateQuantity", { id: productId, quantity: qty }, function (data) {
          if (data.success) {
              $("#cart-total").text(data.total);
          }
      });
  });
  ```
- **Serialize/Deserialize Session**: Lưu `List<CartItem>` vào Session đòi hỏi tuần tự hóa đối tượng; xử lý trường hợp Session hết hạn (timeout) để không gây lỗi `NullReferenceException`.

---

## 4. Kết quả đạt được

- [x] Dự án khởi tạo thành công, kết nối CSDL ổn định.
- [x] Hoàn thành chức năng **Trang chủ**, **Danh sách sản phẩm** và **Bộ lọc**.
- [x] **Giỏ hàng** hoạt động ổn định: thêm, cập nhật số lượng (qua AJAX), xóa sản phẩm.
- [x] Luồng **Đặt hàng** hoàn chỉnh: từ xem sản phẩm → thêm giỏ → nhập thông tin → lưu đơn hàng vào database.
- [x] **Lịch sử mua hàng** hiển thị đúng theo tài khoản đăng nhập.

---

## 5. Kế hoạch tuần tiếp theo (Tuần 4: 13/07/2026 - 19/07/2026)

- Tích hợp template **SB Admin 2** cho khu vực quản trị.
- Viết các chức năng CRUD cho Admin: Sản phẩm công nghệ Apple, Kênh phân phối, Đơn hàng, Tin tức thuộc Chủ đề.
- Thiết lập hệ thống phân quyền bằng `[Authorize(Roles = "Admin")]`.
