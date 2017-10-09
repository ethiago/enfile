using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfile.Commons.ApiContract.v1;
using Enfile.Commons.Model;
using Enfile.Core.Interfaces;
using Enfile.Core.Model;
using Enfile.Presentation.Filter;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using AppFile = Enfile.Core.Model.File;
using Enfile.Presentation.Mvc;

namespace Enfile.Presentation.v1.Controllers
{
    [Route("api/v1/[controller]")]
    public class FileController : Controller
    {
        public IFileService _service;
        public FileController(IFileService service)
        {
            this._service = service;
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] FileRequest request)
        {
            if( !Enum.TryParse(typeof(FileEncoding),request.Encoding, out var obj))
            {
                return BadRequest($"Unrecognize encoding {request.Encoding}");
            }

            FileEncoding fileEncoding = (FileEncoding)obj;
            
            var filedto = _service.CreateFile(request.UniqueIdentifier, request.FileName, request.ClearingDate, fileEncoding);

            return Ok(ConvertFromModel(filedto));
        }

        [HttpGet]
        [Route("{id}/info")]
        public async Task<FileResponse> GetFileInfo([FromRoute] Guid id)
        {
            var filedto = await _service.GetFileInfo(id);

            return ConvertFromModel(filedto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] Guid id)
        {
            var filedto = await _service.GetFileInfo(id);

            if(filedto == null) return BadRequest();

            var fileBytes = _service.GetFileBytes(filedto);

            return new FileCallbackResult(new MediaTypeHeaderValue("text/plain"), 
            async (outputStream, _) => 
            {
                foreach(var block in fileBytes)
                {
                    await outputStream.WriteAsync(block, 0, block.Length);
                }
            })
            {
                FileDownloadName = filedto.FileName
            };
        }

        [HttpGet]
        [Route("info")]
        public async Task<IEnumerable<FileResponse>> GetFileInfo([FromQuery] string filename)
        {
            var fileList = await _service.GetFileInfo(filename);

            return fileList.Select(f => ConvertFromModel(f));
        }

        [HttpPost]
        [ValidateModel]
        [Route("{fileId}/content")]
        public async Task<IActionResult> PostContent([FromRoute] Guid fileId, [FromBody] ContentBlockApi request)
        {
            var filedto = await _service.GetFileInfo(fileId);

            if(filedto == null) return BadRequest();
            
            var contentDto = _service.AddContentToFile( fileId, request.ExternalCodeId, request.Priority, request.Base64Content);

            return Ok(ConvertFromModel(filedto));
        }

        [HttpGet]
        [Route("{fileId}/content")]
        public async Task<IEnumerable<ContentBlockApi>> GetFileContent([FromRoute] Guid fileId)
        {
            var fileContentDto = await _service.GetFileContent(fileId);

            return fileContentDto.Select(c => ConvertFromModel(c));
        }


        private FileResponse ConvertFromModel(AppFile file)
        {
            return new FileResponse()
            {
                Id = file.Id,
                FileName = file.FileName,
                ClearingDate = file.ClearingDate,
                Encoding = file.Encoding,  
            };
        }

        private ContentBlockApi ConvertFromModel(ContentBlock content)
        {
            return new ContentBlockApi()
            {
                FileID = content.FileID,
                ExternalCodeId = content.ExternalCodeId,
                Priority = content.Priority,
                Base64Content = content.Base64Content,
                CretedAt = content.CretedAt
            };
        }
    }
}