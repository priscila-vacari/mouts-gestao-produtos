using GestaoProdutos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.Infra.MapConfigs
{
    [ExcludeFromCodeCoverage]
    public class OrderMapConfig: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.OrderId)
                .HasColumnName("orderId")
                .IsRequired();

            builder.Property(p => p.ClientId)
                .HasColumnName("clientId")
                .IsRequired();

            builder.Property(p => p.Tax)
                .HasColumnName("tax");

            builder.Property(p => p.Status)
                .HasColumnName("status")
                .IsRequired();
        }
    }
}
