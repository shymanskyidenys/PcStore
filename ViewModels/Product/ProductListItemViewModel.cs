namespace PcStoreApp.ViewModels;

public class ProductListItemViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public string CategoryName { get; set; } = "";
}
