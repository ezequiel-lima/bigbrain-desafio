using BigBrain.Core.Dtos;

namespace BigBrain.Core.Interfaces
{
    public interface IGraphCalendarEventService
    {
        Task<List<GraphCalendarEventDto>> GetUserCalendarEventsAsync(string userPrincipalName, DateTime startDate, DateTime endDate);
    }
}
