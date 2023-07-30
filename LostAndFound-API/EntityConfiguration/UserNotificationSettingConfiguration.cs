using LostAndFound_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LostAndFound_API.EntityConfiguration
{
    public class UserNotificationSettingConfiguration : IEntityTypeConfiguration<UserNotificationSetting>
    {
        public void Configure(EntityTypeBuilder<UserNotificationSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(e => e.User)
                .WithOne(e => e.UserNotificationSetting)
                .HasForeignKey<UserNotificationSetting>(x => x.UserId)
                .IsRequired();
               
        }
    }
}
