using MeterReaderAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Services
{
    public class NotebookRepository : INotebookRepository
    {
        private readonly ApplicationDbContext _context;

        public NotebookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Notebook item)
        {
            _context.Notebooks.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            bool isDeleted = false;
            Notebook entity = _context.Notebooks.FirstOrDefault(p => p.Id == id);
            if (entity != null)
            {
                _context.Notebooks.Remove(entity);
                _context.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public Notebook Get(int id)
        {
            return _context.Notebooks.Include(x => x.Tracks).FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Notebook> GetAll()
        {
            return _context.Notebooks;
        }

        public bool Update(Notebook entity)
        {
            var notebook = _context.Notebooks.FirstOrDefault(x => x.Id == entity.Id);

            if (notebook != null)
            {
                notebook.Number = entity.Number;

                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
