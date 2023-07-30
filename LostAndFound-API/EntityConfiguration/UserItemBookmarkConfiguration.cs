using LostAndFound_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostAndFound_API.EntityConfiguration
{
    public class UserItemBookmarkConfiguration : IEntityTypeConfiguration<UserItemBookmark>
    {
        public void Configure(EntityTypeBuilder<UserItemBookmark> builder)
        {
            builder.HasKey(uib => new { uib.BookmarkByUserId, uib.ItemBookmarkId });
        }
    }
}
