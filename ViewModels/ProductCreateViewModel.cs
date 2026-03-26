using Microsoft.AspNetCore.Mvc.Rendering;

namespace PcStoreApp.ViewModels
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
