using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.Plugin
{
    /// <summary>
    /// StaticDataPlugin
    /// </summary>
    public class StaticDataPlugin : IPlugin
    {
        /// <summary>
        /// checks if the Plugin is valid
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public float CanHandle(IRequest req)
        {
            if (RightPath(req))
            {
                return 2.0f;
            }

            return 0.0f;
        }

        private bool RightPath(IRequest req)
        {
            if (req.Url.RawUrl == "/")
            {
                return true;
            }
            else
                return false;
        }



        /// <summary>
        /// handles the Plugin and returns the response
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IResponse Handle(IRequest req)
        {
            IResponse res = new Response();
            string replacedurl = req.Url.RawUrl.Replace("/", "\\");
            string localURL = Path.Combine(Directory.GetCurrentDirectory(), replacedurl);
            if (File.Exists(localURL))
                Console.WriteLine("mach die response mit dem file");
            else
                res.StatusCode = 404;

            res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html");
           // res.AddHeader(FixStrings.HTTP.CONTENT_LANGUAGE, "de");
            res.SetContent("<!DOCTYPE html><html><body><h1>Test</h1><h3>hi</h3></body></html>");
            res.StatusCode = 200;
            return res;
        }
    }
}
