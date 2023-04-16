using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<ProductResponeModel> Get([Required] int id)
        {
            return Ok(_productService.Get(id));
        }
    }
}
