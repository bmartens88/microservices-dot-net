using ShoppingCart.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .Scan(selector =>
        selector.FromAssemblyOf<Program>()
            .AddClasses()
            .AsImplementedInterfaces());

builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapGet("/", () => "Hello World!");

app.Run();