using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.ViewModels.Attribute;

public class AttributeValueFormViewModel
{
    public int? ValueId { get; set; }
    public int AttributeId { get; set; }
    public string? Value { get; set; }

    public IEnumerable<SelectListItem> Attributes { get; set; } = [];

    public bool IsEdit => ValueId > 0;
    public string Title => IsEdit ? $"Edit attribute value {Value}" : "Add new attribute value";
}
