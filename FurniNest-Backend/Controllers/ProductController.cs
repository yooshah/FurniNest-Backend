using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniNest_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        { 
            _productService = productService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO newProduct ,IFormFile image)
        {
            var productAdded=await _productService.AddProduct(newProduct,image);
            
            return Ok($"Product added succefully ,Were ProductId:{productAdded}");
        }

    }


}
