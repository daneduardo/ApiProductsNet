using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Controllers;
using ProductsApi.Data;
using ProductsApi.Models;
using System.Reflection.Metadata;
using Xunit;

namespace ProductsApi.Tests;

public class ProductsControllerTests
{
    private readonly ProductsController _controller;
    private readonly ProductDbContext _context;

    public ProductsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _context = new ProductDbContext(options);
        _controller = new ProductsController(_context);
    }
    [Fact]
    public async Task CreateProduct_ShouldReturnCreated_WhenValid()
    {
        var product = new Product
        {
            Name = "Monitor Gamer",
            Price = 350,
            Stock = 10,
            Description = "4K 144Hz"
        };
        var result = await _controller.CreateProduct(product);

        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returnedProduct = createdResult.Value.Should().BeOfType<Product>().Subject;

        returnedProduct.Name.Should().Be("Monitor Gamer");
        _context.Products.Count().Should().Be(1);
    }
    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        _context.Products.Add(new Product { Name = "Mouse", Price = 10, Stock = 10 });
        _context.Products.Add(new Product { Name = "Monitor", Price = 100, Stock = 10 });
        await _context.SaveChangesAsync();

        var result = await _controller.GetProduct();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var products = okResult.Value.Should().BeAssignableTo<IEnumerable<Product>>().Subject;

        products.Count().Should().Be(2);
    }
    [Fact]
    public async Task DeleteProduct_ShouldRemoveFromDb()
    {
        var product = new Product { Id = Guid.NewGuid(), Name = "To Delete", Price = 10 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await _controller.DeleteProduct(product.Id);

        _context.Products.Should().BeEmpty();
    }
}
