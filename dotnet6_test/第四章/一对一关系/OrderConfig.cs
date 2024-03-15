
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    void IEntityTypeConfiguration<Order>.Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("T_Order");
        builder.HasOne(r => r.Delivery).WithOne(t => t.Order).HasForeignKey<Delivery>(r=>r.OrderId).IsRequired();
    }
}