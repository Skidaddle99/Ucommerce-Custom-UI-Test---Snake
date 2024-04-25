namespace CustomUITest.Models;

public record OrderLineViewModel
{
    public string? Price { get; init; }
    public string? ProductName { get; init; }
    public int Quantity { get; init; }
    public string? Tax { get; init; }
    public string? Total { get; init; }
}