namespace PcStoreApp.ViewModels;

public class AttributeFormViewModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public bool IsEdit => Id > 0;
    public string Title => IsEdit ? $"Edit attribute {Name}" : "Add new attribute";
}
