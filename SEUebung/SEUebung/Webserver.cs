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
        private PluginManager pm = new PluginManager();
        /// <summary>
        /// Constructor of webserver
        /// </summary>
        public Webserver(){}
        /// <summary>
        /// starts the Webserver. Waits for a client to connect and than threds the Request
        /// </summary>
        public void Start()
        {
            server = new TcpListener(Adress, Port);
            server.Start();
            Console.WriteLine("Waiting for connection..."); //chrome sendet eine req nach dem icon - daher connected der 2x
            while (true)
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
                    if (plugintodo != null)
                    {
                        IResponse res = plugintodo.Handle(req);
                        res.Send(ns);
                    }

                    if (current == 0)
                    {
                        SendBadRequest(ns, "400");
                    }
                }
            }
            client.Close();
            Console.WriteLine("\nWaiting for connection...");
          
        }
        public  void SendBadRequest(Stream ns, string error)
        {
            var localURL = Path.Combine(Directory.GetCurrentDirectory(), "html\\"+error+"error.html");
            Console.WriteLine(localURL);
            Response err = new Response();
            err.StatusCode = 400;
            err.SetContent(File.ReadAllBytes(localURL));
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
