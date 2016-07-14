using System;
using System.Text;
using Windows.Networking.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BingelIT.MicroWebServerLib
{

    public delegate HttpResponse RequestDelegate(HttpRequest request);

    public struct PathHandler
    {
        public string Path { get; set; }
        public HttpRequestMethod Method { get; set; }
        public RequestDelegate Handler { get; set; }
    }

    public class WebServer
    {
        private static uint bufferSize = 1024;

        public uint Port { get; set; }

        private IList<PathHandler> pathHandlers = new List<PathHandler>();
        private HttpRequestBuilder requestCreator = new HttpRequestBuilder();

        public WebServer(uint port = 80)
        {
            this.Port = port;
        }

        public async void InitAsync()
        {
            await Task.Run( () =>
            {
                while (true)
                {
                    try
                    {
                        StreamSocketListener listener = new StreamSocketListener();
                        listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
                        listener.BindServiceNameAsync(this.Port.ToString()).AsTask();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception: " + ex.Message);
                    }
                }
            });
            
        }


        private async void ProcessRequestAsync(StreamSocket socket)
        {
            HttpRequest httpRequest = null;
            try
            {
                using (IInputStream input = socket.InputStream)
                {
                    StringBuilder requestString = new StringBuilder();
                    byte[] data = new byte[bufferSize];
                    IBuffer buffer = data.AsBuffer();
                    uint dataRead = bufferSize;
                    while (dataRead == bufferSize)
                    {
                        await input.ReadAsync(buffer, bufferSize, InputStreamOptions.Partial);
                        requestString.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                        dataRead = buffer.Length;

                    }

                    httpRequest = requestCreator.ProcessRequest(requestString.ToString());
                    Debug.WriteLine(DateTime.Now.ToString()
                        + " Request " + httpRequest.Method + " " + httpRequest.Url + ".");
                }

            } catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

            if (httpRequest == null)
                return;

            using (IOutputStream output = socket.OutputStream)
            {
                using (System.IO.Stream responseStream = output.AsStreamForWrite())
                {
                    var httpResponse = onRequestReceived(httpRequest);

                    if (httpResponse != null)
                    {
                        byte[] responseData = httpResponse.BuildResponseHead();
                        await responseStream.WriteAsync(responseData, 0, responseData.Length);
                        if (httpResponse.Content != null)
                        {
                            await responseStream.WriteAsync(httpResponse.Content, 0, httpResponse.Content.Length);
                        }
                        await responseStream.FlushAsync();
                    }

                }
            }
        }

        public void AddPathHandler(string path, HttpRequestMethod method, RequestDelegate handler)
        {
            var pathHandler = new PathHandler { Path = path.ToLower(), Method = method, Handler = handler };
            pathHandlers.Add(pathHandler);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private HttpResponse onRequestReceived(HttpRequest request)
        {
            foreach (PathHandler pathHandler in pathHandlers)
            {
                // method match
                if (pathHandler.Method != request.Method
                    && pathHandler.Method != HttpRequestMethod.Any)
                    continue;

                // exact path match
                var path = pathHandler.Path;
                var url = request.Url.ToLower();
                var match = path == url;

                // path pattern match 
                if (!match)
                {
                    var pathLength = path.Length;
                    if (path[pathLength - 1] != '*')
                        continue;

                    path = path.Substring(0, pathLength - 1);
                    match = url.IndexOf(path) == 0;
                    if (!match)
                        continue;
                }
                return pathHandler.Handler(request);
            }
            return null;
        }
    }
}




//string page = "";
//var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
//// acquire file
//var file = await folder.GetFileAsync("index.html");
//var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);
//foreach (var line in readFile)
//{
//    page += line;
//}
//byte[] bodyArray = Encoding.UTF8.GetBytes(page);
//var bodyStream = new MemoryStream(bodyArray);

//var header = "HTTP/1.1 200 OK\r\n" +

//            $"Content-Length: {bodyStream.Length}\r\n" +

//                "Connection: close\r\n\r\n";
//byte[] headerArray = Encoding.UTF8.GetBytes(header);
//await bodyStream.CopyToAsync(responseStream);

