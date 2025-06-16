using Catalogo.Data;
using Shared.Data.Seed;

namespace Catalogo.Data.Seed;
public class CatalogoDataSeeder(CatalogoDbContext dbContext)
    : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(InitialData.Products);
            await dbContext.SaveChangesAsync();
        }
    }
}