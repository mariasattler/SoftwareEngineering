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
                    string key = headerline[0].ToLower().Trim();
                    string value = headerline[1].Trim();
                    Headers.Add(key, value);
                }
                else //first Line does not contain ':'
                {
                    ParseFirstHeaderLine(line);
                }
            }

            //Read Body - only in POST Operation
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
        public string Method { get; private set; } = string.Empty;
        /// <summary>
        /// returns the URL
        /// </summary>
        public IUrl Url{get; private set;} = new Url("");
        /// <summary>
        /// returns the a Dictionary of all Header Values from the Request
        /// </summary>
        public IDictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
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
                if(value != null)
                    return value;
                return string.Empty;
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
                int outvalue = 0;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_LENGTH_LW, out value);
                value = null;
                Int32.TryParse(value, out outvalue);
                return outvalue;
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
                if(value != null)
                    return value;
                return string.Empty;
            }
        }
        /// <summary>
        /// returns the Stream
        /// </summary>
        public Stream ContentStream{get; private set;} = null;
        /// <summary>
        /// returns the ContentString
        /// </summary>
        public string ContentString{get; private set;} = null;
        /// <summary>
        /// returns the Content in Bytes
        /// </summary>
        public byte[] ContentBytes{get; private set;} = null;

        #region [private]
        private void ParseFirstHeaderLine(string line)
        {
            string[] elements = line.Split(' ');
            if(elements.Length > 1)
            {
                Method = elements[0].ToUpper();
                Url = new Url(elements[1]);
            }
        }
        private bool CheckValidation()
        {
            if (Url.RawUrl == "" || Method == string.Empty)
                return false;
            return true;
        }
        #endregion
    }
}
