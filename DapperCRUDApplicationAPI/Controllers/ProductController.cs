using DapperCRUDApplicationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkRepositoryPattern.Core.Models;

namespace DapperCRUDApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRespository;

        public ProductController(IProductRepository productRespository)
        {
            _productRespository = productRespository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productRespository.GetAllProductsAsync());
        }
        [HttpGet("GetById/{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await _productRespository.GetProductByIdAsync(id));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProductAsync(Product product)
        {
            var productAdded = await _productRespository.CreateProductAsync(product);
            return CreatedAtRoute("GetProductById", new { id = productAdded.Id }, productAdded);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProductAsync(Product product)
        {
            await _productRespository.UpdateProductAsync(product);
            return Ok();
        }
        [HttpDelete("Delete/{int}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _productRespository.DeleteProductAsync(id);
            return Ok();
        }

    }
}
