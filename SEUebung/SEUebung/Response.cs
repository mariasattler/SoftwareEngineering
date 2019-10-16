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
        private string ServerName;

        public Response()
        {
            Header = new Dictionary<string, string>();
            ServerName = "Test Maria";
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
                Header.TryGetValue(StringHelper.HTTP.CONTENT_LENGTH, out value);
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
                if (!Header.ContainsKey(StringHelper.HTTP.CONTENT_LANGUAGE))
                    return null;
                return Header[StringHelper.HTTP.CONTENT_LANGUAGE];
            }
        }

        public string ContentType
        {
            get
            {
                if (!Header.ContainsKey(StringHelper.HTTP.CONTENT_TYPE))
                    return null;
                return Header[StringHelper.HTTP.CONTENT_TYPE];
            }
        }


        public IDictionary<string, string> Header { get; private set; }

        public void AddHeader(string header, string value)
        {
            Header[header] = value;
        }

        public void Send(Stream ns)
        {
            if (!String.IsNullOrEmpty(ContentType) && ContentLength <= 0)
            {
                throw new InvalidOperationException("Sending a content type without content is not allowed");
            }

            // Write Header Data
            StreamWriter headerWriter = new StreamWriter(ns, Encoding.ASCII);
            headerWriter.NewLine = "\r\n";
            headerWriter.WriteLine("HTTP/1.1 {0}", Status);
            headerWriter.WriteLine("Server: {0}", ServerName);
            foreach (var item in Header)
            {
                headerWriter.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            headerWriter.WriteLine();
            headerWriter.Flush();

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
        public void SetContent(Byte[] content)
        {
            this.contentBytes = content;
            Header.Add(StringHelper.HTTP.CONTENT_LENGTH, content.Length.ToString());
        }

        public void SetContent(string content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));
        }
		public void SetStatuscode(int status)
        {
            string value = null;
            StringHelper.HTTP.STATUS_CODES.TryGetValue(status, out value);
			if(value != null)
            {
                Status = status.ToString() + " - " + value;
            }
        }
    }
}
