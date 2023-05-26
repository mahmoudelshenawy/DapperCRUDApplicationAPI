using Dapper;
using System.Data;
using UnitOfWorkRepositoryPattern.Core.Models;

namespace DapperCRUDApplicationAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperDbContext _context;

        public ProductRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var query = "SELECT * FROM Products AS p LEFT JOIN Categories AS c ON c.Id = p.CategoryId ";
            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, Category, Product>(query,
                    (product, category) =>
                    {
                        if (product.CategoryId == category.Id)
                            product.Category = category;
                        return product;
                    });
                return products.ToList();
            }
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
                return product;
            }
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var query = @"INSERT INTO Products(Name,Price,CategoryId,CreatedAt,UpdatedAt) 
                          VALUES(@Name,@Price,@CategoryId,@CreatedAt,@UpdatedAt);
                          SELECT CAST(SCOPE_IDENTITY() as int);";
            var parameters = new DynamicParameters();
            parameters.Add("Name", product.Name, DbType.String);
            parameters.Add("Price", product.Price, DbType.Decimal);
            parameters.Add("CategoryId", product.CategoryId, DbType.Int32);
            parameters.Add("CreatedAt", DateTime.Now, DbType.DateTime);
            parameters.Add("UpdatedAt", DateTime.Now, DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                product.Id = id;
                return product;
            }
        }
        public async Task UpdateProductAsync(Product product)
        {
            var query = @"UPDATE Products SET 
                        Name=@Name,Price=@Price,CategoryId=@CategoryId,CreatedAt=@CreatedAt,UpdatedAt=@UpdatedAt
                        WHERE Id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("Id", product.Id, DbType.Int32);
            parameters.Add("Name", product.Name, DbType.String);
            parameters.Add("Price", product.Price, DbType.Decimal);
            parameters.Add("CategoryId", product.CategoryId, DbType.Int32);
            parameters.Add("CreatedAt", DateTime.Now, DbType.DateTime);
            parameters.Add("UpdatedAt", DateTime.Now, DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

    }
}
