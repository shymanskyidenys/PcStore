using Npgsql;
using PcStoreApp.Models;

namespace PcStoreApp.Repositories;

public class AttributesRepository
{
    private readonly string _connectionString;

    public AttributesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<Models.Attribute>> GetAttributesListAsync(int limit = 100, int offset = 0)
    {
        var attributes = new List<Models.Attribute>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            SELECT a.attribute_id,
                   a.name,
                   COUNT(av.value_id) AS values_count
            FROM Attributes a
            LEFT JOIN AttributeValues av ON av.attribute_id = a.attribute_id
            GROUP BY a.attribute_id, a.name
            ORDER BY a.attribute_id DESC
            LIMIT @limit
            OFFSET @offset";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);
        cmd.Parameters.AddWithValue("offset", offset);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            attributes.Add(new Models.Attribute
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                AttributeValueCount = reader.GetInt32(2)
            });
        }

        return attributes;
    }

    public async Task<Models.Attribute?> GetAttributeByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var @sql = @"
            SELECT
                attribute_id,
                name
            FROM Attributes
            WHERE attribute_id = @id";
        
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = await cmd.ExecuteReaderAsync();

        Models.Attribute? attribute = null;
        if (await reader.ReadAsync())
        {
            attribute = new Models.Attribute
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
        
        return attribute;
    }

    public async Task<AttributeValue?> GetAttributeValueByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var @sql = @"
            SELECT
                value_id,
                attribute_id,
                value
            FROM AttributeValues
            WHERE value_id = @id";
        
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = await cmd.ExecuteReaderAsync();

        AttributeValue? attributeValue = null;
        if (await reader.ReadAsync())
        {
            attributeValue = new AttributeValue
            {
                ValueId = reader.GetInt32(0),
                AttributeId = reader.GetInt32(1),
                Value = reader.GetString(2)
            };
        }
        
        return attributeValue;
    }

    public async Task<List<AttributeValue>> GetAttributeValuesByAttributeIdAsync(int attributeId)
    {
        if (attributeId <= 0)
        {
            return [];
        }

        var attributeValues = new List<AttributeValue>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            SELECT value_id,
                   attribute_id,
                   value
            FROM AttributeValues
            WHERE attribute_id = @a_id
            ORDER BY value";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("a_id", attributeId);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            attributeValues.Add(new AttributeValue
            {
                ValueId = reader.GetInt32(0),
                AttributeId = reader.GetInt32(1),
                Value = reader.GetString(2)
            });
        }

        return attributeValues;
    }

    public async Task<Dictionary<Models.Attribute, List<AttributeValue>>> GetByCategoryIdAsync(int categoryId)
    {
        if (categoryId <= 0)
        {
            return new Dictionary<Models.Attribute, List<AttributeValue>>();
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
            WHERE ca.category_id = @c_id
            ORDER BY a.name, av.value";

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

    public async Task<bool> SaveAttributeAsync(Models.Attribute attribute)
    {
        if (attribute == null || string.IsNullOrWhiteSpace(attribute.Name))
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        if (attribute.Id <= 0)
        {
            var sql = @"
                INSERT INTO Attributes(name)
                VALUES (@n)";
            
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("n", attribute.Name);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        else
        {
            var sql = @"
                UPDATE Attributes SET
                name = @n
                WHERE attribute_id = @id";
            
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("n", attribute.Name);
            cmd.Parameters.AddWithValue("id", attribute.Id);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }

    public async Task<bool> SaveAttributeValueAsync(AttributeValue attributeValue)
    {
        if (attributeValue == null || string.IsNullOrWhiteSpace(attributeValue.Value))
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        if (attributeValue.ValueId <= 0)
        {
            var sql = @"
                INSERT INTO AttributeValues (attribute_id, value)
                VALUES (@a_id, @v)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("a_id", attributeValue.AttributeId);
            cmd.Parameters.AddWithValue("v", attributeValue.Value);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        else
        {
            var sql = @"
                UPDATE AttributeValues SET
                value = @v,
                attribute_id = @a_id
                WHERE value_id = @v_id";
            
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("v", attributeValue.Value);
            cmd.Parameters.AddWithValue("a_id", attributeValue.AttributeId);
            cmd.Parameters.AddWithValue("v_id", attributeValue.ValueId);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }

    public async Task<bool> DeleteAttributeAsync(int attributeId)
    {
        if (attributeId <= 0)
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"DELETE FROM Attributes WHERE attribute_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", attributeId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAttributeValueAsync(int attributeValueId)
    {
        if (attributeValueId <= 0)
        {
            return false;
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"DELETE FROM AttributeValues WHERE value_id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", attributeValueId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}
