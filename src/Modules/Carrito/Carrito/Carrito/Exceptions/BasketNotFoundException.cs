using Shared.Exceptions;

namespace Carrito.Carrito.Exceptions;
public class BasketNotFoundException(string userName)
    : NotFoundException("ShoppingCart", userName)
{
}