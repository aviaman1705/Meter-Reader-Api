namespace MeterReaderAPI.Entities
{
    public class DashboardSummary
    {
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public int MonthlyCalled { get; set; }
        public int MonthlyUnCalled { get; set; }
        public double TotalUncalledPercentage { get; set; }
        public double MonthlyUncalledPercentage { get; set; }
    }
}
