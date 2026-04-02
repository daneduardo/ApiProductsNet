using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(connectionString) && connectionString != "000")
{
    // Si hay una cadena de conexión válida, usamos Postgres
    builder.Services.AddDbContext<ProductDbContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("--> Using PostgreSQL Database");
}
else
{
    // Si no hay conexión (o es el placeholder 000), usamos Memoria para el examen
    builder.Services.AddDbContext<ProductDbContext>(options =>
        options.UseInMemoryDatabase("ProductsInventory"));
    Console.WriteLine("--> Using In-Memory Database (Demo Mode)");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// MIDDLEWARE
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1");
    c.RoutePrefix = string.Empty; 
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ProductDbContext>();
        if (context.Database.IsNpgsql())
        {
            context.Database.Migrate();
            Console.WriteLine("--> Database migrations applied successfully.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Could not run migrations: {ex.Message} (Expected if using In-Memory)");
    }
}

app.Run();
