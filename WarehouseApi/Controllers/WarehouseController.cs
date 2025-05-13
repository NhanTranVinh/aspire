using Microsoft.AspNetCore.Mvc;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WarehouseController(ILogger<WarehouseController> logger, WarehouseStore store) : ControllerBase
{

    private readonly ILogger<WarehouseController> _logger = logger;

    [HttpGet(Name = "GetQuantity")]
    public async Task<IResult> Get([FromQuery]string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return Results.BadRequest();

        var qty = await store.GetProductQuantity(productName);

        return Results.Ok(qty);
    }
    
    [HttpPost(Name = "SetQuantity")]
    public async Task<IResult> Post([FromBody] SetProductQuantityRequestBody body)
    {
        if (string.IsNullOrWhiteSpace(body.ProductName))
            return Results.BadRequest();

        await store.SetProductQuantity(body.ProductName, body.Quantity);

        return Results.Ok();
    }
}

public record SetProductQuantityRequestBody(string ProductName, int Quantity);