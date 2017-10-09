using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Enfile.ApiIntegration
{
    public static class HttpClientExtension
    {
        public static async Task<TResponse> PostAsJsonAsync<TResponse, TRequest>(this HttpClient client, string path, TRequest request)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(request));

            HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // TODO: Retypattern
            HttpResponseMessage response = await client.PostAsync(path, content);

            return await HttpClientExtension.ProcessResponse<TResponse>(response);
        }

        public static async Task<TResponse> GetAsync<TResponse>(this HttpClient client, string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);

            return await HttpClientExtension.ProcessResponse<TResponse>(response);
        }

        public static async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response)
        {
            if( response.IsSuccessStatusCode && response.Content != null)
            {
                var contentStr = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResponse>(contentStr) );
            }

            throw new IntegrationException(response.StatusCode, response.ReasonPhrase)
                .SetContent( await response.Content?.ReadAsStringAsync() );
        }
    }
}