namespace Carrito.Carrito.Dtos;
public record ShoppingCartDto(
    Guid Id,
    string UserName,
    List<ShoppingCartItemDto> Items
    );