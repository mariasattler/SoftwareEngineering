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
    /// TestPlugin
    /// </summary>
    public class StartPlugin : IPlugin 
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
            IResponse res = (IResponse)new Object();
            
           // res.AddHeader(FixStrings.HTTP.CONTENT_LANGUAGE, "de");
            res.SetContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "html\\index.html")));
            res.StatusCode = 200;
            return res;
        }
    }
}
