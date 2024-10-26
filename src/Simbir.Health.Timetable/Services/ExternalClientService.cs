using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Simbir.Health.Timetable.Services
{
    public class ExternalClientService
    {
        private readonly HttpClient _httpClient;

        public ExternalClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> SendRequestAsync<TResponse>(string accessToken, string url, HttpMethod method, object? body)
        {
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (body != null)
            {
                var jsonBody = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseBody);

        }

    }
}
