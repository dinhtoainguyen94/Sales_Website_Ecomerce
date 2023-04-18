using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productService)
        {
            _productService = productService;
        }

        [HttpGet("/FindProduct/{id}")]
        public ActionResult<ProductResponeModel> Get([Required] int id)
        {
            return Ok(_productService.Get(id));
        }

        [HttpGet("/GetListProduct")]
        public ActionResult<ProductResponeModel> GetALL([Required] int pageIndex)
        {
            return Ok(_productService.GetAll(pageIndex));
        }

        [HttpPost("/AddProduct")]
        public ActionResult AddProduct([FromBody] ProductRequestModel product)
        {
            return Ok(_productService.Create(product));
        }

        [HttpPut("/UpdateProduct")]
        public ActionResult UpdateProduct([FromBody] ProductRequestModel item, [Required] int productID)
        {
            return Ok(_productService.Update(item, productID));
        }

        [HttpDelete("/DeleteProduct")]
        public ActionResult DeleteProduct([Required] int id)
        {
            return Ok(_productService.Delete(id));
        }
    }
}
