using System.Collections.Generic;

namespace CardStorageService.Services
{
    public interface ICardStorageService<T, TId>
    {
        IList<T> GetAll();

        T GetById(TId id);

        TId Create(T item);

        int Update (T item);

        int Delete (T item);
    }
}
