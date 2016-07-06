using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.MicroWebServerLib
{
    public class  HttpRequest 
    {
        public enum HttpRequestMethod
        {
            Unknown,
            Get,
            Post,
            Put,
            Delete,
            Options,
            Move,
            Any
        }

        /// <summary>
        /// URL of request
        /// </summary>
        public string Url { get; set; }

        public HttpRequestMethod Method { get; set; }

        /// <summary>
        /// Parameters of the request
        /// </summary>
        public Dictionary<String, String> Arguments { get; set; } = new Dictionary<string, string>();

        public Dictionary<String, String> Header { get; set; } = new Dictionary<string, string>();
    }
}
