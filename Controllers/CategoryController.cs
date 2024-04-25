using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using CustomUITest.Models;
using Ucommerce.Extensions.InProcess.Abstractions.Catalogs;
using Ucommerce.Extensions.InProcess.Abstractions.Common;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace CustomUITest.Controllers;

public class CategoryController : Controller
{
    private readonly ICatalogContext _catalogContext;
    private readonly ICatalogLibrary _catalogLibrary;

    public CategoryController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary)
    {
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        _catalogLibrary = catalogLibrary ?? throw new ArgumentNullException(nameof(catalogLibrary));
    }

    /// <summary>
    /// Index of the controller
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    public async Task<IActionResult> Index()
    {
        var catalog = await _catalogContext.CurrentCatalog();

        var rootCategories = await _catalogLibrary.GetRootCategories(catalog!.Id);
        var model = new CategoryNavigationViewModel
        {
            Categories = await MapCategories(rootCategories.Results)
        };

        return View("/Views/CategoryNavigationPage.cshtml", model);
    }

    /// <summary>
    /// Index of the controller
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    [Route("/category/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var categoryId = Guid.Parse(id);
        var category = await _catalogLibrary.GetCategory(categoryId);
        var products = await _catalogLibrary.GetProducts(categoryId);

        var categoryModel =
            new CategoryViewModel(category.DisplayName ?? category.Name, category.Description ?? "", category.Id)
            {
                Products = await MapProducts(products.Results),
                TotalProductsCount = products.TotalCount,
            };
        return View("/Views/CategoryPage.cshtml", categoryModel);
    }

    private async Task<IList<ProductViewModel>> MapProducts(IImmutableList<ProductSearchModel> products)
    {
        var productViewModels = new List<ProductViewModel>();
        var productIds = products.Select(x => x.Id).ToImmutableList();
        var priceGroup = await _catalogContext.CurrentPriceGroup();
        var priceGroupIds = ImmutableList.Create(priceGroup!.Id);
        var allPrices = await _catalogLibrary.CalculatePrices(productIds,
            priceGroupIds);
        foreach (var p in products)
        {
            var productPrice = allPrices.First(x => x.ProductId == p.Id && x.VariantSku == null);
            var viewModel = new ProductViewModel(p.Sku, p.DisplayName, p.Id.ToString(), productPrice)
            {
                IsVariant = p.ProductType == ProductType.Variant,
                IsSellable = p.ProductType is not ProductType.ProductFamily,
                VariantSku = p.VariantSku,
            };
            productViewModels.Add(viewModel);
        }

        return productViewModels;
    }

    /// <summary>
    /// Maps a list of categories to view models.
    /// </summary>
    /// <param name="categories"> List of categories to be mapped</param>
    /// <returns>List of category view models</returns>
    private async Task<IList<CategoryViewModel>> MapCategories(IImmutableList<CategorySearchModel> categories)
    {
        var result = new List<CategoryViewModel>();
        foreach (var category in categories)
        {
            var catsResultSet = await _catalogLibrary.GetCategories(category.CategoryIds.ToImmutableList());
            var cats = catsResultSet.Results;
            var viewModel = new CategoryViewModel(category.DisplayName ?? category.Name, category.Description ?? "", category.Id)
            {
                Categories = await MapCategories(cats)
            };
            result.Add(viewModel);
        }

        return result;
    }
}