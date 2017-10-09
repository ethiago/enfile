using System;
using System.Net;
using System.Runtime.Serialization;

namespace Enfile.ApiIntegration
{
    [Serializable]
    public class IntegrationException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonFrase { get; set; }
        public string Content { get; set; }

        public IntegrationException(HttpStatusCode statusCode, string reasonFrase) : this(statusCode, reasonFrase, null){}

        public IntegrationException(HttpStatusCode statusCode, string reasonFrase, Exception innerException) : base($"{statusCode}: {reasonFrase}.", innerException)
        {
            this.StatusCode = statusCode;
            this.ReasonFrase = reasonFrase;
        }

        public IntegrationException SetContent(string content)
        {
            this.Content = content;
            return this;
        }
    }
}