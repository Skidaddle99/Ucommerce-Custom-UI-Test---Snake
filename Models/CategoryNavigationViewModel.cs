namespace CustomUITest.Models;

public record CategoryNavigationViewModel
{
    public IList<CategoryViewModel> Categories { get; init; } = new List<CategoryViewModel>();
}