using Moq;
using BigBrain.Infrastructure.Graph.Auth;
using BigBrain.Infrastructure.Graph.Users;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace BigBrain.Tests.Infrastructure.Graph.Users
{
    [ExcludeFromCodeCoverage]
    public class GraphUserServiceTests
    {
        private readonly Mock<IGraphAuthProvider> _mockAuthProvider;
        private readonly Mock<IRequestAdapter> _mockRequestAdapter;
        private readonly GraphUserService _sut;

        public GraphUserServiceTests()
        {
            _mockAuthProvider = new Mock<IGraphAuthProvider>();
            _mockRequestAdapter = new Mock<IRequestAdapter>();

            var graphClient = new GraphServiceClient(_mockRequestAdapter.Object);

            _mockAuthProvider.Setup(ap => ap.CreateClient())
                .Returns(graphClient);

            _sut = new GraphUserService(_mockAuthProvider.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldCallGraphClientWithNextLink_WhenNextLinkIsProvided()
        {
            // Arrange
            var nextLink = "https://graph.microsoft.com/v1.0/users?$skiptoken=someToken";
            var usersResponse = new UserCollectionResponse
            {
                Value = new List<User>
                {
                    new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User 2", UserPrincipalName = "user2@example.com" }
                },
                OdataNextLink = null
            };

            _mockRequestAdapter
                .Setup(adapter => adapter.SendAsync(
                    It.Is<RequestInformation>(info => info.URI.ToString().Contains(nextLink)),
                    It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                    It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(usersResponse);

            // Act
            var result = await _sut.GetUsersAsync(nextLink);

            // Assert
            _mockRequestAdapter.Verify(adapter => adapter.SendAsync(
                It.Is<RequestInformation>(info => info.URI.ToString().Contains(nextLink)),
                It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.Single(result.Users);
            Assert.Equal("Test User 2", result.Users[0].DisplayName);
            Assert.Null(result.NextLink);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmptyListAndNullNextLink_WhenGraphReturnsNullResponse()
        {
            // Arrange
            // Configura o SendAsync para retornar nulo para a primeira requisição
            _mockRequestAdapter
                .Setup(adapter => adapter.SendAsync(
                    It.IsAny<RequestInformation>(),
                    It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                    It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserCollectionResponse)null); // Retorna null aqui

            // Act
            var result = await _sut.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Users);
            Assert.Null(result.NextLink);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnCorrectDtoConversion()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var userPrincipalName = "user.principal@example.com";
            var mail = "user.mail@example.com";
            var displayName = "User Display";
            var jobTitle = "Software Engineer";
            var businessPhones = new List<string> { "+123456789" };

            var graphUser = new User
            {
                Id = userId,
                DisplayName = displayName,
                UserPrincipalName = userPrincipalName,
                Mail = mail,
                JobTitle = jobTitle,
                BusinessPhones = businessPhones,
            };

            var usersResponse = new UserCollectionResponse
            {
                Value = new List<User> { graphUser },
                OdataNextLink = "someNextLink"
            };

            _mockRequestAdapter
                .Setup(adapter => adapter.SendAsync(
                    It.IsAny<RequestInformation>(),
                    It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                    It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(usersResponse);

            // Act
            var result = await _sut.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Users);
            var dto = result.Users[0];

            Assert.Equal(Guid.Parse(userId), dto.Id);
            Assert.Equal(displayName, dto.DisplayName);
            Assert.Equal(userPrincipalName, dto.UserPrincipalName);
            Assert.Equal(mail, dto.Mail);
            Assert.Equal(jobTitle, dto.JobTitle);
            Assert.Equal("someNextLink", result.NextLink);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldHandleNullUserValueInResponse()
        {
            // Arrange
            var usersResponse = new UserCollectionResponse
            {
                Value = null,
                OdataNextLink = "someNextLink"
            };

            _mockRequestAdapter
                .Setup(adapter => adapter.SendAsync(
                    It.IsAny<RequestInformation>(),
                    It.IsAny<ParsableFactory<UserCollectionResponse>>(),
                    It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(usersResponse);

            // Act
            var result = await _sut.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Users);
            Assert.Equal("someNextLink", result.NextLink);
        }
    }
}