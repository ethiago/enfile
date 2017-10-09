using System.Net.Http;

namespace Enfile
{
    public interface IFMSession
    {
        HttpClient Client { get; }
    }
}