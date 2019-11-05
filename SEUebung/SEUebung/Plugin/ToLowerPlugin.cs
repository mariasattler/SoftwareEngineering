using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace SEUebung.Plugin
{
    public class ToLowerPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            if(req.Url.Segments[0] == "toLower")
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            Response res = new Response();
            if(req.Method == "GET")
            {
                res.SetContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "html\\tolower.html")));
                res.StatusCode = 200;
                res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html");
            }
            if(req.Method == "POST")
            {
                string bodytext = WebUtility.UrlDecode(req.ContentString.Substring(5));
                res.AddHeader(FixStrings.HTTP.CONTENT_TYPE, "text/html;charset=utf-8");
                res.SetContent(@"<!DOCTYPE html><title>Result</title><head>" +
                        "<link rel='stylesheet' type='text/css' href='static/html/styles.css'>" +
                        "</head>" +
                        "<html><body><p>Everything to lower</p>"+
                        "<textarea readonly cols='50' rows='12'>" + bodytext.ToLower().Replace("+", " ") + "</textarea>"+
                        @"<form action='http://localhost:8080/toLower' method='get'>"+ 
                        "<div><button>Go back</button></div></form>"+
                       @"<form action='http://localhost:8080/' method='get'>" +
                        "<button> Get back to the start screen!</button></form> "+
                        "</body></html>");
                res.StatusCode = 200;
            }

            return res;
        }
        
    }
   
}
