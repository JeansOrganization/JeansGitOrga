using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 一对一关系
{
    public class DeliveryConfig : IEntityTypeConfiguration<Delivery>
    {
        void IEntityTypeConfiguration<Delivery>.Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("T_Delivery");
        }
    }
}
