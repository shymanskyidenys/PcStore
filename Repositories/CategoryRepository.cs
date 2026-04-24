using Npgsql;
using PcStoreApp.Models;

namespace PcStoreApp.Repositories;

public class CategoryRepository
{
    private readonly string _connectionString;

    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            SELECT
                category_id,
                name
            FROM Categories
            WHERE category_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = await cmd.ExecuteReaderAsync();

        Category? category = null;
        if (await reader.ReadAsync())
        {
            category = new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };            
        }

        return category;
    }

    public async Task<List<Category>> GetListAsync(int limit = 100, int offset = 0)
    {
        var categories = new List<Category>();

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            SELECT
                c.category_id,
                c.name,
                COUNT(p.product_id) AS product_count
            FROM Categories c
            LEFT JOIN Products p ON c.category_id = p.category_id
            GROUP BY c.category_id, c.name
            ORDER BY c.category_id DESC
            LIMIT @limit
            OFFSET @offset";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);
        cmd.Parameters.AddWithValue("offset", offset);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                ProductCount = reader.GetInt32(2)
            });
        }

        return categories;
    }

    public async Task<bool> AddAsync(Category category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category.Name))
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            INSERT INTO Categories (name)
            VALUES (@name)";
        
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("name", category.Name);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        if (category == null || category.Id <= 0 || string.IsNullOrWhiteSpace(category.Name))
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            UPDATE Categories SET
            name = @name
            WHERE category_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("name", category.Name);
        cmd.Parameters.AddWithValue("id", category.Id);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        if (categoryId <= 0)
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            DELETE FROM Categories
            WHERE category_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", categoryId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}
