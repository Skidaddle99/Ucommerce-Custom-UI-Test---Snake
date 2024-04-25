namespace CustomUITest.Models;

public record CartViewModel
{
    public string? DiscountTotal { get; init; }
    public IList<OrderLineViewModel> OrderLines { get; init; } = new List<OrderLineViewModel>();
    public string? OrderTotal { get; init; }
    public string? Subtotal { get; init; }
    public string? TaxTotal { get; init; }
}