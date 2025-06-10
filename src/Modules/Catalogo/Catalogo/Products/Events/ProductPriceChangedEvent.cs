namespace Catalogo.Products.Events;
public record ProductPriceChangedEvent(Product Product)
    : IDomainEvent;