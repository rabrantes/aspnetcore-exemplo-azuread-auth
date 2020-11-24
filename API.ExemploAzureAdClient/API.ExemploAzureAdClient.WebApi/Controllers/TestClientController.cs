using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.ExemploAzureAdClient.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestClientController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly AzureAd azureAd;

        public TestClientController(IHttpClientFactory httpClientFactory, AzureAd azureAd)
        {
            this.azureAd = azureAd;

            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Get()
        {
            string[] scopes = new string[] { azureAd.Scope };

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder
                .Create(azureAd.ClientId)
                .WithClientSecret(azureAd.ClientSecret)
                .WithAuthority(new Uri(azureAd.Authority))
                .Build();

            AuthenticationResult result;

            result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            using (var httpClient = httpClientFactory.CreateClient("WeatherForeCast"))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.AccessToken);

                HttpResponseMessage responseMessage = await httpClient.GetAsync("/WeatherForecast");

                responseMessage.EnsureSuccessStatusCode();

                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                return Ok(jsonResponse);
            } 
        }
    }
}
