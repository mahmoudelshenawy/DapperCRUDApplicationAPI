using UnitOfWorkRepositoryPattern.Core.Models;

namespace DapperCRUDApplicationAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
    }
}