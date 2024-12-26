using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO? newProduct,IFormFile? image)
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
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] AddProductDTO? updateProduct,IFormFile? image)
        {
            try
            {
                await _productService.UpdateProduct(id, updateProduct,image);

                return Ok(new ApiResponse<string>(200, "Product Updated Successfully "));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "an unexpected error occured", null, ex.Message));
            }
        }

        [HttpDelete("admin/DeleteProduct")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteProductById(int id)
        {
            try
            {
                if (id <= 0) 
                {
                    return BadRequest(new ApiResponse<string>(400, "Invalid Product Id"));
                }

                var res = await _productService.DeleteProductById(id);

                if (!res)
                {
                    return NotFound(new ApiResponse<string>(404, "Product Not Found"));
                }

                return Ok(new ApiResponse<string>(200, $"Successfully deleted Product with Id-{id}"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error,${ex.Message}"));
            }

        }       

        [HttpGet("GetAllProducts")]

        public async Task<IActionResult> GetAllProducts()
        {

            var allProduct =await  _productService.GetAllProducts();

            return Ok(new ApiResponse<List<ProductDTO>>(200,"Fetched All product",allProduct));
        }



        [HttpGet("ViewProductByCategory")]

        public async Task<IActionResult> ViewProductByCategory(int categoryId)
        {
            try
            {
                var res = await _productService.GetroductByCategory(categoryId);

                if (res.StatusCode == 400)
                {
                    return BadRequest(res);
                }

                if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }
                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }

                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"an unexpected error occured {ex.Message}");
            }

        }

        [HttpGet("ProductPagination")]
        public async Task<IActionResult> GetProductByPagination()
        {
            try
            {
                var res = await _productService.GetProductByPagination();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"an unexpected error occured {ex.Message}");
            }
        }

        [HttpGet("SearchProducts")]

        public async Task<IActionResult> SearchProducts(string searchWord)
        {

            try
            {
                var res = await _productService.SearchProduct(searchWord);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "an unexpected error occured", null, ex.Message));
            }
        }



    }


}
