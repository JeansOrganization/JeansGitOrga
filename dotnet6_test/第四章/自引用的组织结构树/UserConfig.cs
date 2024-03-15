using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using 自引用的组织结构树;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("T_User");
        builder.HasOne<User>(r => r.Patent).WithMany(r=>r.Childrens).HasForeignKey(r=>r.ParentId);
    }
}