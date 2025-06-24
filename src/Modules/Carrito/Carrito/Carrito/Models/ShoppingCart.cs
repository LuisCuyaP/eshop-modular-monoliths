using Shared.DDD;

namespace Carrito.Carrito.Models;
public class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; private set; } = default!;

    private readonly List<ShoppingCartItem> _items = new();
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}
