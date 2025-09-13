using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace BigBrain.Infrastructure.Graph.Auth
{
    public class GraphAuthProvider : IGraphAuthProvider
    {
        private readonly IConfiguration _config;

        public GraphAuthProvider(IConfiguration config)
        {
            _config = config;
        }

        public GraphServiceClient CreateClient()
        {
            var tenantId = _config["AzureAd:TenantId"];
            var clientId = _config["AzureAd:ClientId"];
            var clientSecret = _config["AzureAd:ClientSecret"];

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new InvalidOperationException("Azure AD settings were not found. Please check your secrets or environment variables.");
            }

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            return new GraphServiceClient(credential, scopes);
        }
    }
}
