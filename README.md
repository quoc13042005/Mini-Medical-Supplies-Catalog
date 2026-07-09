# Mini Medical Supplies Catalog - Lab06 Final

Đây là dự án quản lý Vật tư Y tế (Mini Medical Supplies) được xây dựng tùy biến theo yêu cầu Câu 2 của bài Lab06 (Final Secure Mini Shop MVC Project - Requirements). Dự án tuân thủ nghiêm ngặt các nguyên tắc bảo mật: Phân quyền (Role/Policy), Identity, CSRF, XSS, LINQ (chống SQLi), Audit Logs chuyên sâu, và xử lý nghiệp vụ phức tạp (AdjustStock - điều chỉnh số lượng an toàn).

## Yêu cầu hệ thống
- .NET 10.0 SDK (hoặc phiên bản tương thích trong file `.csproj`)
- Công cụ dòng lệnh `dotnet` CLI

## Cấu trúc thư mục
- `MedicalSupplies.Mvc`: Chứa source code chính của ứng dụng ASP.NET Core MVC.
- `MedicalSuppliesLab05.db`: Database SQLite của hệ thống vật tư y tế.
- `wwwroot/uploads/products/`: Thư mục lưu trữ hình ảnh của vật tư một cách an toàn.

## Hướng dẫn cài đặt và chạy dự án

1. Mở Terminal (Command Prompt / PowerShell).
2. Di chuyển vào thư mục dự án:
   ```bash
   cd Mini-Medical-Supplies-Catalog/MedicalSupplies.Mvc
   ```
3. Khôi phục các gói phụ thuộc:
   ```bash
   dotnet restore
   ```
4. Cập nhật Database (nếu chạy lần đầu hoặc thay đổi cấu trúc DB):
   ```bash
   dotnet ef database update
   ```
5. Khởi động Web Server:
   ```bash
   dotnet run
   ```
6. Truy cập vào ứng dụng qua trình duyệt bằng địa chỉ:
   - **HTTP**: `http://localhost:5182`
   - **HTTPS**: `https://localhost:7241`

## Tài khoản Demo (Seeded)

Hệ thống đã chuẩn bị sẵn các tài khoản dưới đây để bạn test tính năng đăng nhập và phân quyền:

| Phân quyền (Role) | Email Đăng nhập | Mật khẩu | Phân quyền chi tiết (Policy) |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin@shop.test` | `Admin@123` | Toàn quyền thao tác: Thêm/Sửa/Xóa mềm/Khôi phục Vật tư y tế. Có quyền xem danh sách Logs truy cập hệ thống. |
| **Staff** | `staff@shop.test` | `Staff@123` | Có quyền xem chi tiết vật tư, và đặc biệt **Có quyền thay đổi/điều chỉnh tồn kho (AdjustStock)**. Bị chặn khi cố gắng Thêm mới hoặc Xóa vật tư. |
| **User** | `user@shop.test` | `User@123` | Quyền truy cập cơ bản. Không có quyền truy cập vào các giao diện quản trị Admin/Staff. |

## Các tính năng nâng cao (Câu 2)
Dự án đã tích hợp đầy đủ các tính năng bổ sung theo yêu cầu nâng cao:
1. **Tách quyền quản lý giá và tồn kho**: Sử dụng Policy `CanAdjustStock`. Chỉ Staff và Admin mới được phép cộng/trừ số lượng vật tư.
2. **Thay ảnh sản phẩm an toàn**: Đổi tên ngẫu nhiên (Guid) và chỉ xóa file ảnh cũ khi cập nhật Database thành công (File Upload Service).
3. **Audit Log Search & Security Dashboard**: Ghi nhận toàn bộ thao tác truy cập trái phép và thay đổi nhạy cảm. Thống kê theo ngày (Today Metrics) hiển thị trực quan tại Dashboard, giúp quản trị viên nắm bắt nhanh tình hình an ninh.
4. **API Search an toàn (ValidationProblemDetails)**: Endpoint `/api/supplies/search?keyword=` sử dụng Minimal APIs để xác thực đầu vào, trả về mã lỗi 400 (Validation Problem) hoặc 404 (Not Found) nếu không có kết quả hợp lệ.
