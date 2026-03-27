using Npgsql;
using PcStoreApp.Models;

namespace PcStoreApp.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"SELECT p.product_id,
                           p.name,
                           p.price,
                           p.description,
                           c.name as CategoryName 
                    FROM Products p 
                    LEFT JOIN Categories c ON p.category_id = c.category_id 
                    ORDER BY p.product_id DESC";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product {
                ProductId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Description = reader.GetString(3),
                CategoryName = reader.GetString(4)
            });
        }

        return products;
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException("Product cannot be null");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required", nameof(product.Name));
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"INSERT INTO Products (name, price, description, category_id) 
                    VALUES (@n, @p, @d, @c)";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("n", product.Name);
        cmd.Parameters.AddWithValue("p", product.Price);
        cmd.Parameters.AddWithValue("d", (object)product.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("c", product.CategoryId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        if (productId <= 0)
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"DELETE FROM Products WHERE product_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", productId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = new List<Category>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"SELECT category_id,
                           name
                    FROM Categories
                    ORDER BY name";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category { 
                CategoryId = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return categories;
    }
}
