using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ITrackRepository trackRepository;
        private readonly ISearchRepository searchRepository;
        private readonly IMapper mapper;
        private readonly ILogger<SearchController> logger;

        public SearchController(ITrackRepository trackRepository, ISearchRepository searchRepository, IMapper mapper, ILogger<SearchController> logger)
        {
            this.trackRepository = trackRepository;
            this.searchRepository = searchRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<SysDataTablePager<SearchDTO>>> Get(int page, int itemsPerPage, string term)
        {
            try
            {
                var queryable = trackRepository.GetAll();
                var searchResults = mapper.Map<List<SearchDTO>>(await queryable.Where(x => x.Desc.Contains(term)).ToListAsync());

                int totalItems = searchResults.Count;
                searchResults = searchResults.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                var searchResultPaged = new SysDataTablePager<SearchDTO>(searchResults, totalItems, itemsPerPage, page);
                logger.LogInformation($"Searches list retrived");
                return searchResultPaged;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetSearchResults")]
        public async Task<ActionResult<List<SearchResultDTO>>> GetSearchResults(string term)
        {
            try
            {
                var searchResults = mapper.Map<List<SearchResultDTO>>(searchRepository.Search(term));
                logger.LogInformation($"Searches results retrived");
                return searchResults;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
