using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace DemoAPITesting
{
    internal class Program
    {
        private static object responseDeserialized;

        static void Main(string[] args)
        {
            // built-in system.text.json nugget package
            WeatherForecast forecast = new WeatherForecast();
            string weatherInfo = System.Text.Json.JsonSerializer.Serialize(forecast);

            Console.WriteLine(weatherInfo);

            string jsonString = File.ReadAllText("D:\\Programing\\QA\\Back-End Test Automation\\C#\\demoData.json");
            WeatherForecast forecastDeserialized = System.Text.Json.JsonSerializer.Deserialize<WeatherForecast>(jsonString);

            // newtonsoft json package
            WeatherForecast forecastNewtonSoft = new WeatherForecast();
            string weatherInfoNS = JsonConvert.SerializeObject(forecastNewtonSoft, Formatting.Indented);

            Console.WriteLine(weatherInfoNS);

            jsonString = File.ReadAllText("D:\\Programing\\QA\\Back-End Test Automation\\C#\\demoData.json");
            WeatherForecast forecastDeserializedNS = JsonConvert.DeserializeObject<WeatherForecast>(jsonString);

            // working with anonymous objects
            var json = @"{ 'firstName': 'Svetlin', 'lastName': 'Nakov', 'jobTitle': 'Technical Trainer'}";

            var template = new
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                JobTitle = string.Empty
            };

            var person = JsonConvert.DeserializeAnonymousType(json, template);
            Console.WriteLine(person);

            // applying naming convention to the class properties
            WeatherForecast weatherForecastResolver = new WeatherForecast();
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string snakeCaseJson = JsonConvert.SerializeObject(weatherForecastResolver, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            Console.WriteLine(snakeCaseJson);

            // JObject
            var jsonAsString = JObject.Parse(@"{'products': [{'name': 'Fruits', 'products':['apple', 'bannana']}, {'name': 'Vegetables', 'products':['cucumber']}]}");

            var products = jsonAsString["products"].Select(t => string.Format("{0} ({1})", t["name"], string.Join(", ", t["products"])));

            // Executing simple HTTP get rquest
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/users/softuni/repos", Method.Get);
            var response = client.Execute(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);

            // Using URL Segment Parameters
            var requestURLSegments = new RestRequest("/repos/{user}/{repo}/issues/{id}", Method.Get);
            requestURLSegments.AddUrlSegment("user", "testnakov");
            requestURLSegments.AddUrlSegment("repo", "test-nakov-repo");
            requestURLSegments.AddUrlSegment("id", 1);
            var responseURLSegments = client.Execute(requestURLSegments);
            Console.WriteLine(responseURLSegments.StatusCode);
            Console.WriteLine(responseURLSegments.Content);

            // Deserialize json response
            var requestDeserializing = new RestRequest("/users/softuni/repos", Method.Get);
            var responseDeserializing = client.Execute(requestDeserializing);
            var repos = JsonConvert.DeserializeObject<List<Repo>>(responseDeserializing.Content);

            // HTTP post request with authentication
            var clientWithAuthentication = new RestClient(new RestClientOptions("https://api.github.com")
            {
                // You should paste the API token
                Authenticator = new HttpBasicAuthenticator("YasenKirilovORH", "Here paste API Token")
            });

            var requestWithAuthentication = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Post);
            requestWithAuthentication.AddHeader("Content-Type", "application.json");
            requestWithAuthentication.AddJsonBody(new { title = "TestTitle-Posting with C#", body = "This is a test POST request via C#" });
            var responseWithAuthentication = clientWithAuthentication.Execute(requestWithAuthentication);
            Console.WriteLine(responseWithAuthentication.StatusCode);
        }
    }
}
