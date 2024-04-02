using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore中实现充血模型
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_USER");
            builder.Property("passwordHash");
            builder.Property(x => x.Remark).HasField("remark");
            builder.Ignore(x => x.Tag);
        }
    }
}
