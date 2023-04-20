using Repository.Interface;

namespace UnitOfWork.Interface
{
    public interface IUnitOfWorkRepository
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICartRepository CartRepository { get; }
    }
}
