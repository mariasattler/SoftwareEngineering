using SEUebung.Database;
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
    public class GetTemperature : IPlugin
    {
        private DBConnect db = new DBConnect();
        private bool xmlrest = false;
        private bool noDaySearch = false;
        private bool daySearch = false;
        private string year = "2008";
        private string month = "1";
        private string day = string.Empty;

        public float CanHandle(IRequest req)
        {
            
            if (req.Url.Segments.Length == 4 && req.Url.Segments[0] == "GetTemperature") // rest abfrage
            {
                int fieldcount = 0;
                if (CheckFormat(req.Url.Segments[1], 1999, 2019))
                {
                    year = req.Url.Segments[1];
                    fieldcount++;
                }
                   
                if (CheckFormat(req.Url.Segments[2], 1, 12))
                {
                    month = req.Url.Segments[2];
                    fieldcount++;
                }
                    
                if (CheckFormat(req.Url.Segments[3], 1, 31))
                {
                    day = req.Url.Segments[3];
                    fieldcount++;
                }
                    
                if (fieldcount != 3)
                    return 0.0f;
                xmlrest = true;
                return 1.0f;
            }
            if (req.Url.Segments[0] == "GetTemperature" && req.ContentLength == 0) //first attempt
            {
                noDaySearch = true;
                return 1.0f;
            }
            if (req.Url.Segments[0] == "GetTemperature" && req.ContentLength != 0 && req.ContentString.StartsWith("day=")) //search attempt
            {
                int fieldcount = 0;
                string[] bodysplit = req.ContentString.Split('&');
                if (bodysplit[0].Split('=').Length == 2 && CheckFormat(bodysplit[0].Split('=')[1], 1, 31))
                {
                    day = bodysplit[0].Split('=')[1];
                    fieldcount++;
                }
                   
                if (bodysplit[1].Split('=').Length == 2 && CheckFormat(bodysplit[1].Split('=')[1], 1, 12))
                {
                    month = bodysplit[1].Split('=')[1];
                    fieldcount++;
                }
                 
                if (bodysplit[2].Split('=').Length == 2 && CheckFormat(bodysplit[2].Split('=')[1], 1999, 2019))
                {
                    year = bodysplit[2].Split('=')[1];
                    fieldcount++;
                }
                   

                if (fieldcount != 3)
                    return 0.0f;
                daySearch = true;
                return 1.0f;
            }
            if(req.Url.Segments[0] == "GetTemperature" && req.ContentLength != 0 && req.ContentString.StartsWith("get")) //paging
            {
                noDaySearch = true;
                if (req.ContentString == "getnext=")
                    GetNextDate(true);
                if (req.ContentString == "getprevious=")
                    GetNextDate(false);
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            Response res = new Response
            {
                StatusCode = 200
            };
            if (xmlrest)
                XMLRes(res);
            else
            {
                if (noDaySearch)
                    res = Search(res, false);
                if (daySearch)
                    res = Search(res, true);
            }
            return res;
        }
        private Response Search(Response res, bool withday)
        {
            noDaySearch = false;
            daySearch = false;
            res.StatusCode = 200;
            List<DBEntity> result = new List<DBEntity>();
            if (!withday)
                 result = db.Select(false, year, month, null);
            if (withday)
                result = db.Select(true, year, month, day);

            StringBuilder body = new StringBuilder();
            body.Append(@"<!DOCTYPE html><title>Result</title><head>
                        <link rel='stylesheet' type='text/css' href='static/html/styles.css'></head><html><body><h1>Temperature</h1>");
            body.Append(@"<form id='searchForm' action='http://localhost:8080/GetTemperature' method='post'>");
            body.Append("<div><p>You need to fill out every field</p><textarea name='day' id='day'>Day</textarea>");
            body.Append("<textarea name='month' id='month'>Month</textarea>");
            body.Append("<textarea name='year' id='year'>Year</textarea></div>");
            body.Append("<div><button id ='searchbutton'>Search</button></div></form>");
            body.Append("<table style='text-align:center;'><tr><th>Date</th><th>Time</th><th>Temperature</th></tr>");

            foreach (DBEntity e in result)
            {
                body.Append($"<tr><td>{e.day}.{e.month}.{e.year}</td><td>{e.time}</td><td>{e.temp}</td>");
            }
            body.Append(@"</table><form action='http://localhost:8080/GetTemperature' method='post'><button name='getprevious'>Previous Month</button></from>
                                 <form action='http://localhost:8080/GetTemperature' method='post'><button name='getnext'>Next Month</button></form>
                             <div><form action='http://localhost:8080/' method'get'><button>Get back to the start screen!</button></form></body></html>");

            res.SetContent(body.ToString());
            return res;
        }
        private Response XMLRes(Response res)
        {
            xmlrest = false;
            List<DBEntity> result = db.Select(true, year, month, day);
            year = "2008";
            month = "1";
            using (XmlWriter writer = XmlWriter.Create("entity.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Element");
                foreach (DBEntity e in result)
                {
                    writer.WriteStartElement("Temperature");
                    writer.WriteElementString("id", e.id.ToString());
                    writer.WriteElementString("day", e.day.ToString());
                    writer.WriteElementString("month", e.month.ToString());
                    writer.WriteElementString("year", e.year.ToString());
                    writer.WriteElementString("time", e.time);
                    writer.WriteElementString("temp", e.temp.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndDocument();
                writer.Flush();
            }
            res.SetContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "entity.xml")));
            res.ContentType = "text/xml";

            return res;
        }
        private bool CheckFormat(string value, int min, int max)
        {
            if (Int32.TryParse(value, out int outvalue))
            {
                if (outvalue >= min && outvalue <= max)
                    return true;
            }
            return false;
        }
        private void GetNextDate(bool next)
        {
            int intyear = Int32.Parse(year);
            int intmonth = Int32.Parse(month);
            if (next)
            {
                if (intmonth == 12)
                {
                    month = "1";
                    intyear++;
                    year = intyear.ToString();
                }
                else
                {
                    intmonth++;
                    month = intmonth.ToString();
                }
                    
                
            }
            else
            {
                if (intmonth == 1)
                {
                    month = "12";
                    intyear--;
                    year = intyear.ToString();
                }
                else
                {
                    intmonth--;
                    month = intmonth.ToString();
                }
                    
            }
        }
    }
}
