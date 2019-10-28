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
    /// <summary>
    /// Webserver class
    /// </summary>
    public class Webserver
    {
        private TcpListener server = null;
        private Boolean running = false;
        private PluginManager pm = null;
        /// <summary>
        /// Constructor of webserver
        /// </summary>
        public Webserver()
        {
            pm = new PluginManager(); //im Constructor, damit es nicht bei jedem Thread neu aufgerufen wird
        }
        /// <summary>
        /// starts the Webserver. Waits for a client to connect and than threds the Request
        /// </summary>
        public void Start()
        {
            server = new TcpListener(Adress, Port);
            running = true;
            server.Start();
            Console.WriteLine("Wainting for connection..."); //chrome sendet eine req nach dem icon - daher connected der 2x
            while (running)
            {
                Socket client = server.AcceptSocket();
                Console.WriteLine("Connected!\n");
                ThreadPool.QueueUserWorkItem(HandleRequest, client);
            }
        }
        /// <summary>
        /// handles the request, checks if request is valid
        /// </summary>
        /// <param name="socketclient"></param>
        public void HandleRequest(object socketclient)
        {
            Socket client = (Socket)socketclient;
            using (NetworkStream ns = new NetworkStream(client))
            {
                Request req = new Request(ns);

                if (req.IsValid)
                {
                    List<IPlugin> plugins = (List<IPlugin>)pm.Plugins;
                    IPlugin plugintodo = null;
                    float current = 0;
                    float highest = 0;
                    foreach (IPlugin p in plugins)
                    {
                        current = p.CanHandle(req);
                        if (current > highest)
                        {
                            plugintodo = p;
                            highest = current;
                        }
                    }
                    if(plugintodo != null)
                    {
                        IResponse res = plugintodo.Handle(req);
                        res.Send(ns);
                    }


                    if (current == 0 || (req.Url.Segments.Length == 1 && req.Url.Segments[0] == "favicon.ico")) 
                    {
                        SendBadRequest(ns);
                    }
                }
            }
            client.Close();
            Console.WriteLine("\nWainting for connection...");
          
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
