namespace OMS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var acceptor = new Acceptor();
            var http = new HttpServer("http://localhost:8080/", acceptor);
            Task.Run(() => http.Start());
            Task.Delay(1000).Wait();
            acceptor.Start();
        }
    }
}