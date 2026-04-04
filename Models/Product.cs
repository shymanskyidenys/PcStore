namespace PcStoreApp.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";

    public Dictionary<int, int> Attributes { get; set; } = [];

    public List<Product> Components { get; set; } = [];
}
