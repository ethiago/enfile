using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Enfile
{
    public class FMSession : IFMSession
    {
        public readonly HttpClient _client;

        public FMSession(string baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
            _client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public HttpClient Client => _client;
    }
}