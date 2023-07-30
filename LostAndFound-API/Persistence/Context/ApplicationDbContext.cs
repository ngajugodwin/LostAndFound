using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LostAndFound_API.EntityConfiguration;
using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, long,
        IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ItemComment> ItemComments { get; set; }
        public DbSet<UserItemBookmark> UserItemBookmarks { get; set; }
        public DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCommentConfiguration());
            modelBuilder.ApplyConfiguration(new UserItemBookmarkConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationSettingConfiguration());            
        }
    }
}
