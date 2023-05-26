using DapperCRUDApplicationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkRepositoryPattern.Core.Models;

namespace DapperCRUDApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryRepository.GetAllCategoriesAsync());
        }
        [HttpGet("GetById/{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            return Ok(await _categoryRepository.GetCategoryByIdAsync(id));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            var categoryAdded = await _categoryRepository.AddCategoryAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = categoryAdded.Id }, categoryAdded);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> AddCategory(int id ,[FromBody] Category category)
        {
             await _categoryRepository.UpdateCategoryAsync(id,category);
            return Ok();
        }
         [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
             await _categoryRepository.DeleteCategoryAsync(id);
            return Ok();
        }


    }
}
