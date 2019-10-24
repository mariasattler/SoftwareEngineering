using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    /// <summary>
    /// Request
    /// </summary>
    public class Request : IRequest
    {
        /// <summary>
        /// Constructor for the Request. reads the Header line by line
        /// </summary>
        /// <param name="ns"></param>
        public Request(Stream ns)
        {
            Headers = new Dictionary<string, string>();
            //need to read the request Stream
            StreamReader reader = new StreamReader(ns, Encoding.UTF8);

            // Read the Header line by line
            string line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line)) //if line is empty, then the end of the header is reached
                    break;

                // Parse HTTP Protocol Parameters
                if (line.Contains(':'))
                {
                    string[] headerline = line.Split(':');
                    string key = headerline[0].ToLower().Trim();//tolower um zu vergleichen
                    string value = headerline[1].Trim();
                    Headers.Add(key, value);
                }
                else //first Line does not contain ':'
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
        /// <summary>
        /// returns the Method
        /// </summary>
        public string Method { get; private set; }
        /// <summary>
        /// returns the URL
        /// </summary>
        public IUrl Url { get; private set; }
        /// <summary>
        /// returns the a Dictionary of all Header Values from the Request
        /// </summary>
        public IDictionary<string, string> Headers { get; private set; }
        /// <summary>
        /// checks if the Request is valid
        /// </summary>
        public bool IsValid { get; private set; }
        /// <summary>
        /// returns the UserAgent
        /// </summary>
        public string UserAgent
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.USER_AGENT_LW, out value);
                return value;
            }
        }
        /// <summary>
        /// returns how many lines the Header has
        /// </summary>
        public int HeaderCount { get { return Headers.Count; } }
        /// <summary>
        /// returns the ContentLength
        /// </summary>
        public int ContentLength
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_LENGTH_LW, out value);
                if (value != string.Empty && value != null)
                    return Int32.Parse(value);
                return 0;
            }
        }
        /// <summary>
        /// returns the ContentType
        /// </summary>
        public string ContentType
        {
            get
            {
                string value = string.Empty;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_TYPE_LW, out value);
                return value;
            }
        }
        /// <summary>
        /// returns the Stream
        /// </summary>
        public Stream ContentStream
        {
            get; private set;
        }
        /// <summary>
        /// returns the ContentString
        /// </summary>
        public string ContentString
        {
            get; private set;
        }
        /// <summary>
        /// returns the Content in Bytes
        /// </summary>
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
