using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using CustomUITest.Models;
using Ucommerce.Extensions.InProcess.Abstractions.Common;
using Ucommerce.Extensions.InProcess.Abstractions.Transactions.Libraries;

namespace CustomUITest.Controllers;

public class CartController : Controller
{
    private readonly ICatalogContext _catalogContext;
    private readonly ITransactionLibrary _transactionLibrary;

    public CartController(ITransactionLibrary transactionLibrary, ICatalogContext catalogContext)
    {
        _transactionLibrary = transactionLibrary ?? throw new ArgumentNullException(nameof(transactionLibrary));
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
    }

    /// <summary>
    /// Index of the controller
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token)
    {
        var model = await MapCart(token);

        return View("/Views/CartPage.cshtml", model);
    }

    /// <summary>
    /// Clears the current Cart and returns to the frontpage.
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    [HttpPost("/cart")]
    public async Task<IActionResult> ClearCart(CancellationToken token)
    {
        await _transactionLibrary.ClearCart(token);

        return Redirect("/");
    }

    private async Task<CartViewModel> MapCart(CancellationToken token)
    {
        var cart = await _transactionLibrary.GetCart(create: true, token: token);
        var priceGroup = await _catalogContext.CurrentPriceGroup(token);
        var culture = new CultureInfo(priceGroup!.CurrencyIsoCode);

        var orderLineModels = cart.OrderLines.Select(x => new OrderLineViewModel
                {
                    Price = x.Price.ToString(culture),
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Tax = x.Tax.ToString(culture),
                    Total = x.Total?.ToString(culture) ?? "",
                })
                .ToList();

        var model = new CartViewModel
        {
            OrderLines = orderLineModels,
            OrderTotal = cart.OrderTotal.ToString() ?? "0",
            DiscountTotal = cart.DiscountTotal.ToString() ?? "0",
            TaxTotal = cart.TaxTotal.ToString() ?? "0",
            Subtotal = cart.SubTotal.ToString() ?? "0"
        };
        return model;
    }
}