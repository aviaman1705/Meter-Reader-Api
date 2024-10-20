using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public interface ITrackRepository : IRepository<Track>
    {
        Dashboard GetDashboardData();
    }
}
