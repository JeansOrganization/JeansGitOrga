
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("T_Student");
        builder.HasMany(r => r.Teachers).WithMany(r => r.Students).UsingEntity("T_TeacherToStudent");
    }
}