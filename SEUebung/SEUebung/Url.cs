using SEUebung.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SEUebung
{
    /// <summary>
    /// URL class
    /// </summary>
    public class Url : IUrl
    {
        private string _url;
        private const char  _beginparams = '?';
        private const char _beginfragement = '#';
        private const char _paramsplit = '&';
        private const char _segementdivider = '/';
        IDictionary<string, string> _paramdict = new Dictionary<string, string>();
        string _filename = string.Empty;
        /// <summary>
        /// constructor of the URL
        /// </summary>
        /// <param name="url"></param>
        public Url(string url)
        {
            _url = url;
            //Dictionary mit Parametern füllen
            if (_url != null && _url != string.Empty && _url.Contains(_beginparams))
            {
                string paramstring1 = _url.Split(_beginparams)[1];
                string paramstring = paramstring1.Split(_beginfragement)[0];
                foreach (string item in paramstring.Split(_paramsplit))
                {
                    _paramdict.Add(item.Split('=')[0], item.Split('=')[1]);
                }

                //get filename
                if (Segments.Length > 0)
                {
                    string lastsegment = Segments[Segments.Length - 1];
                    if (lastsegment.Contains(_beginparams))
                    {
                        string file = lastsegment.Split(_beginparams)[0];
                        if (file != string.Empty) //wenn empty gibt es kein file
                        {
                            _filename = file;
                        }
                    }
                }
            }
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
                return _url.Split(_beginparams)[0];
            }
        }

        public IDictionary<string, string> Parameter
        {
            get
            {
                return _paramdict;
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
                string newurl = _url;
                if (newurl[0] == _segementdivider)
                {
                    newurl = newurl.Remove(0, 1);
                }
                return newurl.Split(_segementdivider);

            }
        }

        public string FileName
        {
            get
            {
                return _filename;
            }
        }

        public string Extension
        {
            get
            {
                if (FileName != string.Empty)
                    return "." + FileName.Split('.')[1];
                return string.Empty;
            }
        }

        public string Fragment
        {
            get
            {
                if (Segments.Length > 0)
                {
                    string lastsegment = Segments[Segments.Length - 1];
                    string[] fragment = lastsegment.Split(_beginfragement);
                    if (fragment.Length == 2)
                    {
                        return fragment[1];
                    }
                    return string.Empty;
                }
                else
                    return string.Empty;

            }
        }
    }
}
