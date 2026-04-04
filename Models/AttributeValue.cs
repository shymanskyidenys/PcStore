namespace PcStoreApp.Models;

public class AttributeValue
{
    public int ValueId { get; set; }
    public int AttributeId { get; set; }
    public required string Value { get; set; }
}
