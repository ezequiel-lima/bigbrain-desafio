using BigBrain.Infrastructure.Graph.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Moq;

namespace BigBrain.Tests.Infrastructure.Graph.Auth
{
    public class GraphAuthProviderTests
    {
        [Fact]
        public void CreateClient_ShouldReturnGraphServiceClient_WhenConfigurationIsValid()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AzureAd:TenantId"]).Returns("fake-tenant-id");
            mockConfig.Setup(c => c["AzureAd:ClientId"]).Returns("fake-client-id");
            mockConfig.Setup(c => c["AzureAd:ClientSecret"]).Returns("fake-client-secret");

            var provider = new GraphAuthProvider(mockConfig.Object);

            // Act
            var client = provider.CreateClient();

            // Assert
            Assert.NotNull(client);
            Assert.IsType<GraphServiceClient>(client);
        }

        [Theory]
        [InlineData(null, "client-id", "client-secret")]
        [InlineData("tenant-id", null, "client-secret")]
        [InlineData("tenant-id", "client-id", null)]
        public void CreateClient_ShouldThrowInvalidOperationException_WhenConfigIsMissing(
            string? tenantId, string? clientId, string? clientSecret)
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AzureAd:TenantId"]).Returns(tenantId);
            mockConfig.Setup(c => c["AzureAd:ClientId"]).Returns(clientId);
            mockConfig.Setup(c => c["AzureAd:ClientSecret"]).Returns(clientSecret);

            var provider = new GraphAuthProvider(mockConfig.Object);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => provider.CreateClient());
            Assert.Equal("Azure AD settings were not found. Please check your secrets or environment variables.", ex.Message);
        }
    }
}
