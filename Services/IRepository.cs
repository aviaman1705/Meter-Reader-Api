using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        T Get(int id);
        void Add(T item);

        bool Update(T entity);

        bool Delete(int id);
    }
}
