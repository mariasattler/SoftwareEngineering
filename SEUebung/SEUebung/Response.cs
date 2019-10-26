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
    /// Response class
    /// </summary>
   
    public class Response : IResponse
    {
        private int statuscode;
        private Byte[] contentBytes = null;
        /// <summary>
        /// constructor of the Response
        /// </summary>
        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
            Status = string.Empty;
        }
        /// <summary>
        /// returns the Statuscode with the Name
        /// </summary>
        public string Status
        {
            get; private set;
        }
        /// <summary>
        /// returns the Length of the request Content
        /// </summary>
        public int ContentLength
        {
            get
            {
                string value = null;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_LENGTH, out value);
                if (value != null)
                    return Int32.Parse(value);
                return 0;
            }
            private set { }
        }
        /// <summary>
        /// returns the Content Language
        /// </summary>
        public string ContentLanguage
        {
            get
            {
                if (!Headers.ContainsKey(FixStrings.HTTP.CONTENT_LANGUAGE))
                    return null;
                return Headers[FixStrings.HTTP.CONTENT_LANGUAGE];
            }
        }
        /// <summary>
        /// returns the ContentType
        /// </summary>
        public string ContentType
        {
            get
            {
                if (!Headers.ContainsKey(FixStrings.HTTP.CONTENT_TYPE))
                    return null;
                return Headers[FixStrings.HTTP.CONTENT_TYPE];
            }
            set { }
        }
        /// <summary>
        /// returns a Dictionary of the Header Values
        /// </summary>
        public IDictionary<string, string> Headers { get; private set; }
        /// <summary>
        /// Adds a Value to the Header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        public void AddHeader(string header, string value)
        {
            Headers[header] = value;
        }
        /// <summary>
        /// sends the response
        /// </summary>
        /// <param name="ns"></param>
        public void Send(Stream ns)
        {
            if (Status == string.Empty)
            {
                throw new InvalidOperationException("Status muss gesetzt werden");
            }
            // Write Data in Header
            StreamWriter head = new StreamWriter(ns, Encoding.ASCII);
            head.NewLine = "\r\n";
            head.WriteLine("HTTP/1.1 {0}", Status);
            head.WriteLine("Server: {0}", ServerHeader);
            Console.WriteLine("\r\n");
            Console.WriteLine("HTTP/1.1 {0}", Status);
            Console.WriteLine("Server: {0}", ServerHeader);

            foreach (var item in Headers)
            {
                head.WriteLine("{0}: {1}", item.Key, item.Value);
                Console.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            head.WriteLine();
            head.Flush();

            // Write Content Data
            if (contentBytes != null)
            {
                    BinaryWriter contentWriter = new BinaryWriter(ns);
                    contentWriter.Write(contentBytes);
                    contentWriter.Flush();
            }
        }
        /// <summary>
        /// sets the Content from the Stream
        /// </summary>
        /// <param name="stream"></param>
        public void SetContent(Stream stream)
        {
            byte[] b;
            using (BinaryReader br = new BinaryReader(stream))
            {
                b = br.ReadBytes((int)stream.Length);
            }
            SetContent(b);
        }
        /// <summary>
        /// Sets the Content from a byte array
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(Byte[] content)
        {
            this.contentBytes = content;
            Headers.Add(FixStrings.HTTP.CONTENT_LENGTH, content.Length.ToString());
        }
        /// <summary>
        /// sets the content from a string
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));
        }
        /// <summary>
        /// sets the Statuscode
        /// </summary>
        public int StatusCode
        {
            get { return statuscode; }
            set
            {
                statuscode   = value;
                string outvalue = null;
                FixStrings.HTTP.STATUS_CODES.TryGetValue(value, out outvalue);
                if (outvalue != null)
                {
                    Status = value.ToString() + " - " + outvalue;
                }
            }
        }
        /// <summary>
        /// sets and returns the ServerHeader
        /// </summary>
        public string ServerHeader { get; set; }
    }
}
