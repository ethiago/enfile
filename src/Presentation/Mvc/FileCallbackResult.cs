using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Logging;

namespace Enfile.Presentation.Mvc
{
    public class FileCallbackResult : FileResult
    {
        private Func<Stream, ActionContext, Task> _callback;

        public FileCallbackResult(MediaTypeHeaderValue contentType, Func<Stream, ActionContext, Task> callback)
            : base(contentType?.ToString())
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            _callback = callback;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var executor = new FileCallbackResultExecutor((ILoggerFactory)context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)));
            return executor.ExecuteAsync(context, this);
        }

        private sealed class FileCallbackResultExecutor : FileResultExecutorBase
        {
            public FileCallbackResultExecutor(ILoggerFactory loggerFactory)
                : base(CreateLogger<FileCallbackResultExecutor>(loggerFactory))
            {
            }

            public Task ExecuteAsync(ActionContext context, FileCallbackResult result)
            {
                SetHeadersAndLog(context, result, null);
                return result._callback(context.HttpContext.Response.Body, context);
            }
        }
    }
}