using GreenShop.Models;
using BCrypt.Net;

namespace GreenShop
{
    /// <summary>
    /// Script để tạo tài khoản admin đầu tiên
    /// Chạy một lần duy nhất khi khởi tạo dự án
    /// </summary>
    public class CreateAdminAccount
    {
        public static void CreateDefaultAdmin(GreenShopContext context)
        {
            // Kiểm tra xem đã có admin chưa
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
            {
                // Tạo role Admin nếu chưa có
                adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Status = 1
                };
                context.Roles.Add(adminRole);
                context.SaveChanges();
            }

            // Kiểm tra xem đã có tài khoản admin nào chưa
            var existingAdmin = context.Users
                .Where(u => u.RoleId == adminRole.Id)
                .FirstOrDefault();

            if (existingAdmin == null)
            {
                // Tạo tài khoản admin mặc định
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@greenshop.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Mật khẩu: Admin@123
                    RoleId = adminRole.Id,
                    Status = 1,
                    CreateAt = DateTime.Now
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                Console.WriteLine("✅ Đã tạo tài khoản admin thành công!");
                Console.WriteLine("📧 Email: admin@greenshop.com");
                Console.WriteLine("🔑 Password: Admin@123");
                Console.WriteLine("⚠️  Hãy đổi mật khẩu sau khi đăng nhập lần đầu!");
            }
            else
            {
                Console.WriteLine("ℹ️  Tài khoản admin đã tồn tại.");
            }
        }
    }
}
