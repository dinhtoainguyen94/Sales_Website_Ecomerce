using Models.RequestModel;
using UnitOfWork.Interface;
using Common;

namespace Services
{
    public interface ICartServices
    {
        //ResultModel GetAll(int pageIndex);
        ResultModel Get(int id, int pageIndex);
        ResultModel Create(CartRequestModel model);
        //ResultModel Update(ProductRequestModel model, int productID);
        //ResultModel Delete(int id);
    }
    public class CartServices : ICartServices
    {
        private IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(CartRequestModel item)
        {
            try
            {
                //throw new NotImplementedException();
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Create(item);
                    if (result == 0)
                    {
                        context.DeleteChanges();
                        outModel.Message = "Thêm Cart thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        outModel.Message = "Thêm Cart thành công";
                        outModel.StatusCode = "200";
                    }
                }
                return outModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public ResultModel Delete(int id)
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        ResultModel outModel = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            var result = context.Repositories.ProductRepository.Remove(id);
        //            if (result == 0)
        //            {
        //                outModel.Message = "Xóa thất bại";
        //                outModel.StatusCode = "999";
        //            }
        //            else
        //            {
        //                context.SaveChanges();
        //                outModel.Message = "Xóa thành công";
        //                outModel.StatusCode = "200";
        //            }
        //        }
        //        return outModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public ResultModel Get(int customerID, int pageIndex)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Get(customerID, pageIndex);
                    if (result.Count == 0)
                    {
                        outModel.Message = "Tìm giỏ hàng thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Tìm giỏ hàng thành công";
                        outModel.StatusCode = "200";
                        outModel.DATA = result;
                    }
                }
                return outModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public ResultModel GetAll(int pageIndex)
        //{
        //    try
        //    {
        //        ResultModel outModel = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            var result = context.Repositories.ProductRepository.GetAll(pageIndex);
        //            if (result.Count == 0)
        //            {
        //                outModel.Message = "Tìm tất cả sản phấm thất bại";
        //                outModel.StatusCode = "999";
        //            }
        //            else
        //            {
        //                outModel.Message = "Tìm tất cả sản phấm thành công";
        //                outModel.StatusCode = "200";
        //                outModel.DATA = result;
        //            }
        //        }
        //        return outModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public ResultModel Update(ProductRequestModel item, int productID)
        //{
        //    try
        //    {
        //        ResultModel res = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            var result = context.Repositories.ProductRepository.Update(item, productID);
        //            if (result == 0)
        //            {
        //                res.Message = "Sửa thất bại";
        //                res.StatusCode = "999";
        //            }
        //            else
        //            {
        //                context.SaveChanges();
        //                res.Message = "Sửa thành công";
        //                res.StatusCode = "200";
        //            }
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
