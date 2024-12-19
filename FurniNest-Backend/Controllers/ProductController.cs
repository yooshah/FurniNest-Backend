using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
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
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO newProduct ,IFormFile image=null)
        {
            var productAdded=await _productService.AddProduct(newProduct,image);

            return Created($"api/products/{productAdded}",new ApiResponse<AddProductDTO>(201,"Product Added Successfully",newProduct));
        }

        [HttpGet("GetProductById/{id}")]
        public  async Task<IActionResult> GetProductThroughId(int id)
        {

            var getProduct=await _productService.GetProductById(id);

            return Ok(new ApiResponse<ProductDTO>(200, "Successfully accessed Product",getProduct));
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] AddProductDTO updateProduct, IFormFile image = null)
        {
            try
            {
                await _productService.UpdateProduct(id, updateProduct, image);

                return Ok(new ApiResponse<string>(200, "Product Updated Successfully "));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "an unexpected error occured", null, ex.Message));
            }
        }

        [HttpGet("GetAllProducts")]

        public async Task<IActionResult> GetAllProducts()
        {

            var allProduct =await  _productService.GetAllProducts();

            return Ok(new ApiResponse<List<ProductDTO>>(200,"Fetched All product",allProduct));
        }

    }


}
