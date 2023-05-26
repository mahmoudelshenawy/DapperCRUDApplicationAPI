using Dapper;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Data;
using UnitOfWorkRepositoryPattern.Core.Models;

namespace DapperCRUDApplicationAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperDbContext _context;

        public CategoryRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var query = "SELECT * FROM Categories AS c LEFT JOIN Products AS p ON c.Id = p.CategoryId";
            using (var connection = _context.CreateConnection())
            {
                var categoryDictionary = new Dictionary<int, Category>();
                var categories = await connection.QueryAsync<Category, Product, Category>(query, (category, product) =>
                {

                    if (!categoryDictionary.TryGetValue(category.Id, out var currentCategory))
                    {
                        currentCategory = category;
                        categoryDictionary.Add(currentCategory.Id, currentCategory);
                    }
                    if (product != null && product.CategoryId == currentCategory.Id)
                        currentCategory.Products.Add(product);

                    return currentCategory;
                });

                return categories.Distinct().ToList();
            }
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var query = "SELECT * FROM Categories WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var category = await connection.QueryFirstOrDefaultAsync<Category>(query , new {Id = id});
                return category;
            }
        }
        public async Task UpdateCategoryAsync(int id,Category category)
        {
            var query = "UPDATE Categories SET Name = @Name, UpdatedAt = @UpdatedAt  WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", category.Name, DbType.String);
            parameters.Add("UpdatedAt", DateTime.Now, DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                
            }
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var query = "DELETE FROM Categories WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                
            }
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var query = @"INSERT INTO Categories(Name,CreatedAt,UpdatedAt) VALUES (@Name,@CreatedAt,@UpdatedAt);
                           SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", category.Name, DbType.String);
            parameters.Add("CreatedAt", DateTime.Now, DbType.DateTime);
            parameters.Add("UpdatedAt", DateTime.Now, DbType.DateTime);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                category.Id = id;
                return category;
            }
        }
    }
}
