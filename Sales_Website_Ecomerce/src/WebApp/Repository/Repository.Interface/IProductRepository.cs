using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository :IReadRepository<ProductResponeModel, int>
    {

    }
}
