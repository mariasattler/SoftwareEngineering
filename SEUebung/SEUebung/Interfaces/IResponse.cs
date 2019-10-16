using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.Interfaces
{
    public interface IResponse
    {
        string Status { get; }
        IDictionary<string, string> Header { get; }
        int ContentLength {get; }
        string ContentLanguage { get; }
        string ContentType { get; }
        void SetContent(string content);
        void Send(Stream ns);
        void AddHeader(string header, string value);
        void SetContent(Byte[] content);
        void SetStatuscode(int status);
    }
}
