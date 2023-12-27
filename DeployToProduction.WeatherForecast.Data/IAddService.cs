using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data;

public interface IAddService
{
    Task<AddPost> PostAsync(AddRequest request);
}