using Microsoft.Graph.Models;

namespace BigBrain.Core.Dtos
{
    public class GraphCalendarEventDto
    {
        public Guid UserId { get; set; }
        public string? ICalUId { get; set; }
        public string? OriginalStartTimeZone { get; set; }
        public string? OriginalEndTimeZone { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int ReminderMinutesBeforeStart { get; set; }
        public bool IsReminderOn { get; set; }

        public static explicit operator GraphCalendarEventDto(Event eventModel)
        {
            return new GraphCalendarEventDto
            {
                ICalUId = eventModel.ICalUId,
                OriginalStartTimeZone = eventModel.OriginalStartTimeZone,
                OriginalEndTimeZone = eventModel.OriginalEndTimeZone,
                Response = eventModel.ResponseStatus?.Response?.ToString(),
                ResponseTime = eventModel.ResponseStatus?.Time?.UtcDateTime,
                ReminderMinutesBeforeStart = eventModel.ReminderMinutesBeforeStart ?? 0,
                IsReminderOn = eventModel.IsReminderOn ?? false
            };
        }
    }
}
