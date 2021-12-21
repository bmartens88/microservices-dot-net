using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> items = new();

    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items => items;

    public ShoppingCart(int userId)
    {
        UserId = userId;
    }

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems)
    {
        foreach (var item in shoppingCartItems) items.Add(item);
    }

    public void RemoveItems(int[] productCatalogueIds, IEventStore eventStore)
    {
        items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogId));
    }
}

public record ShoppingCartItem(
    int ProductCatalogId,
    string ProductName,
    string Description,
    Money Price)
{
    public virtual bool Equals(ShoppingCartItem? obj)
    {
        return obj != null && ProductCatalogId.Equals(obj.ProductCatalogId);
    }

    public override int GetHashCode()
    {
        return ProductCatalogId.GetHashCode();
    }
}

public record Money(string Currency, decimal Amount);