using Catalogo.Products.Dtos;
using MediatR;
using Shared.CQRS;

namespace Catalogo.Products.CreateProduct;

//lo que se necesita para crear un producto
public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

//lo que se devuelve al crear un producto
public record CreateProductResult(Guid id);

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Aquí iría la lógica para crear el producto
        // Por ahora, devolvemos un ID ficticio
        var productId = Guid.NewGuid();
        return Task.FromResult(new CreateProductResult(productId));
    }
}


