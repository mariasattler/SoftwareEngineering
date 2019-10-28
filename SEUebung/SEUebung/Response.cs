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
        private int statuscode = 0;
        private Byte[] contentBytes = null;
        /// <summary>
        /// Constructor of the Response. Sets the ServerHeader to BIF-SWE1-Server.
        /// </summary>
        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
            Status = string.Empty;
        }
        /// <summary>
        /// Returns the Status of the Response as a String 
        /// </summary>
        public string Status
        {
            get; private set;
        }
        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
        public int ContentLength
        {
            get
            {
                string value = string.Empty;
                Int32 outvalue = 0;
                Headers.TryGetValue(FixStrings.HTTP.CONTENT_LENGTH, out value);
                Int32.TryParse(value, out outvalue);
                return outvalue;
            }
        }
        /// <summary>
        /// returns the ContentType
        /// ------ was ist mit der Exception gemeint?
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
        /// returns a Dictionary of the Values from the Header 
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
            // Write Data in Header
            StreamWriter responseheader = new StreamWriter(ns, Encoding.ASCII);
            responseheader.WriteLine("\r\nHTTP/1.1 {0}", Status);
            responseheader.WriteLine("Server: {0}", ServerHeader);
            Console.WriteLine("\nHTTP/1.1 {0}", Status);
            Console.WriteLine("Server: {0}", ServerHeader);
            foreach (var item in Headers)
            {
                responseheader.WriteLine("{0}: {1}", item.Key, item.Value);
                Console.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            responseheader.WriteLine();
            responseheader.Flush();

            // Write the Content Data
            if (contentBytes != null)
            {
                ns.Write(contentBytes, 0, contentBytes.Length);
            }
        }
        /// <summary>
        /// sets the Content from the Stream
        /// </summary>
        /// <param name="stream"></param>
        public void SetContent(Stream stream)
        {
            byte[] b;
            try
            {
                BinaryReader br = new BinaryReader(stream);
                b = br.ReadBytes((int)stream.Length);
                SetContent(b);
            }
            catch(IOException e)
            {
                throw e;
            }
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
                statuscode = value;
                string outvalue = null;
                FixStrings.HTTP.STATUS_CODES.TryGetValue(value, out outvalue);
                if (outvalue != null)
                {
                    Status = value.ToString() + " "+ outvalue.ToUpper();
                }
            }
        }
        /// <summary>
        /// sets and returns the ServerHeader
        /// </summary>
        public string ServerHeader { get; set; }
    }
}
