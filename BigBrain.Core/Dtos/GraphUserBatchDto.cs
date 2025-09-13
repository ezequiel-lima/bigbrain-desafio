namespace BigBrain.Core.Dtos
{
    public class GraphUserBatchDto
    {
        public List<GraphUserDto> Users { get; set; } = new();
        public string? NextLink { get; set; }
    }
}
