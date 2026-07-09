# 📀 soft/ — Phần mềm cần cài đặt để chạy AppleShop

Thư mục này chứa bộ cài các phần mềm bắt buộc để build và chạy website **AppleShop** (ASP.NET MVC 5, .NET Framework 4.8, SQL Server).

> Các file trong thư mục là **bootstrapper/web installer chính thức của Microsoft** (dung lượng nhỏ, khi chạy sẽ tự tải phần còn lại — cần Internet). Muốn có bộ cài offline đầy đủ, chạy script `download-offline-installers.ps1`.

---

## 🧰 Danh sách phần mềm & thứ tự cài đặt

| Thứ tự | File / Phần mềm | Mục đích | Ghi chú |
|---|---|---|---|
| 1 | `vs_community_2022_setup.exe` | **Visual Studio 2022 Community** — IDE để mở `scr/AppleShop/AppleShop.sln`, build và chạy web (IIS Express) | Khi cài **bắt buộc tick workload** ✅ *ASP.NET and web development* (đã bao gồm .NET Framework 4.8 targeting pack và IIS Express) |
| 2 | `SQL2022-Express_setup.exe` | **SQL Server 2022 Express** — máy chủ CSDL chạy `AppleShopDB` | Chọn kiểu cài **Basic** là đủ. Instance mặc định sẽ là `.\SQLEXPRESS` |
| 3 | SSMS (tải bằng script bên dưới) | **SQL Server Management Studio** — mở và chạy `scr/AppleShopDB_Script.sql` | File full ~700MB nên không kèm sẵn; chạy `download-offline-installers.ps1` hoặc tải tại https://aka.ms/ssmsfullsetup |
| — | .NET Framework 4.8 | Runtime chạy ứng dụng | **Windows 10 (1903+) / 11 đã có sẵn**, không cần cài riêng. Targeting pack do Visual Studio cài ở bước 1 |

---

## 🚀 Các bước sau khi cài xong

1. Mở **SSMS** → kết nối `.\SQLEXPRESS` → mở file `scr/AppleShopDB_Script.sql` → **Execute** (tạo database `AppleShopDB` + 21 bảng + dữ liệu mẫu).
2. Mở `scr/AppleShop/AppleShop.sln` bằng **Visual Studio 2022**.
3. Sửa connection string trong `scr/AppleShop/Web.config`:
   ```xml
   <add name="AppleShopContext"
        connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=AppleShopDB;Integrated Security=True;MultipleActiveResultSets=True"
        providerName="System.Data.SqlClient" />
   ```
4. Chuột phải Solution → **Restore NuGet Packages** (VS tự làm khi Build).
5. Nhấn **F5** — trình duyệt mở web tại `http://localhost:<port>/`.
6. Đăng nhập Admin: `admin@appleshop.vn / Admin@123456` (ứng dụng tự tạo ở lần chạy đầu).

> ⚠️ Giao diện dùng CDN (Bootstrap, SB Admin 2, CKEditor, Chart.js) — máy demo cần Internet.

---

## 🔗 Link tải chính thức (khi cần tải lại)

| Phần mềm | Link |
|---|---|
| Visual Studio 2022 Community | https://visualstudio.microsoft.com/vs/community/ |
| SQL Server 2022 Express | https://www.microsoft.com/en-us/sql-server/sql-server-downloads |
| SQL Server Management Studio (SSMS) | https://aka.ms/ssmsfullsetup |
| .NET Framework 4.8 Developer Pack | https://dotnet.microsoft.com/download/dotnet-framework/net48 |
| Git for Windows (tùy chọn) | https://git-scm.com/download/win |
