using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.Interfaces
{
    public interface IRequest
    {
        string Method { get; }
        IUrl Url { get; }
        IDictionary<string, string> HeaderRequest { get; }
        bool IsValid { get; }
    }
}
