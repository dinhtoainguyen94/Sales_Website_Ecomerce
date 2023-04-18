using Models.RequestModel;
using UnitOfWork.Interface;

namespace Services
{
    public interface ICategoryServices
    {
        List<string> GetAll();
        string Get(int id);
        string Create(CategoryRequestModel model);
        string Update(CategoryRequestModel model, int CategoryID);
        string Delete(int id);
    }
    public class CategoryServices : ICategoryServices
    {
        private IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string Create(CategoryRequestModel item)
        {
            //throw new NotImplementedException();
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Create(item);
                return result;
            }
        }

        public string Delete(int id)
        {
            //throw new NotImplementedException();
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Remove(id);
                return result;
            }
        }

        public string Get(int id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Get(id);
                return result;
            }
        }

        public List<string> GetAll()
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.GetAll();
                return result;
            }
        }

        public string Update(CategoryRequestModel item, int CategoryID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Update(item, CategoryID);
                return result;
            }
        }
    }
}
