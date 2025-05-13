using StackExchange.Redis;

namespace WarehouseApi;

public class WarehouseStore(IConnectionMultiplexer connection)
{
    public async Task SetProductQuantity(string productName, int qty)
    {
        var db = connection.GetDatabase();
        await db.StringSetAsync(productName, qty.ToString());
    }

    public async Task<int> GetProductQuantity(string productName)
    {
        var db = connection.GetDatabase();
        var value = await db.StringGetAsync(productName);
        if (value.IsNullOrEmpty) return 0;
        return (int)value;
    }
}