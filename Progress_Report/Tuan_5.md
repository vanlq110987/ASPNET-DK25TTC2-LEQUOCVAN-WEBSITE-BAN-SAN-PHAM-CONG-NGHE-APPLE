# BÁO CÁO TIẾN ĐỘ THỰC HIỆN ĐỒ ÁN - TUẦN 5

| Thông tin | Chi tiết |
|---|---|
| **Tên đồ án** | Xây dựng Website Bán Sản Phẩm Công Nghệ của Apple |
| **Công nghệ** | ASP.NET MVC 5, SQL Server, Bootstrap |
| **Tuần thực hiện** | Tuần 5 |
| **Thời gian** | 20/07/2026 - 01/08/2026 |
| **Giai đoạn** | Kiểm thử, Đánh giá Ưu/Nhược điểm và Hoàn thiện Báo cáo |

---

## 1. Nội dung công việc đã thực hiện

### 1.1 Triển khai thử nghiệm và Kiểm thử hệ thống
- Triển khai ứng dụng trên môi trường **máy chủ cục bộ (Localhost)** sử dụng IIS Express tích hợp trong Visual Studio.
- Thực hiện **kiểm thử hộp đen (Black-box Testing)** toàn bộ các chức năng theo danh sách test case:

| STT | Chức năng kiểm thử | Kết quả |
|---|---|---|
| 1 | Đăng ký tài khoản mới | Đạt |
| 2 | Đăng nhập / Đăng xuất | Đạt |
| 3 | Xem danh sách & chi tiết sản phẩm | Đạt |
| 4 | Bộ lọc sản phẩm theo kênh phân phối/Nhu cầu | Đạt |
| 5 | Thêm sản phẩm vào giỏ hàng | Đạt |
| 6 | Cập nhật số lượng / Xóa giỏ hàng | Đạt |
| 7 | Đặt hàng và xem lịch sử đơn hàng | Đạt |
| 8 | Admin - CRUD Sản phẩm công nghệ Apple | Đạt |
| 9 | Admin - CRUD Đơn hàng | Đạt |
| 10 | Admin - Phân quyền Role | Đạt |
| 11 | Hiển thị đúng trên mobile (responsive) | Đạt |

### 1.2 Đánh giá ưu điểm và nhược điểm của hệ thống

**Ưu điểm:**
- Hệ thống vận hành **ổn định**, luồng mua hàng từ đầu đến cuối không có lỗi nghiêm trọng.
- Giao diện **thân thiện, trực quan**, sử dụng Bootstrap đảm bảo tính responsive trên nhiều thiết bị.
- Phân quyền rõ ràng giữa Khách hàng và Admin.
- Code có cấu trúc rõ ràng theo mô hình MVC, dễ bảo trì và mở rộng.

**Nhược điểm:**
- **Bảo mật chưa mạnh**: Chưa áp dụng HTTPS, chưa có cơ chế chống CSRF đầy đủ, chưa rate-limit API đặt hàng.
- **Khảo sát thực tế còn hạn chế**: Chưa có khảo sát sâu từ người dùng thực tế (khách hàng, nhân viên cửa hàng) để tinh chỉnh UX/UI.
- **Chưa nâng cấp đồng bộ Realtime toàn diện**: Tính năng SignalR cho thông báo đơn hàng mới chưa hoàn thiện do thiếu thời gian cấu hình.
- Chưa tích hợp cổng thanh toán trực tuyến (VNPay, Momo,...); đơn hàng hiện tại chỉ hỗ trợ thanh toán khi nhận hàng (COD).

### 1.3 Hoàn thiện báo cáo
- Viết phần **Kết luận**: Tóm tắt những gì đã đạt được, đối chiếu với mục tiêu đề ra ban đầu.
- Viết **Hướng phát triển**: Tích hợp cổng thanh toán, nâng cấp Realtime với SignalR, cải thiện bảo mật, mobile app.
- Định dạng toàn bộ báo cáo theo chuẩn văn bản hành chính (**Nghị định 30/2020/NĐ-CP**): font chữ Times New Roman 13pt, giãn dòng 1.5, lề trang chuẩn.
- Làm **slide thuyết trình** đồ án (PowerPoint/Google Slides): giới thiệu đề tài, công nghệ, sơ đồ hệ thống, demo chức năng, kết luận.

---

## 2. Tài liệu liên quan đã tham khảo

- **Nghị định 30/2020/NĐ-CP** của Chính phủ về công tác văn thư - áp dụng chuẩn trình bày văn bản hành chính vào báo cáo tốt nghiệp.
- Hướng dẫn trình bày tiểu luận, đồ án tốt nghiệp của Trường Đại học Trà Vinh (TVU).
- Mẫu báo cáo đồ án ngành Công nghệ Thông tin.

---

## 3. Khó khăn gặp phải

- **Bug hiển thị hình ảnh sản phẩm**: Một số sản phẩm không hiển thị được ảnh do đường dẫn slug URL tạo ra từ tên sản phẩm bị lỗi khi tên có ký tự đặc biệt (dấu `/`, `&`). Cách khắc phục: chuẩn hóa hàm tạo slug để loại bỏ ký tự đặc biệt trước khi lưu vào DB.
  ```csharp
  // Hàm tạo slug an toàn
  public static string ToSlug(string input) {
      // Bỏ dấu tiếng Việt, thay khoảng trắng bằng "-", loại ký tự đặc biệt
      return Regex.Replace(RemoveDiacritics(input).ToLower(), @"[^a-z0-9\-]", "")
                  .Replace(" ", "-");
  }
  ```
- **Định dạng hiển thị tiền tệ (decimal)**: Giá sản phẩm lưu kiểu `decimal` trong SQL Server nhưng khi hiển thị ra View bị sai định dạng (hiện thêm chữ số thập phân thừa). Khắc phục bằng cách dùng format string `{0:N0}` (định dạng số nguyên có dấu phân cách hàng nghìn) trong Razor View.
  ```html
  @string.Format("{0:N0} VNĐ", item.GiaBan)
  ```

---

## 4. Kết quả đạt được

- [x] Hệ thống **kiểm thử đạt 100%** test case đã đề ra.
- [x] Giao diện hiển thị **mượt mà, chính xác** trên đa thiết bị (máy tính, điện thoại, máy tính bảng).
- [x] Đã fix toàn bộ bug: đường dẫn ảnh sản phẩm, định dạng tiền tệ.
- [x] Hoàn thành **báo cáo tổng hợp** đầy đủ 5 chương, đúng định dạng quy định.
- [x] Hoàn thành **slide thuyết trình** đồ án.
- [x] Nộp file báo cáo cho **Giảng viên hướng dẫn** để chấm điểm.

---

## 5. Tổng kết toàn bộ đồ án

| Tuần | Giai đoạn | Trạng thái |
|---|---|---|
| Tuần 1 (22/06 - 28/06) | Nghiên cứu lý thuyết, chuẩn bị môi trường | Hoàn thành |
| Tuần 2 (29/06 - 05/07) | Phân tích thiết kế hệ thống, CSDL | Hoàn thành |
| Tuần 3 (06/07 - 12/07) | Code chức năng phía Khách hàng | Hoàn thành |
| Tuần 4 (13/07 - 19/07) | Code chức năng Admin, cải tiến | Hoàn thành |
| Tuần 5 (20/07 - 01/08) | Kiểm thử, báo cáo, thuyết trình | **Hoàn thành** |
