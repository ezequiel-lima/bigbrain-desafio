namespace BigBrain.Core.Configurations
{
    public class CalendarEventSyncSettings
    {
        public int BatchSize { get; set; }
        public int MaxDegreeOfParallelism { get; set; }
        public DateTime StartDate => new(DateTime.UtcNow.Year, 1, 1);
        public DateTime EndDate => new(DateTime.UtcNow.Year, 12, 31);
    }
}
