using System;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;

namespace ConsoleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TokenResponse tokenResponse;

            using (var discoveryDocumentHttpClient = new HttpClient())
            {
                var discoveryDocument = 
                    await discoveryDocumentHttpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

                Console.WriteLine(discoveryDocument.TokenEndpoint);
                
                tokenResponse = await discoveryDocumentHttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "console",
                    ClientSecret = "secret",
                    Scope = "api"
                });
                Console.WriteLine(tokenResponse.AccessToken);
            }

            using (var apiHttpClient = new HttpClient())
            {
                apiHttpClient.SetBearerToken(tokenResponse.AccessToken);
            
                var response = await apiHttpClient.GetStringAsync("https://localhost:5003/WeatherForecast");
                Console.WriteLine(response);
            }


        }
    }
}
