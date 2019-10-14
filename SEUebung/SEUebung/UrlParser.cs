using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SEUebung
{
    public class UrlParser : IUrl
    {
        private string _url;
        public UrlParser(string url)
        {
            _url = url;
        }
        public string RawUrl
        {
            get
            {
                return _url;
            }
        }

        public string Path
        {
            get
            {
                string withoutpar = _url.Split('?')[0];
                return withoutpar;
            }
        }

        public IDictionary<string, string> Parameter
        {
            get
            {
                IDictionary<string, string> dict = new Dictionary<string, string>();
                if (_url != null && _url != string.Empty && _url.Contains('?'))
                {
                    string paramstring1 = _url.Split('?')[1];
                    string paramstring = paramstring1.Split('#')[0];
                    if (paramstring.Length < 2)
                    {
                        return dict;
                    }
                    else
                    {
                        foreach (string item in paramstring.Split('&'))
                        {
                            dict.Add(item.Split('=')[0], item.Split('=')[1]);
                        }
                        return dict;
                    }
                }
                else
                    return dict;
               
            }
        }

        public int ParameterCount
        {
            get
            {
                return Parameter.Count;
            }
        }

        public string[] Segments
        {
            get
            {
                return _url.Split('/');
            }
        }

        public string FileName
        {
            get
            {
                string lastsegment = Segments[Segments.Length - 1];
                if (lastsegment.Contains('?'))
                {
                    string pfad = lastsegment.Split('?')[0];
                    if (pfad != string.Empty)
                    {
                        return pfad;
                    }
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;              

            }
        }

        public string Extension
        {
            get
            {
                string extension = string.Empty;
                if(FileName != string.Empty)
                {
                    extension = FileName.Split('.')[1];
                    return "." + extension;

                }
                return extension;
            }
        }

        public string Fragment
        {
            get
            {
                string lastsegment = Segments[Segments.Length - 1];
                string[] fragment = lastsegment.Split('#');
                if (fragment.Length == 2)
                {
                    return fragment[1];
                }
                else return string.Empty;
            }
        }
    }
}
