using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MicroWebServerLib
{
    public class  HttpRequest 
    {

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
