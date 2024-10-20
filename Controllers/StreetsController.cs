using AutoMapper;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController : ControllerBase
    {
        private readonly IStreetRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<StreetsController> logger;
        public StreetsController(ApplicationDbContext context, IStreetRepository repository, IMapper mapper, ILogger<StreetsController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;   
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<StreetDTO>>> Get(int page, int itemsPerPage, string sortColumn, string sortType, string? search)
        {
            try
            {
                logger.LogInformation($"Get streets list");
                List<StreetDTO> streets = new List<StreetDTO>();
                int totalItems = 0;

                var queryable = repository.GetAll();
                await HttpContext.InsertParametersPagintionInHelper(queryable);
                streets = mapper.Map<List<StreetDTO>>(await queryable.ToListAsync());

                if (!string.IsNullOrEmpty(search))
                {
                    streets = streets
                          .Where(m => m.Title.ToString().Contains(search)).ToList();

                    totalItems = streets.Count;
                    streets = Sort(sortColumn, sortType, streets).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    totalItems = streets.Count;
                    streets = Sort(sortColumn, sortType, streets).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }

                var streetsPaged = new SysDataTablePager<StreetDTO>(streets, totalItems, itemsPerPage, page);
                logger.LogInformation($"Streets list retrived");
                return streetsPaged;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private List<StreetDTO> Sort(string sortColumn, string sortType, List<StreetDTO> list)
        {
            switch (sortColumn)
            {
                case "title":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Title).ToList();
                    else
                        return list.OrderBy(x => x.Title).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
