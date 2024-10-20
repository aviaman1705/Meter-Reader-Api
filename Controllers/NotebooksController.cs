using AutoMapper;
using MeterReaderAPI.DTO.Notebook;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<NotebooksController> logger;
        public NotebooksController(ApplicationDbContext context, INotebookRepository repository, IMapper mapper, ILogger<NotebooksController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<NotebookDTO>>> Get(int page, int itemsPerPage, string sortColumn, string sortType, string? search)
        {
            try
            {
                logger.LogInformation($"Get notebooks list");
                List<NotebookDTO> Notebooks = new List<NotebookDTO>();
                int totalItems = 0;

                //1 טעינת האייטמים ממסד הנתונים
                var queryable = repository.GetAll();

                //2 מעדכן סה"כ אייטמים בהאדר הבקשה
                await HttpContext.InsertParametersPagintionInHelper(queryable);

                //המרת השאילתה למערך 3  
                Notebooks = mapper.Map<List<NotebookDTO>>(await queryable.ToListAsync());

                //4 בדיקה אם הוזן טקסט בשה חיפוש
                if (!string.IsNullOrEmpty(search))
                {
                    Notebooks = Notebooks.Where(m => m.Number.ToString().Contains(search)).ToList();

                    totalItems = Notebooks.Count;
                    Notebooks = Sort(sortColumn, sortType, Notebooks).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    totalItems = Notebooks.Count;
                    Notebooks = Sort(sortColumn, sortType, Notebooks).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }

                var NotebooksPaged = new SysDataTablePager<NotebookDTO>(Notebooks, totalItems, itemsPerPage, page);
                logger.LogInformation($"Notebooks list retrived");
                return NotebooksPaged;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<NotebookDTO> Get(int id)
        {
            try
            {
                logger.LogInformation($"Get notebook by id {id}");
                var Notebook = repository.Get(id);

                if (Notebook == null)
                {
                    logger.LogInformation($"Notebook not found");
                    return NotFound();
                }

                var dto = mapper.Map<NotebookDTO>(Notebook);
                logger.LogInformation($"Notebook {id} retrived");
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetNotebookByNumber/{number:int}/{id?}")]
        public ActionResult<bool> GetNotebookByNumber(int number, int id = 0)
        {
            try
            {
                Notebook? notebook;

                if (id > 0)
                {
                    logger.LogInformation($"Get notebook by id {id} and number {number}");
                    notebook = repository.GetAll().Where(m => m.Number == number && m.Id != id).FirstOrDefault();
                }
                else
                {
                    logger.LogInformation($"Get notebook by id {id}");
                    notebook = repository.GetAll().Where(m => m.Number == number).FirstOrDefault();
                }

                if (notebook == null)
                {
                    logger.LogInformation($"Notebook {id} not found");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetNotebooks")]
        public ActionResult<List<NotebookDTO>> GetNotebooks()
        {
            try
            {
                var notebooks = repository.GetAll().OrderBy(x => x.Number);

                if (notebooks == null)
                {
                    logger.LogInformation($"Notebooks list not found");
                    return NotFound();
                }

                var model = mapper.Map<List<NotebookDTO>>(notebooks);
                logger.LogInformation($"Notebooks list retreved");
                return model;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromForm] NotebookDTO model)
        {
            try
            {
                logger.LogInformation($"Check if notebook number {model.Number} with id {id} allready esxist");
                if (repository.GetAll().Any(x => x.Number == model.Number && x.Id != model.Id))
                {
                    return BadRequest("מספר פנקס כבר קיים.");
                }

                logger.LogInformation($"Get notebook {id} for update");
                var Notebook = repository.Get(id);

                if (Notebook == null)
                {
                    logger.LogInformation($"Notebook {id} not found");
                    return NotFound();
                }

                Notebook = mapper.Map<Notebook>(model);

                repository.Update(Notebook);
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
                Notebook notebookToDelete = repository.Get(id);
                if (notebookToDelete.Tracks.Count == 0)
                {
                    bool isDeleted = repository.Delete(id);

                    if (isDeleted)
                    {
                        logger.LogInformation($"Notebook {id} deleted");
                        return NoContent();
                    }
                }

                logger.LogInformation($"Cannot delete notebook {id}");
                return BadRequest("לא ניתן למחוק פנקס שמשוייכים אליו מסלולים.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<int>> Post([FromForm] AddNotebookDTO notebookDTO)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var notebook = mapper.Map<Notebook>(notebookDTO);
                repository.Add(notebook);
                logger.LogInformation($"Notebook {notebook.Id}was created");
                return notebook.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private List<NotebookDTO> Sort(string sortColumn, string sortType, List<NotebookDTO> list)
        {
            switch (sortColumn)
            {
                case "number":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Number).ToList();
                    else
                        return list.OrderBy(x => x.Number).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
