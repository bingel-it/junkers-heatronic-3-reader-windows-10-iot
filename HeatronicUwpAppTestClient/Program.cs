using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpAppTestClient
{
    class Program
    {

        public int Port { get; set; } = 9096;


        static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
            Console.ReadLine();

            var heatronicRestClient = new HeatronicRestClient();
            heatronicRestClient.UnregisterService();
        }

        private void Start()
        {
            // Open Firewall
            var applicationFullFilename = "HeatronicListener";
            FirewallManager.OpenPort(applicationFullFilename, this.Port, FirewallManager.Protocol.Tcp);

            // Start HttpClient
            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.TryParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            //httpClient.GetAsync("?key=client1&uri=" + HttpUtility.UrlEncode("http://IP:" + Port + "/listenerUrl"));

            Listen();


            var heatronicRestClient = new HeatronicRestClient();
            heatronicRestClient.RegisterService();
        }

        public void Listen()
        {

            HttpListener server = new HttpListener();  // this is the http server
            server.Prefixes.Add("http://*:" + Port + "/");  //we set a listening address here (localhost)

            server.Start();   // and start the server

            Thread t = new Thread(delegate ()
            {
                while (true)
                {
                    HttpListenerContext context = null;
                    HttpListenerResponse response = null;
                    try
                    {
                        context = server.GetContext();
                        response = context.Response;

                        string text;
                        using (var reader = new StreamReader(context.Request.InputStream,
                                                             context.Request.ContentEncoding))
                        {
                            text = reader.ReadToEnd();
                        }

                        Console.WriteLine("Got Request: " + text);
                        //the response tells the server where to send the datas

                        var serializer = new JavaScriptSerializer();
                        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        dynamic obj = serializer.Deserialize(text, typeof(object));

                        var url = context.Request.Url;


                        response.StatusCode = 200;
                        response.StatusDescription = "OK";
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (context != null)
                            context.Response.Close(); // here we close the connection
                    }
                }
            });

            t.IsBackground = true;
            t.Start();



        }


    }
}
