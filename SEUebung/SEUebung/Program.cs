using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEUebung
{
    class Program
    {
        static void Main(string[] args)
        {
            Webserver server = new Webserver();
            server.Start();
            Console.ReadLine();
        }
    }
}
