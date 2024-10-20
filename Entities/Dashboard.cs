namespace MeterReaderAPI.Entities
{
    public class Dashboard
    {
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public double UnCalledPercentage { get; set; }
        public Track LowestUnCalledTrack { get; set; }
        public Track HighestUnCalledTrack { get; set; }
        public PopularNotebook PopularNotebook { get; set; }
        public List<MonthlyData> CalledsPerMonths { get; set; }
        public List<MonthlyData> UnCalledsPerMonths { get; set; }
    }
}
