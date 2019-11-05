using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung.Plugin
{
    /// <summary>
    /// TestPlugin
    /// </summary>
    public class TestPlugin : IPlugin
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
                return 0.1f;
            }
            if (CheckParams(req))
                return 0.1f;

            return 0.0f;
        }

        private bool CheckParams(IRequest req)
        {
            if (req.Url.Parameter.ContainsKey("test"))
            {
                return true;
            }
            return false;
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
            Response res = new Response();
            res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html");
         //   res.AddHeader(FixStrings.HTTP.CONTENT_LANGUAGE, "de");
            res.SetContent("<!DOCTYPE html><html><body><h1>Test</h1><h3>hi</h3></body></html>");
            res.StatusCode = 200;
            return res;
        }
    }
}
