using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public interface ISearchRepository
    {
        List<SearchResult> Search(string term);
    }
}
