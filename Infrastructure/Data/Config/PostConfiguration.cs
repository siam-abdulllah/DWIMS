using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.PostTitle).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PostDescription).IsRequired(false).HasMaxLength(1000);            
        }
    }
}