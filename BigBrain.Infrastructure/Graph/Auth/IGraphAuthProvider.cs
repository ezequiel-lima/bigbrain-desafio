using Microsoft.Graph;

namespace BigBrain.Infrastructure.Graph.Auth
{
    public interface IGraphAuthProvider
    {
        GraphServiceClient CreateClient();
    }
}
