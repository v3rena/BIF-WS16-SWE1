using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.Text.RegularExpressions;

namespace MyWebServer
{
    public class Url : IUrl
    {
        public Url()
        {
            
        }

        /// <summary>
        /// Parses URL.
        /// </summary>
        /// <param name="raw"></param>
        public Url(string raw)
        {
            if (raw != null)
            {
                RawUrl = raw;

                var RegEx = new Regex(@"(\?|\&)([^=]+)\=([^\&]+)");

                var matches = RegEx.Matches(raw);

                foreach (Match item in matches)
                {
                    Parameter.Add(item.Groups[2].Value, item.Groups[3].Value);
                    ParameterCount++;
                }

                var PathEnd = raw.IndexOf('?');
                var FragStart = raw.IndexOf('#');

                if (ParameterCount == 0)
                {
                    Path = raw;
                }
                else
                {
                    Path = raw.Substring(0, PathEnd);
                }

                if (FragStart >= 0)
                {
                    Path = raw.Substring(0, FragStart);
                    Fragment = raw.Substring(FragStart + 1);
                }

                RegEx = new Regex(@"(\/)([A-Za-z0-9.-]*)");
                matches = RegEx.Matches(Path);

                string[] tempArr = new string[10];

                int i = 0;

                foreach (Match item in matches)
                {
                    tempArr[i] = item.Groups[2].Value;
                    i++;
                }
                
                if(i != 0)
                {
                    Segments = new string[i];

                    for (int k = 0; k < i; k++)
                    {
                        Segments[k] = tempArr[k];
                    }
                }
            }
        }

        /// <summary>
        /// Returns a dictionary with the parameter of the url. Never returns null.
        /// </summary>
        public IDictionary<string, string> Parameter { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns the number of parameter of the url. Returns 0 if there are no parameter.
        /// </summary>
        public int ParameterCount { get; } = 0;

        /// <summary>
        /// Returns the path of the url, without parameter.
        /// </summary>
        public string Path { get; } = string.Empty;

        /// <summary>
        /// Returns the raw url.
        /// </summary>
        public string RawUrl { get; } = string.Empty;

        /// <summary>
        /// Returns the extension of the url filename, including the leading dot. If the url contains no filename, a empty string is returned. Never returns null.
        /// </summary>
        public string Extension { get; } = string.Empty;

        /// <summary>
        /// Returns the filename (with extension) of the url path. If the url contains no filename, a empty string is returned. Never returns null. A filename is present in the url, if the last segment contains a name with at least one dot.
        /// </summary>
        public string FileName { get; } = string.Empty;

        /// <summary>
        /// Returns the url fragment. A fragment is the part after a '#' char at the end of the url. If the url contains no fragment, a empty string is returned. Never returns null.
        /// </summary>
        public string Fragment { get; } = string.Empty;

        /// <summary>
        /// Returns the segments of the url path. A segment is divided by '/' chars. Never returns null.
        /// </summary>
        public string[] Segments { get; } = new string[0];
    }
}
