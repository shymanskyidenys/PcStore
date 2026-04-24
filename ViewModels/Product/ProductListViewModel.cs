namespace PcStoreApp.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<ProductListItemViewModel> Products { get; set; } = [];
    public int TotalCount { get; set; }
}
