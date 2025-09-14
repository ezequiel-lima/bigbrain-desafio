using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace BigBrain.Infrastructure.Persistence.Mappings
{
    [ExcludeFromCodeCoverage]
    public class CalendarEventMapping : IEntityTypeConfiguration<CalendarEventEntity>
    {
        public void Configure(EntityTypeBuilder<CalendarEventEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.ICalUId)
                .HasMaxLength(255);

            builder.Property(e => e.OriginalStartTimeZone)
                .HasMaxLength(255);

            builder.Property(e => e.OriginalEndTimeZone)
                .HasMaxLength(255);

            builder.Property(e => e.Response)
                .HasMaxLength(255);

            builder.Property(x => x.ResponseTime);
            builder.Property(x => x.ReminderMinutesBeforeStart);
            builder.Property(x => x.IsReminderOn);

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("UserCalendarEvents");
        }
    }
}
