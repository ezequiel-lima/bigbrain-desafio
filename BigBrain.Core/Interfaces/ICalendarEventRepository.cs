using BigBrain.Core.Models;

namespace BigBrain.Core.Interfaces
{
    public interface ICalendarEventRepository
    {
        Task CalendarEventsInsertAsync(IList<CalendarEventModel> calendarEvents);
        Task DeleteCalendarEventsAsync();
    }
}
