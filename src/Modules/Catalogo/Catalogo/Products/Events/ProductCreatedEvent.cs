namespace Catalogo.Products.Events;
public record ProductCreatedEvent(Product Product)
    : IDomainEvent;