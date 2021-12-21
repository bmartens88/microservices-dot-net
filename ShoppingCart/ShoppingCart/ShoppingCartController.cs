using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart;

[Route("/shoppingcart")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartStore _shoppingCartStore;
    private readonly IProductCatalogClient _productCatalogClient;
    private readonly IEventStore _eventStore;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalogClient,
        IEventStore eventStore)
    {
        _shoppingCartStore = shoppingCartStore;
        _productCatalogClient = productCatalogClient;
        _eventStore = eventStore;
    }

    [HttpGet("{userId:int}")]
    public ShoppingCart Get(int userId)
    {
        return _shoppingCartStore.Get(userId);
    }

    [HttpPost("{userId:int}/items")]
    public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = _shoppingCartStore.Get(userId);
        var shoppingCartItems =
            await _productCatalogClient.GetShoppingCartItems(productIds);
        shoppingCart.AddItems(shoppingCartItems);
        _shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }

    [HttpDelete("{userId:int}/items")]
    public ShoppingCart Delete(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart =
            _shoppingCartStore.Get(userId);
        shoppingCart.RemoveItems(productIds, _eventStore);
        _shoppingCartStore.Save(shoppingCart);
        return shoppingCart;
    }
}