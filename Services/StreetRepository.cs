using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public class StreetRepository : IStreetRepository
    {
        private readonly ApplicationDbContext _context;
        public StreetRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Street item)
        {
            _context.Streets.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            bool isDeleted = false;
            Street entity = _context.Streets.FirstOrDefault(p => p.Id == id);
            if (entity != null)
            {
                _context.Streets.Remove(entity);
                _context.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public Street Get(int id)
        {
            return _context.Streets.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Street> GetAll()
        {
            return _context.Streets;
        }

        public bool Update(Street entity)
        {
            Street street = _context.Streets.FirstOrDefault(x => x.Id == entity.Id);

            if (street != null)
            {
                street.Title = entity.Title;

                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
