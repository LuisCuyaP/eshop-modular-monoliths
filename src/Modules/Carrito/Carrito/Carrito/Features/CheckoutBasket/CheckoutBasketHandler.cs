using System.Text.Json;
using MassTransit;
using Shared.Messaging.Events;

namespace Carrito.Carrito.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckout cannot be null.");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required.");
    }
}

internal class CheckoutBasketHandler(BasketDbContext dbContext) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    //private readonly IBasketService _basketService;

    /* public CheckoutBasketHandler(IBasketService basketService)
    {
        _basketService = basketService;
    } */

    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        //var result = await _basketService.CheckoutBasketAsync(request.BasketCheckout);
        //return new CheckoutBasketResult(result);

        // get existing basket with total price
        // set totalPrice on basketcheckout event message
        // send basket checkout event to rabbitmq using masstransit
        // delete the basket


        // CHECKOUT SIN OUTBOUX
        /*                      var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
                                var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
                                eventMessage.TotalPrice = basket.TotalPrice;
                                await bus.Publish(eventMessage, cancellationToken);
                                await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);
                                return new CheckoutBasketResult(true); */


        // recordad que la publicacion del evento lo hara la clase outBoxProcessor
        // que se ejecuta en segundo plano y se encarga de publicar los eventos
        // abro una transacción para evitar inconsistencias
        await using var transactions = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // get existing basket with total price
            var basket = await dbContext.ShoppingCarts
                .Include(b => b.Items)
                .SingleOrDefaultAsync(b => b.UserName == command.BasketCheckout.UserName, cancellationToken);

            if (basket == null)
            {
                throw new BasketNotFoundException($"Basket for user {command.BasketCheckout.UserName} not found.");
            }

            // set totalPrice on basketcheckout event message
            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            // set message para la tabla outbox
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };
            dbContext.OutboxMessages.Add(outboxMessage);

            // delete the basket
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            // commit transaction
            await transactions.CommitAsync(cancellationToken);

            return new CheckoutBasketResult(true);
        }
        catch
        {
            // rollback transaction
            await transactions.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);

        }
    }
}