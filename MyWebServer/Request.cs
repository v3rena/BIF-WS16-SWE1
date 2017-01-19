using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.Text.RegularExpressions;

namespace MyWebServer
{
    public class Request : IRequest
    {
        /// <summary>
        /// Reads network stream request, parses URL, adds it to the Headers Dictionary and saves content.
        /// </summary>
        public Request(Stream network)
        {
            if(network != null)
            {
                StreamReader sr = new StreamReader(network);

                string temp = sr.ReadLine();

                if(temp != null && temp.Length > 0)
                {
                    int countWords = temp.Split().Length;

                    if (countWords > 1)
                    {
                        IsValid = true;

                        string[] tokens = temp.Split(' ');

                        Method = tokens[0].ToUpper();

                        if(Method != "POST" && Method != "GET")
                        {
                            IsValid = false;
                        }
                        else
                        {
                            Url = new Url(tokens[1]);

                            while ((temp = sr.ReadLine()) != "")
                            {
                                var RegEx = new Regex(@"([A-Za-z0-9-_]*): ([A-Za-z0-9-,;=\.\/ \(\)-_]*)");
                                var matches = RegEx.Matches(temp);

                                foreach (Match item in matches)
                                {
                                    if (item.Groups[1].Value == "User-Agent")
                                    {
                                        UserAgent = item.Groups[2].Value;
                                    }
                                    else if(item.Groups[1].Value == "Content-Type")
                                    {
                                        ContentType = item.Groups[2].Value;
                                    }

                                    Headers.Add(item.Groups[1].Value.ToLower(), item.Groups[2].Value);
                                    HeaderCount++;
                                }
                            }

                            if (Headers.ContainsKey("content-length"))
                            {
                                temp = Headers["content-length"];
                                UTF8Encoding encoding = new UTF8Encoding();
                                int length;
                                Int32.TryParse(temp, out length);

                                ContentBytes = new byte[length];
                                ContentLength = length;

                                char[] tempArr = new char[length];

                                int read = sr.ReadBlock(tempArr, 0, length);
                                ContentBytes = encoding.GetBytes(tempArr);

                                ContentString = new string(tempArr);

                                ContentStream = new MemoryStream();
                                ContentStream.Write(ContentBytes, 0, ContentLength);
                                ContentStream.Seek(0, SeekOrigin.Begin);
                            }

                            /*while((temp = sr.ReadLine()) != null)
                            {
                                ContentLength = temp.Length;

                                ContentString = temp;

                                UTF8Encoding encoding = new UTF8Encoding();
                                ContentBytes = new byte[encoding.GetByteCount(temp)];
                                ContentBytes = encoding.GetBytes(temp);

                                ContentStream = new MemoryStream();
                                ContentStream.Write(ContentBytes, 0, ContentLength);
                                ContentStream.Seek(0, SeekOrigin.Begin);
                            }*/
                        }                        
                    }                    
                }                
            }
        }

        /// <summary>
        /// Returns the request content (body) as byte[] or null if there is no content.
        /// </summary>
        public byte[] ContentBytes { get; } = new byte[0];

        /// <summary>
        /// Returns the parsed content length request header.
        /// </summary>
        public int ContentLength { get; } = 0;

        /// <summary>
        /// Returns the request content (body) stream or null if there is no content stream.
        /// </summary>
        public Stream ContentStream { get; }

        /// <summary>
        /// Returns the request content (body) as string or null if there is no content.
        /// </summary>
        public string ContentString { get; } = string.Empty;

        /// <summary>
        /// Returns the parsed content type request header. Never returns null.
        /// </summary>
        public string ContentType { get; } = string.Empty;

        /// <summary>
        /// Returns the number of header or 0, if no header where found.
        /// </summary>
        public int HeaderCount { get; } = 0;

        /// <summary>
        /// Returns the request header. Never returns null. All keys must be lower case.
        /// </summary>
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns true if the request is valid. A request is valid, if method and url could be parsed. A header is not necessary.
        /// </summary>
        public bool IsValid { get; } = false;

        /// <summary>
        /// Returns the request method in UPPERCASE. get -> GET.
        /// </summary>
        public string Method { get; } = string.Empty;

        /// <summary>
        /// Returns a URL object of the request. Never returns null.
        /// </summary>
        public IUrl Url { get; } = new Url();

        /// <summary>
        /// Returns the user agent from the request header
        /// </summary>
        public string UserAgent { get; } = string.Empty;
    }
}
