using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Jobs;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Moq;

namespace BigBrain.Tests.Infrastructure.Jobs
{
    public class GraphUserSyncJobTests
    {
        private readonly Mock<IUserSyncService> _mockSyncService;
        private readonly Mock<ILogger<GraphUserSyncJob>> _mockLogger;
        private readonly GraphUserSyncJob _sut;

        public GraphUserSyncJobTests()
        {
            _mockSyncService = new Mock<IUserSyncService>();
            _mockLogger = new Mock<ILogger<GraphUserSyncJob>>();
            _sut = new GraphUserSyncJob(_mockSyncService.Object, _mockLogger.Object);
        }

        private void VerifyLog<T>(Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times? times = null)
        {
            loggerMock.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                times ?? Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogFailure_WhenUserSyncServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("Error during user synchronization");
            _mockSyncService.Setup(s => s.ExecuteAsync(It.IsAny<PerformContext>()))
                .ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _sut.ExecuteAsync(null));
            VerifyLog(_mockLogger, LogLevel.Information, "Initiating Microsoft Graph user synchronization...", Times.Once());
            _mockSyncService.Verify(s => s.ExecuteAsync(It.IsAny<PerformContext>()), Times.Once);
            VerifyLog(_mockLogger, LogLevel.Information, "Microsoft Graph user synchronization finished successfully.", Times.Never());
        }
    }
}
