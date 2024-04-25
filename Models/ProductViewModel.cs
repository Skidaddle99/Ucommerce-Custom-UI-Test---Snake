using Ucommerce.Web.Infrastructure.Core.Models;

namespace CustomUITest.Models;

public record ProductViewModel(string Sku, string DisplayName, string Id, PriceCalculationItem Price)
{
    public bool IsVariant { get; init; }

    public bool IsSellable { get; init; }

    public string DisplayName { get; } = DisplayName;

    public string Id { get; } = Id;

    public IList<ProductViewModel> Variants { get; init; } = new List<ProductViewModel>();

    public string Sku { get; } = Sku;

    public string? VariantSku { get; init; }

    public PriceCalculationItem Price { get; } = Price;
}