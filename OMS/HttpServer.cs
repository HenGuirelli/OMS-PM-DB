using Newtonsoft.Json;
using System.Net;

namespace OMS
{
    internal class HttpServer
    {
        private readonly string _url;
        private readonly HttpListener _listener;
        private readonly OMS.Acceptor _acceptor;

        public HttpServer(string url, OMS.Acceptor acceptor)
        {
            _url = url;
            _listener = new HttpListener();
            _acceptor = acceptor;
        }

        public void Start()
        {
            _listener.Prefixes.Add(_url);
            _listener.Start();
            Console.WriteLine($"Server started at {_url}");

            while (true)
            {
                HttpListenerContext context = _listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.HttpMethod == "GET")
                {
                    // Implement your logic here
                    Console.WriteLine($"Received GET request for {request.RawUrl}");
                    string responseString = JsonConvert.SerializeObject(OrderRepository.Orders);
                    response.ContentType = "application/json";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
                else
                {
                    // Return 405 Method Not Allowed for other HTTP methods
                    response.StatusCode = 405;
                    response.StatusDescription = "Method Not Allowed";
                    response.OutputStream.Close();
                }
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine($"Server stopped");
        }
    }
}
