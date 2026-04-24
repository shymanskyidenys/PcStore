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

    public async Task<IEnumerable<Product>> GetListAsync(int limit = 100, int offset = 0)
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
            ORDER BY p.product_id DESC
            LIMIT @limit
            OFFSET @offset";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);
        cmd.Parameters.AddWithValue("offset", offset);
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

    public async Task<Product?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        Product? product = null;
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var selectProductQuery = @"
            SELECT p.product_id,
                   p.name,
                   p.price,
                   p.description,
                   p.category_id,
                   c.name
            FROM Products p
            LEFT JOIN Categories c ON c.category_id = p.category_id
            WHERE product_id = @p_id";

        using (var cmd = new NpgsqlCommand(selectProductQuery, conn))
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

        var selectAttributesQuery = @"
            SELECT a.attribute_id,
                   av.value_id
            FROM Product_Attributes pa
            LEFT JOIN AttributeValues av ON pa.value_id = av.value_id
            LEFT JOIN Attributes a ON av.attribute_id = a.attribute_id
            WHERE pa.product_id = @p_id";

        using (var cmd = new NpgsqlCommand(selectAttributesQuery, conn))
        {
            cmd.Parameters.AddWithValue("p_id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                product.Attributes.Add(reader.GetInt32(0), reader.GetInt32(1));
            }
        }

        return product;
    }

    public async Task<bool> SaveAsync(Product product)
    {
        if (product == null || string.IsNullOrWhiteSpace(product.Name))
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        using var transaction = await conn.BeginTransactionAsync();

        try
        {
            int productId;

            if (product.ProductId <= 0)
            {
                var sql = @"
                    INSERT INTO Products (name, price, description, category_id)
                    VALUES (@n, @p, @d, @c)
                    RETURNING product_id";

                using var cmd = new NpgsqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("n", product.Name);
                cmd.Parameters.AddWithValue("p", product.Price);
                cmd.Parameters.AddWithValue("d", (object?)product.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("c", product.CategoryId);

                productId = (int)(await cmd.ExecuteScalarAsync())!;
            }
            else
            {
                var sql = @"
                    UPDATE Products 
                    SET name = @n,
                        price = @p,
                        description = @d,
                        category_id = @c
                    WHERE product_id = @id";

                using var cmd = new NpgsqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("n", product.Name);
                cmd.Parameters.AddWithValue("p", product.Price);
                cmd.Parameters.AddWithValue("d", (object?)product.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("c", product.CategoryId);
                cmd.Parameters.AddWithValue("id", product.ProductId);

                await cmd.ExecuteNonQueryAsync();
                productId = product.ProductId;
            }

            using (var cmd = new NpgsqlCommand(
                "DELETE FROM Product_Attributes WHERE product_id = @id", conn, transaction))
            {
                cmd.Parameters.AddWithValue("id", productId);
                await cmd.ExecuteNonQueryAsync();
            }

            foreach (var kv in product.Attributes)
            {
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO Product_Attributes (product_id, value_id) VALUES (@p, @v)",
                    conn, transaction);
                cmd.Parameters.AddWithValue("p", productId);
                cmd.Parameters.AddWithValue("v", kv.Value);
                await cmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int productId)
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
}
