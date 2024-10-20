using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SearchResult> Search(string term)
        {
            List<SearchResult> results = new List<SearchResult>();

            var notebooks = _context.Notebooks
                            .Where(x => x.Number.ToString().Contains(term))
                            .Select(x => new SearchResult()
                            {
                                Title = $"{x.Number}",
                                Link = $"/notebooks/edit/{x.Id}"
                            }).ToList();

            results.AddRange(notebooks);

            var tracks = _context.Tracks
                           .Where(x => x.Called.ToString().Contains(term)
                           || x.UnCalled.ToString().Contains(term)
                           || x.Desc.Contains(term))
                           .Select(x => new SearchResult()
                           {
                               Title = $"{x.Desc}",
                               Link = $"/tracks/edit/{x.Id}"
                           }).ToList();

            results.AddRange(tracks);

            return results;
        }
    }
}
