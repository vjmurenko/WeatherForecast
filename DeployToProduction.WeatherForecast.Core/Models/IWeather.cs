namespace DeployToProduction.WeatherForecast.Core.Models
{
    public interface IWeather
    {
        DateTime Date { get; set; }
        string Location { get; set; }
        Temperature Temperature { get; set; }
        WeatherKind Kind { get; set; }
    }
}
