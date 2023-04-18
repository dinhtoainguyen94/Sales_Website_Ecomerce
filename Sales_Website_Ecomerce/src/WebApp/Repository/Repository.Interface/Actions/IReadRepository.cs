using System.Collections.Generic;

namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T, Y> where T : class
    {
        IEnumerable<T> GetAll(Y pageIndex);
        List<T> GetAll();
        T Get(Y id);
    }
}
