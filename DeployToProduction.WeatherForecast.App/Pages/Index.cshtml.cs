using DeployToProduction.WeatherForecast.Core.Models;
using DeployToProduction.WeatherForecast.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeployToProduction.WeatherForecast.App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAddService _addService;
    private readonly IForecast _forecast;

    public IndexModel(IForecast forecast, ILogger<IndexModel> logger, IAddService addService)
    {
        _forecast = forecast;
        _logger = logger;
        _addService = addService;
    }

    public IWeather? Weather { get; set; } = default;
    public AddPost AddPost { get; set; }


    public async Task OnGetAsync(string? location = null)
    {
        if (!string.IsNullOrWhiteSpace(location))
        {
            try
            {
                Weather = await _forecast.PredictAsync(location);
                AddPost = await _addService.PostAsync(new AddRequest() { City = location });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}