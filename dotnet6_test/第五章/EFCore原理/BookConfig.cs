
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("T_Book");
        builder.Property(e => e.Title).HasMaxLength(50).IsRequired();
        builder.Property(e => e.AuthorName).HasMaxLength(20).IsRequired();
    }
}