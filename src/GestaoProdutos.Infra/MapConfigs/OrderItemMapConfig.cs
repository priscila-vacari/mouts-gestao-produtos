using GestaoProdutos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.Infra.MapConfigs
{
    [ExcludeFromCodeCoverage]
    public class OrderItemMapConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            builder.HasKey(k => new { k.Id, k.ProductId });

            builder.Property(p => p.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.HasOne(p => p.Order)
                   .WithMany(p => p.Itens)
                   .HasForeignKey(p => p.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
