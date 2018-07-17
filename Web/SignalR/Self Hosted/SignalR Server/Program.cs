using System;
using Microsoft.Owin.Hosting;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 or http://+:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.

            var host = "http://localhost:8081/";
            using (WebApp.Start<Startup>(host))
            {
                Console.WriteLine($"Server running at {host}");
                Console.ReadLine();
            }
        }
    }
}
