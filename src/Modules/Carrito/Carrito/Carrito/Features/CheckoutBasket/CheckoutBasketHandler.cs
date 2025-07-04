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

internal class CheckoutBasketHandler(IBasketRepository repository, IBus bus) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
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

        var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        await bus.Publish(eventMessage, cancellationToken);
        await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);
        return new CheckoutBasketResult(true);
    }
}