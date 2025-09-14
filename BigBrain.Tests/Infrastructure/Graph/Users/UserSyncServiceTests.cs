using Moq;
using BigBrain.Core.Dtos;
using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using BigBrain.Infrastructure.Graph.Users;
using System.Diagnostics.CodeAnalysis;

namespace BigBrain.Tests.Infrastructure.Graph.Users
{
    [ExcludeFromCodeCoverage]
    public class UserSyncServiceTests
    {
        private readonly Mock<IGraphUserService> _mockGraphUserService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserSyncService _sut;

        public UserSyncServiceTests()
        {
            _mockGraphUserService = new Mock<IGraphUserService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _sut = new UserSyncService(_mockGraphUserService.Object, _mockUserRepository.Object);
        }

        private GraphUserDto CreateGraphUserDto(string id, string displayName, string userPrincipalName, string mail = null)
        {
            return new GraphUserDto
            {
                Id = Guid.Parse(id),
                DisplayName = displayName,
                UserPrincipalName = userPrincipalName,
                Mail = mail ?? $"{userPrincipalName.Split('@')[0]}@example.com",
                BusinessPhones = "[]",
                JobTitle = "Developer"
            };
        }

        [Fact]
        public async Task ExecuteAsync_ShouldClearRepositoryAndSyncAllUsersInOneBatch()
        {
            // Arrange
            var userDto1 = CreateGraphUserDto("19e62aa8-8129-4854-8eda-477d04e76f43", "User A", "user.a@example.com");
            var userDto2 = CreateGraphUserDto("99ad1f7b-df94-4167-be9f-127beb75b179", "User B", "user.b@example.com");

            var batch1 = new GraphUserBatchDto
            {
                Users = new List<GraphUserDto> { userDto1, userDto2 },
                NextLink = null
            };

            _mockGraphUserService.Setup(s => s.GetUsersAsync(null))
                .ReturnsAsync(batch1);

            _mockUserRepository.Setup(r => r.DeleteUsersAsync())
                .Returns(Task.CompletedTask);

            _mockUserRepository.Setup(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null); // Passa null para o PerformContext

            // Assert
            _mockUserRepository.Verify(r => r.DeleteUsersAsync(), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(null), Times.Once);
            _mockUserRepository.Verify(r => r.InsertUsersAsync(
                It.Is<IList<UserModel>>(users => users.Count == 2 && users.Any(u => u.Id == userDto1.Id))),
                Times.Once);

            _mockGraphUserService.Verify(s => s.GetUsersAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldClearRepositoryAndSyncUsersInMultipleBatches()
        {
            // Arrange
            var userDto1 = CreateGraphUserDto("b548cf83-0562-42fb-a3e3-51e013d47049", "User A", "user.a@example.com");
            var userDto2 = CreateGraphUserDto("9c8de99a-38e1-45bf-8714-c29d06c1f643", "User B", "user.b@example.com");
            var userDto3 = CreateGraphUserDto("993a195c-a891-403e-8e04-a59f5f0ec8a7", "User C", "user.c@example.com");

            var nextLinkBatch1 = "http://nextlink.com/page2";
            var batch1 = new GraphUserBatchDto
            {
                Users = new List<GraphUserDto> { userDto1, userDto2 },
                NextLink = nextLinkBatch1
            };

            var batch2 = new GraphUserBatchDto
            {
                Users = new List<GraphUserDto> { userDto3 },
                NextLink = null
            };

            _mockGraphUserService.SetupSequence(s => s.GetUsersAsync(It.IsAny<string>()))
                .ReturnsAsync(batch1)
                .ReturnsAsync(batch2);

            _mockUserRepository.Setup(r => r.DeleteUsersAsync())
                .Returns(Task.CompletedTask);

            _mockUserRepository.Setup(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockUserRepository.Verify(r => r.DeleteUsersAsync(), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(null), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(nextLinkBatch1), Times.Once);

            _mockUserRepository.Verify(r => r.InsertUsersAsync(
                It.Is<IList<UserModel>>(users => users.Count == 2 && users.Any(u => u.Id == userDto1.Id))),
                Times.Once);
            _mockUserRepository.Verify(r => r.InsertUsersAsync(
                It.Is<IList<UserModel>>(users => users.Count == 1 && users.Any(u => u.Id == userDto3.Id))),
                Times.Once);      
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyGraphResponseGracefully()
        {
            // Arrange
            var emptyBatch = new GraphUserBatchDto
            {
                Users = new List<GraphUserDto>(),
                NextLink = null
            };

            _mockGraphUserService.Setup(s => s.GetUsersAsync(null))
                .ReturnsAsync(emptyBatch);

            _mockUserRepository.Setup(r => r.DeleteUsersAsync())
                .Returns(Task.CompletedTask);

            _mockUserRepository.Setup(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockUserRepository.Verify(r => r.DeleteUsersAsync(), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(null), Times.Once);
            _mockUserRepository.Verify(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleNullGraphResponseGracefully()
        {
            // Arrange
            _mockGraphUserService.Setup(s => s.GetUsersAsync(null))
                .ReturnsAsync((GraphUserBatchDto)null);

            _mockUserRepository.Setup(r => r.DeleteUsersAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockUserRepository.Verify(r => r.DeleteUsersAsync(), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(null), Times.Once);
            _mockUserRepository.Verify(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBatchWithNullUsersListGracefully()
        {
            // Arrange
            var batchWithNullUsers = new GraphUserBatchDto
            {
                Users = null,
                NextLink = null
            };

            _mockGraphUserService.Setup(s => s.GetUsersAsync(null))
                .ReturnsAsync(batchWithNullUsers);

            _mockUserRepository.Setup(r => r.DeleteUsersAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ExecuteAsync(null);

            // Assert
            _mockUserRepository.Verify(r => r.DeleteUsersAsync(), Times.Once);
            _mockGraphUserService.Verify(s => s.GetUsersAsync(null), Times.Once);
            _mockUserRepository.Verify(r => r.InsertUsersAsync(It.IsAny<IList<UserModel>>()), Times.Never);
        }
    }
}
