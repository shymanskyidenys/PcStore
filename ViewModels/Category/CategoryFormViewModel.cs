namespace PcStoreApp.ViewModels;

public class CategoryFormViewModel
{
    public int? Id { get; set; } = null;
    public string? Name { get; set; }

    public bool IsEdit => Id.HasValue && Id > 0;
    public string Title => IsEdit ? $"Edit category {Name}" : "Add new category";
}
