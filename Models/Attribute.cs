namespace PcStoreApp.Models;

public class Attribute
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? AttributeValueCount { get; set; } = null;
}
