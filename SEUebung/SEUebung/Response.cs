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
        private Byte[] contentBytes;

        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
            Status = string.Empty;
        }

        public string Status
        {
            get; private set;
        }

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

        public string ContentLanguage
        {
            get
            {
                if (!Headers.ContainsKey(FixStrings.HTTP.CONTENT_LANGUAGE))
                    return null;
                return Headers[FixStrings.HTTP.CONTENT_LANGUAGE];
            }
        }
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
        public IDictionary<string, string> Headers { get; private set; }
        public void AddHeader(string header, string value)
        {
            Headers[header] = value;
        }
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
                try
                {
                    BinaryWriter contentWriter = new BinaryWriter(ns);
                    contentWriter.Write(contentBytes);
                    contentWriter.Flush();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //need to set stream;
        public void SetContent(Stream stream)
        {
            byte[] b;
            using (BinaryReader br = new BinaryReader(stream))
            {
                b = br.ReadBytes((int)stream.Length);
            }
            SetContent(b);
        }
        public void SetContent(Byte[] content)
        {
            this.contentBytes = content;
            Headers.Add(FixStrings.HTTP.CONTENT_LENGTH, content.Length.ToString());
        }
        public void SetContent(string content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));
        }
        public int StatusCode
        {
            get { return StatusCode; }
            set
            {
                string outvalue = null;
                FixStrings.HTTP.STATUS_CODES.TryGetValue(value, out outvalue);
                if (outvalue != null)
                {
                    Status = value.ToString() + " - " + outvalue;
                }
            }
        }
        public string ServerHeader { get; set; }
    }
}
