namespace PcStoreApp.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? ProductCount { get; set; } = null;
}
