namespace CustomUITest.Models;

public record CategoryViewModel(string DisplayName, string Description, Guid Id)
{
    public string DisplayName { get; } = DisplayName;
    public string Description { get; } = Description;
    public long TotalProductsCount { get; init;  }
    public Guid Id { get; } = Id;
    public IList<CategoryViewModel> Categories { get; init; } = new List<CategoryViewModel>();
    public IList<ProductViewModel> Products { get; init; } = new List<ProductViewModel>();
}