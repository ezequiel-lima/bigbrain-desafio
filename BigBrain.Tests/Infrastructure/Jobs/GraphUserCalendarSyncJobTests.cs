using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Jobs;
using Microsoft.Extensions.Logging;
using Moq;

namespace BigBrain.Tests.Infrastructure.Jobs
{
    public class GraphUserCalendarSyncJobTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldCallSyncServiceExecuteAsync()
        {
            // Arrange
            var mockSyncService = new Mock<ICalendarEventSyncService>();
            var mockLogger = new Mock<ILogger<GraphUserCalendarSyncJob>>();
            var job = new GraphUserCalendarSyncJob(mockSyncService.Object, mockLogger.Object);

            // Act
            await job.ExecuteAsync(null);

            // Assert
            mockSyncService.Verify(s => s.ExecuteAsync(null), Times.Once);
        }
    }
}
