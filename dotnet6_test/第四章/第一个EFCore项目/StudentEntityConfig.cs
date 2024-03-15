using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一个EFCore项目
{
    public class StudentEntityConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("T_Student");
            builder.Property(r => r.id).HasComment("主键").IsRequired();
            builder.Property(r => r.name).HasMaxLength(20).HasComment("姓名").IsRequired();
            builder.Property(r => r.grade).HasMaxLength(2).HasComment("年级");
            builder.Property(r => r.indate).HasComment("入学时间");
        }
    }
}
