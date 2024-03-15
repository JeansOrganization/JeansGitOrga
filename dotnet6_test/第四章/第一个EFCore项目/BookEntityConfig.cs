using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一个EFCore项目
{
    public class BookEntityConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("T_Book");
            builder.Property(r => r.id).HasComment("主键").IsRequired();
            builder.Property(r => r.name).HasMaxLength(50).HasComment("书名").IsRequired();
            builder.Property(r => r.price).HasComment("价格").HasDefaultValue(0);
        }
    }
}
