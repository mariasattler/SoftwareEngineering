using Microsoft.AspNetCore.StaticFiles;
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
            if (req != null && req.IsValid && req.Url.Segments[0] == "static")
            {
                return 1.0f;
            }

            return 0.0f;
        }

        /// <summary>
        /// handles the Plugin and returns the response
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IResponse Handle(IRequest req)
        {
            Response res = new Response();
            string replacedurl = req.Url.RawUrl.Replace("/", "\\").Remove(0,8);
            string localURL = Path.Combine(Directory.GetCurrentDirectory(), replacedurl);
            if (File.Exists(localURL))
            {
                string filename = req.Url.Segments[req.Url.Segments.Length - 1];
                string mimetype = Get(filename);
       
                res.SetContent(File.ReadAllBytes(localURL));
                res.StatusCode = 200;
                res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, mimetype);
            }
            else
            {
                res.SetContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "html\\404error.html")));
                res.StatusCode = 404;
                res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html");
            }

            return res;
        }



        /// <summary>
        /// getting the Mimetyp from the filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string Get(string fileName)
        {
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
