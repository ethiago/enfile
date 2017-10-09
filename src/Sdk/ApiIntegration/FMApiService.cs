using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Enfile.Commons.ApiContract.v1;
using Newtonsoft.Json;
using System.IO;

namespace Enfile.ApiIntegration
{
    internal class FMApiService
    {
        private IFMSession _session;

        public FMApiService(IFMSession session)
        {
            this._session = session;
        }

        public async Task<FileResponse> CreateFileAsync(FileRequest request)
        {

            return await this._session.Client.PostAsJsonAsync<FileResponse, FileRequest>("/api/v1/file", request);
        }

        public async Task<FileResponse> GetFileInfoAsync(Guid id)
        {
            return await this._session.Client.GetAsync<FileResponse>($"/api/v1/file/{id}/info");
        }

        public async Task<List<FileResponse>> GetFileInfoAsync(string fileName)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["filename"] = fileName;
            
            return await this._session.Client.GetAsync<List<FileResponse>>($"/api/v1/file/info?{query}");
        }

        public async Task<ContentBlockApi> CreateContentBlockAsync(ContentBlockApi request)
        {
            return await this._session.Client.PostAsJsonAsync<ContentBlockApi, ContentBlockApi>($"/api/v1/file/{request.FileID}/content", request);
        }

        internal async Task<Stream> GetFileStreamAsync(Guid id)
        {
            HttpResponseMessage response = await this._session.Client.GetAsync($"/api/v1/file/{id}");

            if( response.IsSuccessStatusCode && response.Content != null)
            {
                return await response.Content.ReadAsStreamAsync();
            }

            throw new IntegrationException(response.StatusCode, response.ReasonPhrase)
                .SetContent( await response.Content?.ReadAsStringAsync() );
        }

        internal async Task<IEnumerable<ContentBlockApi>> GetFileContentList(Guid id)
        {
            return await this._session.Client.GetAsync<IEnumerable<ContentBlockApi>>($"/api/v1/file/{id}/content");
        }
    }
}