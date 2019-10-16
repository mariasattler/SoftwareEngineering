using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    public class Request : IRequest
    {
        public Request(Stream ns)
        {
            HeaderRequest = new Dictionary<string, string>();
            //need to read the request Stream
            StreamReader reader = new StreamReader(ns, Encoding.UTF8);

            // Read the Header line by line
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line)) //if line is empty, then it's end of header
                    break;

                // Parse HTTP Protocol Parameters
                if (line.Contains(':'))
                {
                    string[] headerline = line.Split(':');
                    string key = headerline[0].ToLower().Trim();//tolower um zu vergleichen
                    string value = headerline[1].Trim();
                    HeaderRequest.Add(key, value);
                }
                else
                {
                    ParseFirstHeaderLine(line);
                }
            }
            IsValid = CheckValidation();
        }
       
        public string Method{get; private set;}
        public IUrl Url{get; private set;}
        public IDictionary<string, string> HeaderRequest{get; private set;}
        public bool IsValid {get; private set;}
        #region [private]
        private void ParseFirstHeaderLine(string line)
        {
            string[] elements = line.Split(' ');
            Method = elements[0].ToUpper();
            Url = new Url(elements[1]);          
        }
        private bool CheckValidation()
        {
            if (Url == null)
                return false;
            return true;
        }
        #endregion
    }
}
