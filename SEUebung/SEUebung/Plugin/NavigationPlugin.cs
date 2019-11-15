using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SEUebung.Plugin
{
    /// <summary>
    /// NavigationPlugin
    /// </summary>
    public class NavigationPlugin : IPlugin
    {

        private static object reloadLock = new object();
        private static bool reloading = false;

        private bool search = false;
        private bool load = false;
        private Dictionary<string, List<string>> StreetCityMap = null;
        /// <summary>
        /// constructor NavigationPlugin
        /// </summary>
        public NavigationPlugin()
        {
            ReadMap();
        }
        /// <summary>
        /// checks if the plugin can handle the req
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public float CanHandle(IRequest req)
        {
            if (req.Url.Segments[0] == "Navigation" && req.ContentLength != 0 && req.ContentString.StartsWith("street=")) //searching
            {
                if (req.ContentString.Split('=').Length == 2)
                {
                    search = true;
                    return 1.0f;
                }
                else
                    return 0.0f;
            }
            if (req.Url.Segments[0] == "Navigation" && req.ContentLength != 0 && req.ContentString.StartsWith("load="))
            {
                load = true;
                return 1.0f;
            }
            if (req.Url.Segments[0] == "Navigation") //first attempt
                return 1.0f;
            return 0.0f;
        }
        /// <summary>
        /// handles the req
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IResponse Handle(IRequest req)
        {
            Response res = new Response();
            res.StatusCode = 200;
            res.ContentType = "text/html";

            lock (reloadLock)
            {
                if (reloading)
                {
                    res.SetContent("Warning! Map is currently loading! Try again.");
                    return res;
                }
            }

            StringBuilder body = new StringBuilder();
            body.Append(@"<!DOCTYPE html><title>Result</title><head>
                        <link rel='stylesheet' type='text/css' href='static/html/styles.css'></head><html><body><h1>Navigation</h1>");
            body.Append(@"<p>Type in the street name.</p><form id='searchForm' action='http://localhost:8080/Navigation' method='post'>");
            body.Append("<div><textarea name='street' id='street'></textarea></br>");
            body.Append("<button id ='searchbutton'>Search</button></div></form>");
            if (search)
            {
                search = false;
                body.Append("</br>");
                if (StreetCityMap.ContainsKey(req.ContentString.Split('=')[1]))
                {
                    body.Append($"<p> The street name '{req.ContentString.Split('=')[1]}' exists in these cities:</p><ul>");
                    List<string> cities = StreetCityMap[req.ContentString.Split('=')[1]];

                    foreach (string c in cities)
                    {
                        body.Append($"<li>{c}</li>");
                    }
                    body.Append("</ul>");
                }
                else
                    body.Append($"<p>There is no street named '{req.ContentString.Split('=')[1]}' in that map.</p>");
            }

            if (load) //user wants to reload
            {
                if (!reloading)
                {

                    if (!ReadMap())
                    {
                        res.SetContent("Warning! Map is currently loading! Try again.");
                        return res;
                    }
                }
            }

            body.Append(@"<div><form id= 'loadagain' action='http://localhost:8080/Navigation' method='post'><button name='load'>Load the map again!</button></form></div>");
            body.Append(@"<div><form action='http://localhost:8080/' method'get'><button>Get back to the start screen!</button></form></div></body></html>");

            res.SetContent(body.ToString());
            return res;
        }

        private bool ReadMap()
        {
            lock (reloadLock)
            {
                if (reloading)
                {
                    return false;
                }
                reloading = true;
            }

            StreetCityMap = new Dictionary<string, List<string>>();
            string street = null;
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "streetmap\\liechtenstein.osm")))
            {
                using (XmlReader reader = XmlReader.Create(Path.Combine(Directory.GetCurrentDirectory(), "streetmap\\liechtenstein.osm")))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {

                        // Check if the node is an element
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        // Check if current element is a "tag" element
                        if (reader.Name == null || reader.Name != "tag")
                        {
                            continue;
                        }

                        // Retrieve key value
                        string key = reader.GetAttribute("k");

                        // Check if element contains street name
                        if (key != null && (key == "name" || key == "addr:street"))
                        {
                            street = reader.GetAttribute("v");
                        }

                        // Save city value
                        if (street != null && key != null && (key == "city" || key == "addr:city"))
                        {
                            if (!StreetCityMap.ContainsKey(street))
                            {
                                StreetCityMap.Add(street, new List<string>());
                            }
                            if (!StreetCityMap[street].Contains(reader.GetAttribute("v")))
                                StreetCityMap[street].Add(reader.GetAttribute("v"));

                            street = null;
                        }
                    }
                }
            }

            lock (reloadLock)
            {
                reloading = false;
            }
            return true;
        }
    }
}
