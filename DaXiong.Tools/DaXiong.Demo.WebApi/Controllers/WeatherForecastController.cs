using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaXiong.Demo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache _cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            throw new NullReferenceException();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet]
        public async Task<IActionResult> Test(string id)
        {


            _cache.Set("key", "value");
            var cc = _cache.Get("key");
            //绝对过期时间
            _cache.Set("key", "value", new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(1)));
            //滑动过期，最后一次访问的minute分钟后过期
            _cache.Set("key", "value", new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(1)));
            return Ok();
        }
    }
}
