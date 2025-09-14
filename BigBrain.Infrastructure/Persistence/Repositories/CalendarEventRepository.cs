using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using BigBrain.Infrastructure.Persistence.Context;
using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigBrain.Infrastructure.Persistence.Repositories
{
    public class CalendarEventRepository : ICalendarEventRepository
    {
        private readonly BigBrainDbContext _context;

        public CalendarEventRepository(BigBrainDbContext context)
        {
            _context = context;
        }

        public async Task DeleteCalendarEventsAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [UserCalendarEvents]");
        }

        public async Task CalendarEventsInsertAsync(IList<CalendarEventModel> calendarEvents)
        {
            if (!calendarEvents.Any()) return;

            var entities = calendarEvents.Select(calendarEvent => (CalendarEventEntity)calendarEvent);

            await _context.CalendarEvents.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
