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

    public async Task<List<Models.Attribute>> GetAttributesAsync()
    {
        var attributes = new List<Models.Attribute>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"SELECT attribute_id,
                           name
                    FROM Attributes
                    ORDER BY name";

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            attributes.Add(new Models.Attribute
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return attributes;
    }

    public async Task<List<AttributeValue>> GetAttributeValuesByAttributeIdAsync(int attributeId)
    {
        var attributeValues = new List<AttributeValue>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"SELECT value_id,
                           attribute_id,
                           value
                    FROM AttributeValues
                    WHERE id_attribute = @a_id
                    ORDER BY value";

        using var cmd = new NpgsqlCommand(sql, conn);
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

    public async Task<bool> AddAttributeAsync(Models.Attribute attribute)
    {
        if (attribute == null)
        {
            throw new ArgumentNullException("Attribute must not be null");
        }

        if (string.IsNullOrWhiteSpace(attribute.Name))
        {
            throw new ArgumentException("Attribute name is required");
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"INSERT INTO Attributes (name)
                    VALUES (@n)";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("n", attribute.Name);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> AddAttributeValueAsync(AttributeValue attributeValue)
    {
        if (attributeValue == null)
        {
            throw new ArgumentNullException("AttributeValue must not be null");
        }

        if (string.IsNullOrWhiteSpace(attributeValue.Value))
        {
            throw new ArgumentException("AttributeValue is required");
        }

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"INSERT INTO AttributeValues (value_id, attribute_id, value)
                    VALUES (@v_id, @a_id, @v)";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("v_id", attributeValue.ValueId);
        cmd.Parameters.AddWithValue("c_id", attributeValue.AttributeId);
        cmd.Parameters.AddWithValue("v", attributeValue.Value);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
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
