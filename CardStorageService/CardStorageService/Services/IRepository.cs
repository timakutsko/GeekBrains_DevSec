using System.Collections.Generic;

namespace CardStorageService.Services
{
    public interface IRepository<T, TId>
    {
        IList<T> GetAll();

        IList<T> GetById(TId id);

        TId Create(T item);

        int Update (T item);

        int Delete (T item);
    }
}
