using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigBrain.Infrastructure.Persistence.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserPrincipalName)
                .IsRequired()
                .HasMaxLength(255);     

            builder.Property(u => u.DisplayName)
                .HasMaxLength(255);

            builder.Property(u => u.GivenName)
                .HasMaxLength(64);

            builder.Property(u => u.Surname)
                .HasMaxLength(64);

            builder.Property(u => u.Mail)
                .HasMaxLength(255);

            builder.Property(u => u.JobTitle)
                .HasMaxLength(128);

            builder.Property(u => u.MobilePhone)
                .HasMaxLength(64);

            builder.Property(u => u.BusinessPhones)
                .HasMaxLength(255);

            builder.Property(u => u.OfficeLocation)
                .HasMaxLength(255);

            builder.Property(u => u.PreferredLanguage)
                .HasMaxLength(10);

            builder.HasIndex(u => u.UserPrincipalName)
                .IsUnique();

            builder.ToTable("Users");
        }
    }
}
