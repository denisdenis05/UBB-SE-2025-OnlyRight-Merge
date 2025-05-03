using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(rating => rating.RatingID);

            builder.Property(rating => rating.RatingID)
                   .ValueGeneratedOnAdd();

            builder.Property(rating => rating.DrinkID)
                   .IsRequired();

            builder.Property(rating => rating.UserID)
                   .IsRequired();

            builder.Property(rating => rating.RatingValue)
                   .HasColumnType("float");

            builder.Property(rating => rating.RatingDate)
                   .HasColumnType("datetime");

            builder.Property(rating => rating.IsActive)
                   .HasColumnType("tinyint")
                   .HasDefaultValue(null);

            builder.HasIndex(rating => new { rating.UserID, rating.DrinkID })
                   .IsUnique();
        }
    }
}
