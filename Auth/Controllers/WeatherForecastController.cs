using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;

namespace Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        public WeatherForecastController (JwtAuthenticationManager jwtAuthenticationManager)
        {
            this._jwtAuthenticationManager = jwtAuthenticationManager;

        }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        

       
        [Authorize]

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [AllowAnonymous]
        [HttpPost("Authorize")]

        public IActionResult AuthUser([FromBody]user usr)
        {
            var token = _jwtAuthenticationManager.Authenticate(usr.UserName, usr.Password);

            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);


            

        }

    }
    public class user
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}