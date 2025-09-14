using BigBrain.Core.Configurations;
using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using BigBrain.Infrastructure.Graph.Calendars;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace BigBrain.Tests.Infrastructure.Graph.Calendars
{
    [ExcludeFromCodeCoverage]
    public class CalendarEventSyncServiceTests
    {
        private readonly Mock<IGraphCalendarEventService> _mockGraphCalendarService;
        private readonly Mock<ICalendarEventRepository> _mockEventRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<CalendarEventSyncService>> _mockLogger;
        private readonly IOptions<CalendarEventSyncSettings> _settings;
        private readonly CalendarEventSyncService _sut; 

        public CalendarEventSyncServiceTests()
        {
            _mockGraphCalendarService = new Mock<IGraphCalendarEventService>();
            _mockEventRepository = new Mock<ICalendarEventRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<CalendarEventSyncService>>();

            _settings = Options.Create(new CalendarEventSyncSettings
            {
                BatchSize = 2,
                MaxDegreeOfParallelism = 2
            });

            _sut = new CalendarEventSyncService(
                _mockGraphCalendarService.Object,
                _mockEventRepository.Object,
                _mockUserRepository.Object,
                _mockLogger.Object,
                _settings);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldDeleteExistingEvents_BeforeSyncing()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>());

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockEventRepository.Verify(repo => repo.DeleteCalendarEventsAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessAllUsersInBatches()
        {
            // Arrange
            var users = new List<(Guid Id, string UserPrincipalName)>
        {
            (Guid.NewGuid(), "user1@example.com"),
            (Guid.NewGuid(), "user2@example.com"),
            (Guid.NewGuid(), "user3@example.com")
        };

            var userBatch1 = new List<(Guid Id, string UserPrincipalName)> { users[0], users[1] };
            var userBatch2 = new List<(Guid Id, string UserPrincipalName)> { users[2] };

            _mockUserRepository.SetupSequence(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(userBatch1) 
                .ReturnsAsync(userBatch2)
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>()); 

            _mockGraphCalendarService.Setup(service => service.GetUserCalendarEventsAsync(
                    It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<BigBrain.Core.Dtos.GraphCalendarEventDto>());

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockUserRepository.Verify(repo => repo.GetUserBatchAsync(0, _settings.Value.BatchSize), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetUserBatchAsync(_settings.Value.BatchSize, _settings.Value.BatchSize), Times.Once);
            _mockGraphCalendarService.Verify(service => service.GetUserCalendarEventsAsync(users[0].UserPrincipalName, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _mockGraphCalendarService.Verify(service => service.GetUserCalendarEventsAsync(users[1].UserPrincipalName, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _mockGraphCalendarService.Verify(service => service.GetUserCalendarEventsAsync(users[2].UserPrincipalName, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _mockEventRepository.Verify(repo => repo.CalendarEventsInsertAsync(It.IsAny<List<CalendarEventModel>>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldSaveCalendarEvents_ForUsersWithEvents()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userPrincipalName1 = "user1@example.com";
            var userBatch = new List<(Guid Id, string UserPrincipalName)> { (userId1, userPrincipalName1) };

            var graphEvents = new List<BigBrain.Core.Dtos.GraphCalendarEventDto>
            {
                new BigBrain.Core.Dtos.GraphCalendarEventDto { ICalUId = "event1", UserId = userId1 },
                new BigBrain.Core.Dtos.GraphCalendarEventDto { ICalUId = "event2", UserId = userId1 }
            };

            _mockUserRepository.SetupSequence(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(userBatch)
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>());

            _mockGraphCalendarService.Setup(service => service.GetUserCalendarEventsAsync(
                    userPrincipalName1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(graphEvents);


            IList<CalendarEventModel> capturedEvents = null; 
            _mockEventRepository.Setup(repo => repo.CalendarEventsInsertAsync(It.IsAny<IList<CalendarEventModel>>()))
                .Callback<IList<CalendarEventModel>>(events => capturedEvents = events) 
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockGraphCalendarService.Verify(service => service.GetUserCalendarEventsAsync(
                userPrincipalName1, _settings.Value.StartDate, _settings.Value.EndDate), Times.Once);

            Assert.NotNull(capturedEvents);
            Assert.Equal(2, capturedEvents.Count);
            Assert.Contains(capturedEvents, e => e.ICalUId == "event1" && e.UserId == userId1);
            Assert.Contains(capturedEvents, e => e.ICalUId == "event2" && e.UserId == userId1);
            _mockEventRepository.Verify(repo => repo.CalendarEventsInsertAsync(It.IsAny<IList<CalendarEventModel>>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogWarning_WhenGraphServiceThrowsExceptionForAUser()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userPrincipalName1 = "user1@example.com";
            var userId2 = Guid.NewGuid();
            var userPrincipalName2 = "user2@example.com";

            var userBatch = new List<(Guid Id, string UserPrincipalName)> { (userId1, userPrincipalName1), (userId2, userPrincipalName2) };

            _mockUserRepository.SetupSequence(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(userBatch)
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>());

            _mockGraphCalendarService.Setup(service => service.GetUserCalendarEventsAsync(
                    userPrincipalName1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Graph API error for user1"));

            _mockGraphCalendarService.Setup(service => service.GetUserCalendarEventsAsync(
                    userPrincipalName2, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<BigBrain.Core.Dtos.GraphCalendarEventDto>
                {
                new BigBrain.Core.Dtos.GraphCalendarEventDto { ICalUId = "event3", UserId = userId2 }
                });

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error syncing calendar events for user {userPrincipalName1}")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);

            _mockEventRepository.Verify(repo => repo.CalendarEventsInsertAsync(
                It.Is<List<CalendarEventModel>>(list => list.Count == 1 && list[0].ICalUId == "event3" && list[0].UserId == userId2)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotSaveEvents_WhenNoUsersHaveEvents()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>());

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockEventRepository.Verify(repo => repo.CalendarEventsInsertAsync(It.IsAny<List<CalendarEventModel>>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyCalendarEventsFromGraph()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userPrincipalName1 = "user1@example.com";
            var userBatch = new List<(Guid Id, string UserPrincipalName)> { (userId1, userPrincipalName1) };

            _mockUserRepository.SetupSequence(repo => repo.GetUserBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(userBatch)
                .ReturnsAsync(new List<(Guid Id, string UserPrincipalName)>());

            _mockGraphCalendarService.Setup(service => service.GetUserCalendarEventsAsync(
                    userPrincipalName1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<BigBrain.Core.Dtos.GraphCalendarEventDto>());

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockEventRepository.Verify(repo => repo.CalendarEventsInsertAsync(
                It.Is<List<CalendarEventModel>>(list => list.Count == 0)), Times.Once);
        }     
    }
}
