using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace aspire2.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{

    private readonly ILogger<ProductsController> _logger;
    private readonly ProductDbContext _context;

    public ProductsController(ILogger<ProductsController> logger, ProductDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _context.Products.ToListAsync();
    }
}