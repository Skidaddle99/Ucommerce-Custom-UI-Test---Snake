using CustomUITest;
using Ucommerce.Extensions.InProcess.Abstractions.Extensions;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Set up services
builder.Services.AddUcommerce(builder.Configuration)
    .AddBackOffice().AddCustomComponentWithPath(
        "products-home_main-top", "TestHeader", "snake-game", "MyComponent.js")
    .AddWebSite()
    .AddInProcess<ContextParser>()
    .UcommerceBuilder
    .AddSearch()
    .UcommerceBuilder
    .AddElasticsearch()
    .AddPayments()
    .Build();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// configure and run
app.UseUcommerce()
    .UseEndpoints(u => { u.UseUcommerceEndpoints(); })
    .UsePayments()
    .UseBackOfficeUi().UseCustomComponentServer();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();