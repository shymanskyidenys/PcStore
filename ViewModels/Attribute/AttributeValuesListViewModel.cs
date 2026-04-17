namespace PcStoreApp.ViewModels.Attribute;

public class AttributeValuesListViewModel
{
    public int AttributeId { get; set; }
    public string AttributeName { get; set; } = "";
    public IEnumerable<AttributeValueListItemViewModel> Values { get; set; } = [];
}
