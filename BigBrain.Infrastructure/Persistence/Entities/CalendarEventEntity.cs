using BigBrain.Core.Models;

namespace BigBrain.Infrastructure.Persistence.Entities
{
    public class CalendarEventEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? ICalUId { get; set; }
        public string? OriginalStartTimeZone { get; set; }
        public string? OriginalEndTimeZone { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int ReminderMinutesBeforeStart { get; set; }
        public bool IsReminderOn { get; set; }
        public Guid UserId { get; set; }

        public static explicit operator CalendarEventEntity(CalendarEventModel calendarEvent)
        {
            return new CalendarEventEntity
            {
                ICalUId = calendarEvent.ICalUId,
                OriginalStartTimeZone = calendarEvent.OriginalStartTimeZone,
                OriginalEndTimeZone = calendarEvent.OriginalEndTimeZone,
                Response = calendarEvent.Response,
                ResponseTime = calendarEvent.ResponseTime,
                ReminderMinutesBeforeStart = calendarEvent.ReminderMinutesBeforeStart,
                IsReminderOn = calendarEvent.IsReminderOn,
                UserId = calendarEvent.UserId
            };
        }
    }
}
