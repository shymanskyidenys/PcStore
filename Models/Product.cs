namespace PcStoreApp.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";

    public List<ProductAttributeInfo> Attributes { get; set; } = [];

    public List<Product> Components { get; set; } = [];
}

public class ProductAttributeInfo
{
    public string AttributeName { get; set; } = String.Empty;
    public string AttributeValue { get; set; } = String.Empty;
}
