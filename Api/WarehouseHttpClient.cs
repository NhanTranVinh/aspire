namespace Api;

public class WarehouseHttpClient(HttpClient httpClient, ILogger<WarehouseHttpClient> logger) : HttpClient
{
    public Task<int> GetProductQuantity(string productName) =>
        httpClient.GetFromJsonAsync<int>($"/warehouse?productName={productName}");

    public Task SetProductQuantity(string productName, int qty)
    {
        return httpClient.PostAsJsonAsync($"/warehouse",
            new
            {
                ProductName = productName,
                Quantity = qty,
            });
    }
}