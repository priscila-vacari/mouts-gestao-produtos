using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Infra.MapConfigs;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.Infra.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMapConfig());
            modelBuilder.ApplyConfiguration(new OrderItemMapConfig());
        }
    }
}
