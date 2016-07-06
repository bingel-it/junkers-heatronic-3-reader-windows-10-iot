using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.MicroWebServerLib
{
    public class HttpResponseBuilder
    {
        public static HttpResponse CreateOKResponse(String content = null, String contentType = "text/html")
        {
            var response = new HttpResponse();
            response.Code = HttpResponse.ResponseCode.OK;

            createContentTypeHeader(response.Header, contentType);
            createHeaderAccessControlAllowOrigin(response.Header, "*");
            if (content != null)
            {
                createHeaderContentLength(response.Header, (uint)content.Length);
                response.SetStringContent(content);
            }
            createHeaderConnection(response.Header, "close");

            return response;
        }

        private static void createContentTypeHeader(Dictionary<String, String> headers, String contentType)
        {
            headers.Add("Content-Type", contentType + "; charset=utf-8");
        }

        private static void createHeaderAccessControlAllowOrigin(Dictionary<String, String> headers, String origin)
        {
            headers.Add("Access-Control-Allow-Origin", origin);
        }

        private static void createHeaderContentLength(Dictionary<String, String> headers, uint length)
        {
            headers.Add("Content-Length", length.ToString());
        }

        private static void createHeaderConnection(Dictionary<String, String> headers, String connection)
        {
            headers.Add("Connection", connection);
        }

    }
}
