using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpAppTestClient
{
    class HeatronicRestClient
    {
        // Invoke-RestMethod "http://192.168.0.29:91/rest/init/?key=notebook&uri=http://192.168.0.20:9096/" -Method Put
        public void RegisterService()
       {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.0.25:91/rest/init/?key=notebook&uri=http://192.168.0.20:9096/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"user\":\"test\"," +
                //              "\"password\":\"bla\"}";

                //streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        public void UnregisterService()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.0.29:91/rest/init/?key=notebook");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"user\":\"test\"," +
                //              "\"password\":\"bla\"}";

                //streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

    }
}
