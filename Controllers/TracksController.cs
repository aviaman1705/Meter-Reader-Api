using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Filters;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Xml.Linq;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<TracksController> logger;
        public TracksController(ApplicationDbContext context, ITrackRepository repository, IMapper mapper, ILogger<TracksController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<TrackGridItem>>> Get(int page, int itemsPerPage, string sortColumn, string sortType, string? search)
        {

            try
            {
                logger.LogInformation($"Get tracks list");

                List<TrackGridItem> tracks = new List<TrackGridItem>();
                int totalItems = 0;

                //1 טעינת האייטמים ממסד הנתונים
                var queryable = repository.GetAll();

                //2 מעדכן סה"כ אייטמים בהאדר הבקשה
                await HttpContext.InsertParametersPagintionInHelper(queryable);

                //המרת השאילתה למערך 3  
                tracks = mapper.Map<List<TrackGridItem>>(await queryable.ToListAsync());

                //4 בדיקה אם הוזן טקסט בשה חיפוש
                if (!string.IsNullOrEmpty(search))
                {
                    tracks = tracks
                          .Where(m => m.Called.ToString().Contains(search)
                               || m.NotebookNumber.ToString().Contains(search)
                               || m.FromDate.ToString("dd/MM/yyyy").Contains(search)
                               || m.ToDate.ToString("dd/MM/yyyy").Contains(search)
                               || m.Desc.Contains(search)
                               || m.UnCalled.ToString().Contains(search)).ToList();

                    totalItems = tracks.Count;
                    tracks = Sort(sortColumn, sortType, tracks).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    totalItems = tracks.Count;
                    tracks = Sort(sortColumn, sortType, tracks).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }

                var tracksPaged = new SysDataTablePager<TrackGridItem>(tracks, totalItems, itemsPerPage, page);
                logger.LogInformation($"Tracks list retrived");
                return tracksPaged;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<TrackEditDTO> Get(int id)
        {
            try
            {
                logger.LogInformation($"Get track by id {id}");
                var track = repository.Get(id);

                if (track == null)
                {
                    logger.LogInformation($"Track not found");
                    return NotFound();
                }

                var dto = mapper.Map<TrackEditDTO>(track);
                logger.LogInformation($"Track {id} retrived");
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetDashboardData")]
        public ActionResult<DashboardDTO> GetDashboardData()
        {
            try
            {
                logger.LogInformation($"Get dashboard data");
                var dashboardData = repository.GetDashboardData();

                if (dashboardData == null)
                {
                    logger.LogInformation($"Dashboard data not found");
                    return NotFound();
                }

                var model = mapper.Map<DashboardDTO>(dashboardData);
                return model;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromForm] TrackEditDTO trackDTO)
        {
            try
            {
                logger.LogInformation($"Get track {id} for update");
                var track = repository.Get(id);

                if (track == null)
                {
                    logger.LogInformation($"Track {id} not found");
                    return NotFound();
                }

                track = mapper.Map<Track>(trackDTO);

                repository.Update(track);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = repository.Delete(id);

                if (isDeleted)
                {
                    logger.LogInformation($"Track {id} deleted");
                    return NoContent();
                }

                logger.LogInformation($"Track {id} not found");
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] TrackCreationDTO trackCreationDTO)
        {
            try
            {
                var track = mapper.Map<Track>(trackCreationDTO);
                repository.Add(track);
                logger.LogInformation($"Track {track.Id}was created");
                return track.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private List<TrackGridItem> Sort(string sortColumn, string sortType, List<TrackGridItem> list)
        {
            switch (sortColumn)
            {
                case "id":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Id).ToList();
                    else
                        return list.OrderBy(x => x.Id).ToList();
                case "date":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.FromDate).ToList();
                    else
                        return list.OrderBy(x => x.FromDate).ToList();
                case "notebookNumber":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.NotebookNumber).ToList();
                    else
                        return list.OrderBy(x => x.NotebookNumber).ToList();
                case "desc":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Desc).ToList();
                    else
                        return list.OrderBy(x => x.Desc).ToList();
                case "called":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Called).ToList();
                    else
                        return list.OrderBy(x => x.Called).ToList();
                case "unCalled":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.UnCalled).ToList();
                    else
                        return list.OrderBy(x => x.UnCalled).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
