using Repository.Interface;

namespace UnitOfWork.Interface
{
    public interface IUnitOfWorkRepository
    {
        IProductRepository ProductRepository { get; }
    }
}
