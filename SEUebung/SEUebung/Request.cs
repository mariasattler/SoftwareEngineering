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
            Headers = new Dictionary<string, string>();
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
                    Headers.Add(key, value);
                }
                else
                {
                    ParseFirstHeaderLine(line);
                }
            }

            //Read Body - only in POST
            if (ContentLength > 0)
            {
                char[] body = new char[ContentLength];
                reader.Read(body, 0, ContentLength);
                ContentString = new string(body);
                ContentBytes = Encoding.UTF8.GetBytes(ContentString);
                ContentStream = new MemoryStream(ContentBytes);
            }

            IsValid = CheckValidation();
        }

        public string Method { get; private set; }
        public IUrl Url { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public bool IsValid { get; private set; }

        public string UserAgent
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.USER_AGENT_LW, out value);
                return value;
            }
        }

        public int HeaderCount { get { return Headers.Count; } }

        public int ContentLength
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_LENGTH_LW, out value);
                if (value != string.Empty)
                    return Int32.Parse(value);
                return 0;
            }
        }

        public string ContentType
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_TYPE_LW, out value);
                return value;
            }
        }

        public Stream ContentStream
        {
            get; private set;
        }

        public string ContentString
        {
            get; private set;
        }

        public byte[] ContentBytes
        {
            get; private set;
        }

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
