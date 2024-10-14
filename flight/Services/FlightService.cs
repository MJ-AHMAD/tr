namespace flight.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class FlightService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FlightService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var apiKey = _configuration["Amadeus:ApiKey"];
            var apiSecret = _configuration["Amadeus:ApiSecret"];
            var request = new HttpRequestMessage(HttpMethod.Post, "https://test.api.amadeus.com/v1/security/oauth2/token");
            var content = new StringContent($"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(responseContent);
            return jsonDocument.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<string> SearchFlightsAsync(string origin, string destination, string departureDate)
        {
            var accessToken = await GetAccessTokenAsync();
            var requestUrl = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode={origin}&destinationLocationCode={destination}&departureDate={departureDate}&adults=1";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}