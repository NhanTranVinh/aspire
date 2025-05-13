namespace Api;

public class WeatherHttpClient(HttpClient httpClient) : HttpClient
{
    public Task<List<WeatherForecast>?> GetWeatherForecast() =>
        httpClient.GetFromJsonAsync<List<WeatherForecast>>("/weatherforecast");
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}