using Models.RequestModel;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICategoryRepository : IReadRepository<string, int>, ICreateRepository<CategoryRequestModel>, IUpdateRepository<CategoryRequestModel>, IRemoveRepository<int>
    {
    }
}
