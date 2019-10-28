using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    public class Response : IResponse
    {
        private int statuscode = 0;
        private Byte[] contentBytes = null;

        public Response(){}
        /// <summary>
        /// Returns the Status of the Response as a String 
        /// </summary>
        public string Status{ get; private set;} = string.Empty;
        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
        public int ContentLength { get; private set; } = 0;
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
        /// returns a Dictionary of the Values from the Header 
        /// </summary>
        public IDictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
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

            responseheader.WriteLine();
            responseheader.WriteLine("HTTP/1.1 {0}", Status);
            responseheader.WriteLine("Server: {0}", ServerHeader);

            Console.WriteLine();
            Console.WriteLine("HTTP/1.1 {0}", Status);
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
            if(content != null)
            {
                this.contentBytes = content;
                Headers.Add(FixStrings.HTTP.CONTENT_LENGTH, content.Length.ToString());
                ContentLength = content.Length;
            }
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
                    Status = value.ToString() + " "+ outvalue.ToUpper();
            }
        }
        /// <summary>
        /// sets and returns the ServerHeader
        /// </summary>
        public string ServerHeader { get; set; } = "BIF-SWE1-Server";
    }
}
