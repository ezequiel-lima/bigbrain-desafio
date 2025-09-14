using BigBrain.Core.Dtos;
using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Graph.Auth;
using Microsoft.Graph;

namespace BigBrain.Infrastructure.Graph.Calendars
{
    public class GraphCalendarEventService : IGraphCalendarEventService
    {
        private readonly GraphServiceClient _graphClient;

        public GraphCalendarEventService(IGraphAuthProvider authProvider)
        {
            _graphClient = authProvider.CreateClient();
        }

        public async Task<List<GraphCalendarEventDto>> GetUserCalendarEventsAsync(string userPrincipalName, DateTime startDate, DateTime endDate)
        {
            var response = await _graphClient.Users[userPrincipalName].CalendarView.GetAsync(request =>
            {
                request.QueryParameters.StartDateTime = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                request.QueryParameters.EndDateTime = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                request.QueryParameters.Top = 999;
                request.QueryParameters.Select = new[]
                {
                    "originalStartTimeZone", "originalEndTimeZone", "responseStatus", "iCalUId",
                    "reminderMinutesBeforeStart", "isReminderOn"
                };
            });

            var events = response?.Value?
                .Select(e => (GraphCalendarEventDto)e)
                .ToList() ?? new();

            return events;
        }
    }
}
