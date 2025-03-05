using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Ecommerce.Domain;
//using Ecommerce.Infrastructure.Seed;

namespace Ecommerce.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //DatabaseSeeder.Seed(modelBuilder);
            {

            }
        }
    }
}

