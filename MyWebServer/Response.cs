using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class Response : IResponse
    {
        /// <summary>
        /// Returns a writable dictionary of the response headers. Never returns null.
        /// </summary>
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
        public int ContentLength { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        public string contentType;
        public string ContentType
        {
            get
            {
                return contentType;
            }

            set
            {
                contentType = value;
            }
        }

        /// <summary>
        /// Gets or sets the content of the response in a bytearray.
        /// </summary>
        public byte[] contentByteArr
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the Server response header. Defaults to "BIF-SWE1-Server".
        /// </summary>
        public string serverHeader = "BIF-SWE1-Server";
        public string ServerHeader
        {
            get
            {
                return serverHeader;
            }

            set
            {
                serverHeader = value;
            }
        }

        /// <summary>
        /// Returns the status code as string. (200 OK)
        /// </summary>
        private string status;
        public string Status { get { return status; } }

        /// <summary>
        /// Gets or sets the current status code.
        /// </summary>
        private int statusCode;
        public int StatusCode
        {
            get
            {
                if (statusCode == 0)
                {
                    throw new InvalidOperationException("Statuscode not sent");
                }
                else
                {
                    return statusCode;
                }
            }

            set
            {
                switch (value)
                {
                    case 500:
                        status = "500 INTERNAL SERVER ERROR";
                        break;
                    case 200:
                        status = "200 OK";
                        break;
                    case 404:
                        status = "404 Not Found";
                        break;
                }
                statusCode = value;
            }
        }

        /// <summary>
        /// Adds or replaces a response header in the headers dictionary.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        public void AddHeader(string header, string value)
        {
            if (Headers.ContainsKey(header))
            {
                Headers[header] = value;
            }
            else
            {
                Headers.Add(header, value);
            }    
        }

        /// <summary>
        /// Sends the response to the network stream.
        /// </summary>
        /// <param name="network"></param>
        public void Send(Stream network)
        {
            if(network.CanWrite)
            {
                StreamWriter sw = new StreamWriter(network);

                sw.WriteLine("HTTP/1.1 " + status);
                sw.WriteLine("Server: " + serverHeader);

                foreach (KeyValuePair<string, string> entry in Headers)
                {
                    sw.WriteLine(entry.Key + ": " + entry.Value);
                }

                sw.WriteLine("");

                if (contentType != null && ContentLength == 0)
                {
                    //error handling einbauen
                    throw new System.ArgumentException("Content type is set, but no content was submitted!", "original");
                }
                else
                {
                    if (contentByteArr != null)
                    {
                        UTF8Encoding encoding = new UTF8Encoding();
                        string temp = encoding.GetString(contentByteArr);
                        sw.Write(temp);
                    }
                }

                sw.Flush();
            }
            
        }

        /// <summary>
        /// Sets the stream as content.
        /// </summary>
        /// <param name="stream"></param>
        public void SetContent(Stream stream)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string temp = stream.ToString();
            contentByteArr = new byte[stream.Length];
            stream.Read(contentByteArr, 0, contentByteArr.Length);
            //ContentType = "stream";
            ContentLength = contentByteArr.Length;

        }

        /// <summary>
        /// Sets a byte[] as content.
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(byte[] content)
        {
            ContentLength = content.Length;
            //ContentType = "byte[]";
            
            contentByteArr = content;
        }

        /// <summary>
        /// Sets a string content. The content will be encoded in UTF-8.
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string content)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            contentByteArr = new byte[encoding.GetByteCount(content)];
            contentByteArr = encoding.GetBytes(content);

            //ContentType = "string";

            ContentLength = contentByteArr.Length;
        }
    }
}
