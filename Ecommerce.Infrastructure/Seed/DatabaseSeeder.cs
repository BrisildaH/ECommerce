//using Ecommerce.Domain;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;
//using System.Text;

//namespace Ecommerce.Infrastructure.Seed
//{
//    public class DatabaseSeeder
//    {
//        public static void Seed(ModelBuilder modelBuilder)
//        {
//            // Seed Roles
//            modelBuilder.Entity<Role>().HasData(
//                new Role { Id = 1, RoleName = "Admin" },
//                new Role { Id = 2, RoleName = "Customer" }
//            );

//            // Seed Users
//                modelBuilder.Entity<User>().HasData(
//                new User { Id = 1, Username = "Admin", RoleId = 1, Password = DatabaseSeeder.HashPassword("Admin123") },
//                new User { Id = 2, Username = "Customer", RoleId = 2, Password = DatabaseSeeder.HashPassword("Customer123") },
//                new User { Id = 3, Username = "Customer1", RoleId = 2, Password = DatabaseSeeder.HashPassword("Customer234") }
//            );

//            // Seed Products
//            modelBuilder.Entity<Product>().HasData(
//                new Product { Id = 1, Name = "Laptop", Description = "Laptop i7 1Tb", Price = 999.99m, Stock = 10, IsPublic = true, IsAvailable = true },
//                new Product { Id = 2, Name = "Phone", Description = "Iphone 15 Pro Max", Price = 499.99m, Stock = 10, IsPublic = true, IsAvailable = true }
//            );

//            // Seed Discounts
//            modelBuilder.Entity<Discount>().HasData(
//                new Discount { Id = 1,Description = "NEWYEAR", Percentage = 10 },
//                new Discount { Id = 2, Description = "Summer 2025", Percentage = 15 }
//            );
//            // Seed Orders
//            modelBuilder.Entity<Order>().HasData(
//                new Order { Id = 1, OrderDate = new DateTime(2025-01-01), TotalAmount = 999.99m,  UserId = 2 },
//                new Order { Id = 2, OrderDate = new DateTime(2025 -01-10), TotalAmount = 999.98m, UserId = 3}
//            );


//        }
//        public static string HashPassword(string password)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                return Convert.ToBase64String(hashBytes);
//            }
//        }
//    }
//}

