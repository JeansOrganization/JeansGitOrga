using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一个EFCore项目
{
    public class TeacherEntityConfig : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("T_Teacher");
            builder.Property(r => r.id).HasComment("主键").IsRequired();
            builder.Property(r => r.name).HasMaxLength(20).HasComment("姓名").IsRequired();
            builder.Property(r => r.salary).HasComment("薪水");
            builder.Property(r => r.indate).HasComment("入职时间").IsRequired();
        }
    }
}
