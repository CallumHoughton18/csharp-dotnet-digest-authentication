using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HTTPClientDigestAuth.IntegrationTests
{
    /// <summary>
    /// Peforms integration tests against the test digest authentication service hosted at httpbin.org
    /// </summary>
    public class HttpClientExtensionTests
    {
        // Httpbin doesn't *actually* contain this user, so don't need to hide the values. It's just a simple
        // http request and response service to perform tests against.
        // https://github.com/postmanlabs/httpbin
        
        private const string Username = "test";
        private const string Password = "password";
        private const string Host = "https://httpbin.org";
        private readonly string _resource = $"/digest-auth/auth/{Username}/{Password}";
        
        [Fact]
        public async Task Should_Authenticate_And_Return_200_Response()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Host);
            var request = GenerateRequestToHttpBin(_resource);
            
            var response = await client.SendWithDigestAuthAsync(request, HttpCompletionOption.ResponseContentRead, Username, Password);
            Assert.True(response.StatusCode == HttpStatusCode.OK);

            var expectedJsonResponse = JToken.Parse($@"{{""authenticated"": true,""user"": ""{Username}""}}");
            var actualBodyText = await response.Content.ReadAsStringAsync();
            var actualJsonResponse = JToken.Parse(actualBodyText);
            
            Assert.Equal(expectedJsonResponse, actualJsonResponse);
        }

        private HttpRequestMessage GenerateRequestToHttpBin(string resourcePath)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourcePath);
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "HttpClientDigestAuthTester");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Connection", "keep-alive");
            return request;
        }
    }
}