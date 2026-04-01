using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductsApi.Data;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

        // Usamos una cadena de conexión local (localhost) solo para el diseño
        // Asegúrate de que el puerto 5432 esté mapeado en tu docker-compose para db
        optionsBuilder.UseNpgsql("Host=localhost;Database=product_catalog;Username=devuser;Password=devpassword;Port=5432");

        return new ProductDbContext(optionsBuilder.Options);
    }
}