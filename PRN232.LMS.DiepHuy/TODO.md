# TODO - Fix yêu cầu 2 (Auth tables)

## Bối cảnh
- Project hiện có table `Users` (entity `User`) đúng theo cột yêu cầu.
- Chưa có `RefreshToken`, `Permission`, `AuditLog` trong schema.

## Tiến độ
- [x] Xác định thiếu ở PRN232.LMS.Repositories
- [ ] Thêm entity/model: RefreshToken, Permission, AuditLog
- [ ] Cập nhật DatabaseContext: DbSet + mapping
- [ ] Tạo migration mới để tạo 3 bảng
- [ ] (Tuỳ yêu cầu) Update API/auth logic nếu cần
- [ ] Build + verify migration

## Checklist triển khai (không đụng code đang chạy bình thường)
1. Thêm entity/model mới trong `PRN232.LMS.Repositories/Models/Entities/`
   - `RefreshToken`
   - `Permission`
   - `AuditLog`
2. Sửa `PRN232.LMS.Repositories/Models/DatabaseContext.cs`
   - `DbSet<>` cho 3 entity
   - Fluent API: khóa chính, độ dài cột, quan hệ (nếu có)
3. Tạo migration mới trong `PRN232.LMS.Repositories/Migrations/`
4. Build project(s) để đảm bảo không lỗi biên dịch.
5. Đảm bảo `Database.Migrate()` ở `PRN232.LMS.API/Program.cs` sẽ chạy migration đúng.

