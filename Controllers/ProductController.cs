using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using CustomUITest.Models;
using Ucommerce.Extensions.InProcess.Abstractions.Catalogs;
using Ucommerce.Extensions.InProcess.Abstractions.Common;
using Ucommerce.Extensions.InProcess.Abstractions.Transactions.Libraries;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.BackOffice.Pipelines.Product.DeleteProduct;
using Ucommerce.Web.Infrastructure.Core.Models;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace CustomUITest.Controllers;

public class ProductController : Controller
{
    private readonly UcommerceDbContext _ucommerceDbContext;
    private readonly ICatalogContext _catalogContext;
    private readonly ICatalogLibrary _catalogLibrary;
    private readonly ITransactionLibrary _transactionLibrary;
    private readonly IPipeline<CreateProductInput, CreateProductOutput> _createProductPipeline;

    public ProductController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary,
        ITransactionLibrary transactionLibrary, UcommerceDbContext ucommerceDbContext, IPipeline<CreateProductInput, CreateProductOutput> createProductPipeline)
    {
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        _catalogLibrary = catalogLibrary ?? throw new ArgumentNullException(nameof(catalogLibrary));
        _transactionLibrary = transactionLibrary ?? throw new ArgumentNullException(nameof(transactionLibrary));
        _ucommerceDbContext = ucommerceDbContext;
        _createProductPipeline = createProductPipeline ?? throw new ArgumentNullException(nameof(createProductPipeline));
    }

    /// <summary>
    /// Deletes a random product
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    [HttpPost]
    public async Task<IActionResult> Index()
    {

        var allProducts = _ucommerceDbContext.Products.Select(product => product.Guid).ToList();
        if (!allProducts.Any()) return Ok();
        var selected = allProducts[Random.Shared.Next(allProducts.Count)];
        var product = _ucommerceDbContext.Products.First(product => product.Guid == selected);
        _ucommerceDbContext.Remove(product);
        await _ucommerceDbContext.SaveChangesAsync();
        return Ok();

    }


}