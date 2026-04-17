using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.ViewModels;

public class ProductFormViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = [];
    public IEnumerable<ProductFormAttributeViewModel> Attributes { get; set; } = [];
    public Dictionary<int, int> SelectedAtributes { get; set; } = [];

    public bool IsEdit => ProductId > 0;
    public string Title => IsEdit ? $"Edit product {Name}" : "Add new product";
}
