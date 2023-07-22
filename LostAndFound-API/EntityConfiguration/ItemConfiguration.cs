using LostAndFound_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostAndFound_API.EntityConfiguration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Color)
                 .IsRequired()
                 .HasMaxLength(50);

            builder.Property(i => i.Description)
                 .IsRequired();

            builder.Property(i => i.LostLocationAddress)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(i => i.RetrievalLocationAddress)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(i => i.ModeOfContact)
                  .HasDefaultValue(ModeOfContact.Email);

            builder.HasOne(x => x.ReportedByUser)
                .WithMany(x => x.ItemReportedByUser)
                .HasForeignKey(e => e.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ClaimedByUser)
               .WithMany(x => x.ItemClaimedByUser)
               .HasForeignKey(e => e.ClaimedByUserId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
