using Shared.Contracts.CQRS;

namespace Catalogo.Contracts.Products.Features.GetProductById;
public record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);