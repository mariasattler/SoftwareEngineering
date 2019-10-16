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

        public Webserver()
        {
        }

        public void Start()
        {
            server = new TcpListener(Adress, Port);
            running = true;
            server.Start();
            while (running)
            {
                Console.WriteLine("Wainting for connection...");
                Socket client = server.AcceptSocket();
                Console.WriteLine("Connected!");
                ThreadPool.QueueUserWorkItem(HandleRequest, client);
            }
        }

        public void HandleRequest(object socketclient)
        {
            Socket client = (Socket)socketclient;
            using(NetworkStream ns = new NetworkStream(client))
            {
                Request req = new Request(ns);
                if (!req.IsValid)
                {
                    Response res = new Response();
                    res.AddHeader(StringHelper.HTTP.CONTENT_TYPE, "text/html");
                    res.AddHeader(StringHelper.HTTP.CONTENT_LANGUAGE, "de");
                    res.SetContent("<!DOCTYPE html><html><body><h1>Test</h1><h3>hi</h3></body></html>");
                    res.SetStatuscode(200);
                    res.Send(ns);
                }
                else
                {
                    SendBadRequest(ns);
                }

            }

            client.Close();
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
            err.SetStatuscode(400);
            err.SetContent(bytes);
            err.AddHeader(StringHelper.HTTP.CONTENT_TYPE, "text/html");
            err.AddHeader(StringHelper.HTTP.CONTENT_LANGUAGE, "de");
            err.Send(ns);
        }

        //sets the IPAdress standard to localhost
        public IPAddress Adress { get; set; } = IPAddress.Loopback;
        //Sets the Port to 8080
        public int Port { get; set; } = 8080;
    }

}
