using System.Data.SqlTypes;
using System.Runtime.InteropServices;
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

        var sql = @"
            SELECT p.product_id,
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

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product id mast be greater then 0");
        }

        Product? product = null;
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql1 = @"
            SELECT p.product_id,
                   p.name,
                   p.price,
                   p.description,
                   p.category_id,
                   c.name
            FROM Products p
            LEFT JOIN Categories c ON c.category_id = p.category_id
            WHERE product_id = @p_id";

        using (var cmd = new NpgsqlCommand(sql1, conn))
        {
            cmd.Parameters.AddWithValue("p_id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                product = new Product
                {
                    ProductId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    CategoryId = reader.GetInt32(4),
                    CategoryName = reader.GetString(5)
                };
            }
        }

        if (product == null)
        {
            return null;
        }

        var sql2 = @"
            SELECT a.attribute_id,
                   av.value_id
            FROM Product_Attributes pa
            LEFT JOIN AttributeValues av ON pa.value_id = av.value_id
            LEFT JOIN Attributes a ON av.attribute_id = a.attribute_id
            WHERE pa.product_id = @p_id";

        using (var cmd = new NpgsqlCommand(sql2, conn))
        {
            cmd.Parameters.AddWithValue("p_id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                product.Attributes.Add(reader.GetInt32(1), reader.GetInt32(3));
            }
        }

        return product;
    }

    public async Task<Dictionary<Models.Attribute, List<AttributeValue>>> GetAttributesByCategoryIdAsync(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category id mast be greater then 0");
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            SELECT a.attribute_id,
                   a.name,
                   av.value_id,
                   av.value
            FROM Category_Attributes ca
            LEFT JOIN Attributes a ON a.attribute_id = ca.attribute_id
            LEFT JOIN AttributeValues av ON av.attribute_id = a.attribute_id
            WHERE ca.category_id = @c_id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("c_id", categoryId);

        var result = new Dictionary<Models.Attribute, List<AttributeValue>>();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var attrKey = result.Keys.FirstOrDefault(a => a.Id == reader.GetInt32(0));
            if (attrKey == null)
            {
                attrKey = new Models.Attribute
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };

                result.Add(attrKey, new List<AttributeValue>());
            }

            if (!reader.IsDBNull(2))
            {
                result[attrKey].Add(new AttributeValue
                {
                    ValueId = reader.GetInt32(2),
                    AttributeId = attrKey.Id,
                    Value = reader.GetString(3)
                });
            }
        }

        return result;
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
            throw new ArgumentException("Id product mast be greater then 0");
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
