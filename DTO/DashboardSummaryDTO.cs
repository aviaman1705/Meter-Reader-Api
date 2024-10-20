namespace MeterReaderAPI.DTO
{
    public class DashboardSummaryDTO
    {
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public int MonthlyCalled { get; set; }
        public int MonthlyUnCalled { get; set; }
        public float TotalUncalledPercentage { get; set; }
        public float MonthlyUncalledPercentage { get; set; }
    }
}
