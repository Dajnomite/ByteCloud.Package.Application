using ByteCloud.Package.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ByteCloud.Package.Application
{
    public static class Secret
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static readonly string baseUrl = "https://ByteCloud.Package.com/16d1cf68-883a-4daa-936a-1133c88ed4cd/vault";

        public static string Read(string secret, string key)
        {
            Log.Assert(!string.IsNullOrWhiteSpace(secret), "Secret was null or whitespace");
            Log.Assert(!string.IsNullOrWhiteSpace(key), "Key was null or whitespace");

            // Create an anonymous type for the request body
            var readRequest = new
            {
                Key = key,
                Secret = secret
            };

            // Serialize the anonymous type to JSON
            var jsonContent = JsonSerializer.Serialize(readRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send a POST request to the Read endpoint
            var response = httpClient.PostAsync($"{baseUrl}/Vault/read", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                Log.Error($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}. Response content: {errorContent}");
            }

            Log.Assert(response.IsSuccessStatusCode, "Request failed");

            // Deserialize the response to a string
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        public static bool Write(string secret, string key, string value)
        {
            Log.Assert(!string.IsNullOrWhiteSpace(secret), "Secret was null or whitespace");
            Log.Assert(!string.IsNullOrWhiteSpace(key), "Key was null or whitespace");
            Log.Assert(value != null, "Value was null");  // Depending on your use case, an empty value might be allowed, hence != null check.

            // Create an anonymous type for the request body
            var writeRequest = new
            {
                Key = key,
                Value = value,
                Secret = secret
            };

            // Serialize the anonymous type to JSON
            var jsonContent = JsonSerializer.Serialize(writeRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send a POST request to the Write endpoint
            var response = httpClient.PostAsync($"{baseUrl}/Vault/write", content).Result;

            Log.Assert(response.IsSuccessStatusCode, $"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");

            return response.IsSuccessStatusCode;
        }
    }
}