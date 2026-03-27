using Microsoft.AspNetCore.Mvc.Rendering;
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

    public async Task<List<Product>> GetAllProductsAsync()
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

    public async Task AddProductAsync(Product product)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"INSERT INTO Products (name, price, description, category_id) 
                    VALUES (@n, @p, @d, @c)";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("n", product.Name);
        cmd.Parameters.AddWithValue("p", product.Price);
        cmd.Parameters.AddWithValue("d", product.Description);
        cmd.Parameters.AddWithValue("c", product.CategoryId);
        
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        if (productId < 0)
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

    public async Task<List<SelectListItem>> GetCategoriesForSelectAsync()
    {
        var list = new List<SelectListItem>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand("SELECT category_id, name FROM Categories ORDER BY name", conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new SelectListItem { 
                Value = reader.GetInt32(0).ToString(), 
                Text = reader.GetString(1) 
            });
        }

        return list;
    }
}
