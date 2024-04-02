using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore中实现值对象
{
    public class RegionConfig : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("T_Region");
            builder.OwnsOne(r => r.Location);
            builder.Property(e => e.Level).HasMaxLength(20)
                .IsUnicode(false).HasConversion<string>();
            builder.OwnsOne(r => r.Name, nb =>
            {
                nb.Property(e => e.Chinese).HasMaxLength(20).IsUnicode(false);
                nb.Property(e => e.English).HasMaxLength(20).IsUnicode(false);
            });
            builder.OwnsOne(r => r.Area, nb =>
            {
                nb.Property(e => e.Unit).HasMaxLength(20)
                .IsUnicode(false).HasConversion<string>();
            });
        }
    }
}
