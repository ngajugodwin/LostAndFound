using LostAndFound_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostAndFound_API.EntityConfiguration
{
    public class ItemCommentConfiguration : IEntityTypeConfiguration<ItemComment>
    {
        public void Configure(EntityTypeBuilder<ItemComment> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(x => x.Comment)                
                .IsRequired();

            builder.HasOne(x => x.CommentByUser)
              .WithMany(x => x.ItemCommentByUser)
              .HasForeignKey(e => e.CommentByUserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
             .WithMany(x => x.ItemComments)
             .HasForeignKey(e => e.ItemId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
