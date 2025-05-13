using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Persistence;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(
    ILogger<ProductsController> logger,
    ProductDbContext context,
    WarehouseHttpClient warehouse)
    : ControllerBase
{
    [HttpGet(Name = "GetProducts")]
    public async Task<IResult> Get()
    {
        Tracer.CurrentSpan.SetAttribute("myRandomAttribute", "myRandomValue");
        var products = await context.Products.ToListAsync();
        return Results.Ok(products);
    }
    
    [HttpGet("qty", Name = "GetProductQuantity")]
    public async Task<IResult> Get([FromQuery]string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return Results.BadRequest();

        var qty = await warehouse.GetProductQuantity(productName);

        return Results.Ok(qty);
    }
    
    [HttpPost("qty", Name = "SetProductQuantity")]
    public async Task<IResult> Post([FromBody] SetProductQuantityRequestBody body)
    {
        logger.LogInformation("Setting product quantity to {qty} for {productName}", body.Quantity, body.ProductName);
        if (string.IsNullOrWhiteSpace(body.ProductName))
            return Results.BadRequest();

        await warehouse.SetProductQuantity(body.ProductName, body.Quantity);

        return Results.Ok();
    }
}

public record SetProductQuantityRequestBody(string ProductName, int Quantity);