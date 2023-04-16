using Models.RequestModel;
using Models.ResponseModels;
using UnitOfWork.Interface;

namespace Services
{
    public interface IProductServices
    {
        IEnumerable<ProductResponeModel> GetAll();
        ProductResponeModel Get(int id);
        void Create(ProductRequestModel model);
        void Update(ProductRequestModel model);
        void Delete(int id);
    }
    public class ProductServices : IProductServices
    {
        private IUnitOfWork _unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Create(ProductRequestModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ProductResponeModel Get(int id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Get(id);
                return result;
            }
        }

        public IEnumerable<ProductResponeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ProductRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
