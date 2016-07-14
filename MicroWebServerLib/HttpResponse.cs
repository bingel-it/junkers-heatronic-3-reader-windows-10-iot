using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MicroWebServerLib
{
    public class HttpResponse
    {

        private Encoding encoding = Encoding.UTF8;

        public enum ResponseCode
        {
            OK = 200
        }

        private Dictionary<ResponseCode, String> responseCodeMappings = new Dictionary<ResponseCode, string>()
        {
            {ResponseCode.OK, "OK" }
        };

        public Dictionary<String, String> Header { get; set; } = new Dictionary<string, string>();

        public ResponseCode Code { get; set; }

        public byte[] Content { get; set; }

        public HttpResponse()
        {
        }

        public void SetStringContent(String content)
        {
            this.Content = encoding.GetBytes(content);
        }

        public byte[] BuildResponseHead() {

            StringBuilder responseString = new StringBuilder();

            responseString.Append("HTTP/1.0 ");
            responseString.Append((int)this.Code);
            responseString.Append(" ");
            responseString.Append(responseCodeMappings[this.Code]);
            responseString.Append("\r\n");

            foreach (var header in this.Header)
            {
                responseString.Append(header.Key);
                responseString.Append(": ");
                responseString.Append(header.Value);
                responseString.Append("\r\n");

            }
            responseString.Append("\r\n");

            return encoding.GetBytes(responseString.ToString());
        }

    }
}
