using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.FixStrings
{/// <summary>
/// new class of fix Strings for the Reponse
/// </summary>
    public class ResponseStrings
    {
        /// <summary>
        /// new List for the possible Header Values in the Response
        /// </summary>
        public static List<string> headerValuesResponse = new List<string>();

        /// <summary>
        /// new instance of the possible response strings
        /// </summary>
        static ResponseStrings()
        {
            headerValuesResponse.Add("Accept-Patch");
            headerValuesResponse.Add("Accept-Ranges");
            headerValuesResponse.Add("Age");
            headerValuesResponse.Add("Allow");
            headerValuesResponse.Add("Alt-Svc");
            headerValuesResponse.Add("Cache-Control");
            headerValuesResponse.Add("Connection");
            headerValuesResponse.Add("Content-Disposition");
            headerValuesResponse.Add("Content-Encoding");
            headerValuesResponse.Add("Content-Language");
            headerValuesResponse.Add("Content-Length");
            headerValuesResponse.Add("Content-Location");
            headerValuesResponse.Add("Content-Range");
            headerValuesResponse.Add("Content-Type");
            headerValuesResponse.Add("Date");
            headerValuesResponse.Add("Delta-Base");
            headerValuesResponse.Add("ETag");
            headerValuesResponse.Add("Expires");
            headerValuesResponse.Add("IM");
            headerValuesResponse.Add("Link");
            headerValuesResponse.Add("Location");
            headerValuesResponse.Add("Pragma");
            headerValuesResponse.Add("Proxy-Authenticate");
            headerValuesResponse.Add("Public-Key-Pins");
            headerValuesResponse.Add("Retry-After");
            headerValuesResponse.Add("Server");
            headerValuesResponse.Add("Set-Cookie");
            headerValuesResponse.Add("Strict-Transport-Security");
            headerValuesResponse.Add("Trailer");
            headerValuesResponse.Add("Transfer-Encoding");
            headerValuesResponse.Add("Tk");
            headerValuesResponse.Add("Upgrade");
            headerValuesResponse.Add("Vary");
            headerValuesResponse.Add("Via");
            headerValuesResponse.Add("Warning");
            headerValuesResponse.Add("WWW-Authenticate");
        }
    }
}
