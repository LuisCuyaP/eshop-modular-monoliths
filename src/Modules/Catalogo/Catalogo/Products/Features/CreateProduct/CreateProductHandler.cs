namespace Catalogo.Products.Features.CreateProduct;

//lo que se necesita para crear un producto
public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

//lo que se devuelve al crear un producto
public record CreateProductResult(Guid Id);

internal class CreateProductHandler(CatalogoDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        //create Product entity from command object
        //save to database
        //return result

        var product = CreateNewProduct(command.Product);

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        var product = Product.Create(
            Guid.NewGuid(),
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);

        return product;
    }
}