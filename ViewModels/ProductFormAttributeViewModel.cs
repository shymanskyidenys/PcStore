using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.ViewModels;

public class ProductFormAttributeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public IEnumerable<SelectListItem> AttributeValues { get; set; } = [];
}
