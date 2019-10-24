using SEUebung.Interfaces;
using SEUebung.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SEUebung
{
    public class Webserver
    {
        private TcpListener server = null;
        private Boolean running = false;
        /// <summary>
        /// Constructor of webserver
        /// </summary>
        public Webserver()
        {
        }
        /// <summary>
        /// starts the Webserver. Waits for a client to connect and than threds the Request
        /// </summary>
        public void Start()
        {
            server = new TcpListener(Adress, Port);
            running = true;
            server.Start();
            Console.WriteLine("Wainting for connection...");
            while (running)
            {
                Socket client = server.AcceptSocket();
                Console.WriteLine("Connected!");
                ThreadPool.QueueUserWorkItem(HandleRequest, client);
            }
        }
        /// <summary>
        /// handles the request
        /// </summary>
        /// <param name="socketclient"></param>
        public void HandleRequest(object socketclient)
        {
            Socket client = (Socket)socketclient;
            using(NetworkStream ns = new NetworkStream(client))
            {
                Request req = new Request(ns);
                if (req.IsValid)
                {
                    IPlugin test = new TestPlugin();
                    if (test.CanHandle(req) == 0)
                        SendBadRequest(ns);
                    else
                    {
                        IResponse res = test.Handle(req);
                        res.Send(ns);
                    }
                }
                else
                {
                    SendBadRequest(ns);
                }

            }
            client.Close();
            Console.WriteLine("Wainting for connection...");
        }
        private  void SendBadRequest(Stream ns)
        {
            var localURL = Path.Combine(Directory.GetCurrentDirectory(), "html\\400error.html");
            byte[] bytes;
            using (FileStream fsstream = new FileStream(localURL,
            FileMode.Open, FileAccess.Read))
            {

                // Read the source file into a byte array.
                bytes = new byte[fsstream.Length];
                int numBytesToRead = (int)fsstream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsstream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;                
            }

            Console.WriteLine(localURL);
            Response err = new Response();
            err.StatusCode = 400;
            err.SetContent(bytes);
            err.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html");
            err.AddHeader(FixStrings.HTTP.CONTENT_LANGUAGE, "de");
            err.Send(ns);
           
        }

        //sets the IPAdress standard to localhost
        private IPAddress Adress { get; set; } = IPAddress.Loopback;
        //Sets the Port to 8080
        private int Port { get; set; } = 8080;
    }

}
