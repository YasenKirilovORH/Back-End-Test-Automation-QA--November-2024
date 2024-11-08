using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using DemoNunitTest;


namespace UnitTestsDemo
{
    public class Tests
    {
        private RestClient client;

        string issuesEndpoint = "/repos/testnakov/test-nakov-repo/issues";

        Random random;

        // We will store each created issue in this variable but we set a default value if we need to run only test for editing without creating issue first
        long createdIssueNumber = 8153;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                MaxTimeout = 3000,
                Authenticator = new HttpBasicAuthenticator("YasenKirilovORH", "Here paste API Token")
            };

            this.client = new RestClient(options);

            random = new Random();
        }

        [Test]
        public void Test_GitHubAPIRequest()
        {
            // Arrange
            var request = new RestRequest(issuesEndpoint, Method.Get);

            // Act
            var response = client.Get(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]

        public void Test_GetAllIssuesFromARepo()
        {
            // Arrange
            var request = new RestRequest(issuesEndpoint, Method.Get);

            // Act
            var response = client.Execute(request);
            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            // Assert
            Assert.That(issues.Count > 1);
            foreach (var issue in issues)
            {
                Assert.That(issue.id, Is.GreaterThan(0));
                Assert.That(issue.number, Is.GreaterThan(0));
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.body, Is.Not.Empty);
            }
        }

        private Issue CreateIssue(string title, string body)
        {
            
            var request = new RestRequest(issuesEndpoint);
            request.AddBody(new { body, title });
            var response = client.Execute(request, Method.Post);

            
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }
        //Method which will do generate random string
        public string GenerateRandomStringByPrefix(string prefix)
        {
            int randomNumber = random.Next(9999, 100000);

            return prefix + randomNumber;
        }
        [Test]

        public void Test_CreateGitHubIssue()
        {
            // Arrange
            string title = GenerateRandomStringByPrefix("Title");
            string body = GenerateRandomStringByPrefix("Body");

            // Act
            var issue = CreateIssue(title, body);
            createdIssueNumber = issue.number;

            // Assert
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.Not.Empty);
            Assert.That(issue.title, Is.EqualTo(title));
            Assert.That(issue.body, Is.Not.Empty);
            Assert.That(issue.body, Does.Contain(body));
        }

        [Test]

        public void Test_EditIssue()
        {
            // Arrange
            string updatedTitle = "Changing the name of the issue title";
            var request = new RestRequest(issuesEndpoint + "/" + createdIssueNumber.ToString());
            request.AddBody(new
            {
                title = updatedTitle
            }
            );

            // Act
            var response = client.Execute(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(response.Content, Is.Not.Empty);
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(updatedTitle));
        }

        // Testing Zippopotamus

        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "5000", "Veliko Turnovo")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("GB", "B1", "Birmingham")]
        [TestCase("DE", "01067", "Dresden")]

        public void TestZippopotamus(string countryCode, string zipCode, string expectedPlace)
        {
            // Arrange
            var restClient = new RestClient("https://api.zippopotam.us");
            var httpRequest = new RestRequest(countryCode + "/" + zipCode);

            // Act
            var httpResponse = restClient.Execute(httpRequest);
            var location = JsonSerializer.Deserialize<Location>(httpResponse.Content);

            // Assert
            StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);
        }
    }
}