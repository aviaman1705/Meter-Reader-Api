using MeterReaderAPI.Entities;

namespace MeterReaderAPI.DTO
{
    public class DashboardDTO
    {
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public double UnCalledPercentage { get; set; }
        public TrackDTO LowestUnCalledTrack { get; set; }
        public TrackDTO HighestUnCalledTrack { get; set; }
        public PopularNotebookDTO PopularNotebook { get; set; }
        public List<MonthlyDataDTO> CalledsPerMonths { get; set; }
        public List<MonthlyDataDTO> UnCalledsPerMonths { get; set; }
    }
}
