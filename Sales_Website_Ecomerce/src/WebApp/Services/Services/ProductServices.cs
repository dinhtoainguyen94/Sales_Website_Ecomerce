using Models.RequestModel;
using Models.ResponseModels;
using UnitOfWork.Interface;

namespace Services
{
    public interface IProductServices
    {
        IEnumerable<ProductResponeModel> GetAll();
        ProductResponeModel Get(int id);
        string Create(ProductRequestModel model);
        string Update(ProductRequestModel model);
        string Delete(int id);
    }
    public class ProductServices : IProductServices
    {
        private IUnitOfWork _unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string Create(ProductRequestModel item)
        {
            //throw new NotImplementedException();
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Create(item);
                return result;
            }
        }

        public string Delete(int id)
        {
            //throw new NotImplementedException();
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Remove(id);
                return result;
            }
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
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.GetAll();
                return result;
            }
        }

        public string Update(ProductRequestModel item)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Update(item);
                return result;
            }
        }
    }
}
