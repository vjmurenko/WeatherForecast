using Adds.Core;
using Microsoft.AspNetCore.Mvc;

namespace Adds.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AddsController : ControllerBase
{
    private static readonly AddResponse[] Responses =
    {
        new() { Message = "Moscow good city", Href = "https://prog.msk.ru/" },
        new() { Message = "Come to Moscow pal", Href = "https://xn--80aaacfpel4cc2n3b.xn--80adxhks/schedule.html" },
        new() { Message = "Moscow Bro, come here!", Href = "https://bridgetomoscow.com/moscow-tours" }
    };

    private readonly ILogger<AddsController> _logger;

    public AddsController(ILogger<AddsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public Task<AddResponse> Post(AddRequest request)
    {
        if (request.City == "Moscow")
        {
            var response = new AddResponse() { Message = "Moscow is amazing city! So lucky to live here", Href = "https://moscowwalking.ru/" };
            return Task.FromResult(response);
        }

        var index = new Random(DateTime.Now.Microsecond).Next(Responses.Length - 1);
        return Task.FromResult(Responses[index]);
    }
}