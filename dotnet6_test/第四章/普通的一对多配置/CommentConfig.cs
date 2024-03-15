using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 普通的一对多配置
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comment");
            builder.Property(r => r.message).HasMaxLength(100);
            builder.HasOne<Article>(r => r.article).WithMany(t => t.comments).IsRequired()
                .HasForeignKey(r=>r.articleid);
        }
    }
}
