using System.Net.Http.Json;
using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data;

public class AddService : IAddService
{
    private readonly HttpClient _httpClient;

    public AddService(string addServerUrl)
    {
        _httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromMilliseconds(300),
            BaseAddress = new Uri(addServerUrl)
        };
    }

    public async Task<AddPost> PostAsync(AddRequest request)
    {
        AddPost? post;
        try
        {
            var response = await _httpClient.PostAsJsonAsync("adds", request);
            response.EnsureSuccessStatusCode();
            post = await response.Content.ReadFromJsonAsync<AddPost>();
        }
        catch (Exception e)
        {
            throw new AddsServiceException("Adds error", e);
        }

        if (post == null)
        {
            throw new AddsServiceException("Post is empty");
        }

        return post;
    }

    [Serializable]
    public class AddsServiceException : Exception
    {
        public AddsServiceException()
        {
        }

        public AddsServiceException(string message) : base(message)
        {
        }

        public AddsServiceException(string message, Exception? e) : base(message, e)
        {
        }
    }
}