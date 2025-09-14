using BigBrain.Infrastructure.Graph.Auth;
using BigBrain.Infrastructure.Graph.Calendars;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;

namespace BigBrain.Tests.Infrastructure.Graph.Calendars
{
    public class GraphCalendarEventServiceTests
    {
        [Fact]
        public async Task GetUserCalendarEventsAsync_ReturnsMappedEvents_WhenGraphReturnsValidEvents()
        {
            // Arrange
            var eventResponse = new EventCollectionResponse
            {
                Value = new List<Event>
                {
                    new Event
                    {
                        ICalUId = "event-123",
                        OriginalStartTimeZone = "UTC",
                        OriginalEndTimeZone = "UTC",
                        ReminderMinutesBeforeStart = 30,
                        IsReminderOn = true,
                        ResponseStatus = new ResponseStatus
                        {
                            Response = ResponseType.Accepted,
                            Time = DateTimeOffset.UtcNow
                        }
                    }
                }
            };

            var requestAdapterMock = new Mock<IRequestAdapter>();

            requestAdapterMock
                .Setup(adapter => adapter.SendAsync<EventCollectionResponse>(
                    It.IsAny<RequestInformation>(),
                    It.IsAny<ParsableFactory<EventCollectionResponse>>(),
                    null,
                    default))
                .ReturnsAsync(eventResponse);

            var graphClient = new GraphServiceClient(requestAdapterMock.Object);

            var authProviderMock = new Mock<IGraphAuthProvider>();
            authProviderMock.Setup(p => p.CreateClient()).Returns(graphClient);

            var service = new GraphCalendarEventService(authProviderMock.Object);

            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(1);

            // Act
            var result = await service.GetUserCalendarEventsAsync("usuario@dominio.com", startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
