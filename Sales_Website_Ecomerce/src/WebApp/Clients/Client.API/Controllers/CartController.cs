using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartService;

        public CartController(ICartServices cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/FindCart/{CustomerID}")]
        public ActionResult<ProductResponeModel> Get([Required] int CustomerID, [Required] int pageIndex)
        {
            return Ok(_cartService.Get(CustomerID, pageIndex));
        }

        //[HttpGet("/GetListProduct")]
        //public ActionResult<ProductResponeModel> GetALL([Required] int pageIndex)
        //{
        //    return Ok(_productService.GetAll(pageIndex));
        //}

        [HttpPost("/AddCart")]
        public ActionResult AddCart([FromBody] CartRequestModel cart)
        {
            return Ok(_cartService.Create(cart));
        }

        //[HttpPut("/UpdateProduct")]
        //public ActionResult UpdateProduct([FromBody] ProductRequestModel item, [Required] int productID)
        //{
        //    return Ok(_productService.Update(item, productID));
        //}

        //[HttpDelete("/DeleteProduct")]
        //public ActionResult DeleteProduct([Required] int id)
        //{
        //    return Ok(_productService.Delete(id));
        //}
    }
}
