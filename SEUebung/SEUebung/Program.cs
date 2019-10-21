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
            string url = @"/foo/bar/test.jpg";
            Url test = new Url(url);
            string tedsf = test.Segments[0];
            Console.WriteLine(tedsf);
            Webserver server = new Webserver();
            server.Start();
            Console.ReadLine();
        }
    }
}
