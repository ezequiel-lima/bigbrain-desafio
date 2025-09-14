using BigBrain.Core.Dtos;

namespace BigBrain.Core.Models
{
    public class CalendarEventModel
    {
        public string? ICalUId { get; set; }
        public string? OriginalStartTimeZone { get; set; }
        public string? OriginalEndTimeZone { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int ReminderMinutesBeforeStart { get; set; }
        public bool IsReminderOn { get; set; }
        public Guid UserId { get; set; }

        public static explicit operator CalendarEventModel(GraphCalendarEventDto graphCalendarEvent)
        {
            return new CalendarEventModel
            {
                ICalUId = graphCalendarEvent.ICalUId,
                OriginalStartTimeZone = graphCalendarEvent.OriginalStartTimeZone,
                OriginalEndTimeZone = graphCalendarEvent.OriginalEndTimeZone,
                Response = graphCalendarEvent.Response,
                ResponseTime = graphCalendarEvent.ResponseTime,
                ReminderMinutesBeforeStart = graphCalendarEvent.ReminderMinutesBeforeStart,
                IsReminderOn = graphCalendarEvent.IsReminderOn,
                UserId = graphCalendarEvent.UserId
            };
        }
    }
}
