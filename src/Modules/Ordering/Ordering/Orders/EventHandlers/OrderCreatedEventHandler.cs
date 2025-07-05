namespace Ordering.Orders.EventHandlers;
public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;

        //despues de guardar la orden, se puede publicar un evento de dominio de integracion aqui, y que lo consuma un microservicio de inventario
        //para actualizar stock, o un microservicio de notificaciones para enviar un correo al cliente, etc.
    }
}
