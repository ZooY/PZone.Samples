using System;
using System.Diagnostics;
using System.Net;
using System.Security;


namespace CheckAvailability
{
    class Program
    {
        static void Main()
        {
            #region Доступность физического сервера


            {

                var host = "crmdevapp02";
                var available = ServerAvailability(host);
                Console.WriteLine($"Server {host}: {(available ? "OK" : "Not available")}");

            }


            #endregion


            #region Доступность IIS


            {

                var host = "crmdevapp02";
                var available = IisAvailability(host);
                Console.WriteLine($"IIS on server {host}: {(available ? "OK" : "Not available")}");

            }


            #endregion


            #region Доступность веб-сайта


            {
                var site = "http://crmdevapp01/DEV01";
                var available = SiteAvailability(site);
                Console.WriteLine($"Site {site}: {(available ? "OK" : "Not available")}");
            }


            #endregion


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }


        private static bool ServerAvailability(string host)
        {
            bool available;
            using (var pingSender = new System.Net.NetworkInformation.Ping())
            {
                try
                {
                    pingSender.Send(host);
                    available = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    available = false;
                }
            }
            return available;
        }


        private static bool IisAvailability(string host)
        {
            var available = false;

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo("iisreset", $"{host} /status")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                if (line != null && line.Contains("IISADMIN"))
                    available = true;
            }
            return available;
        }


        private static bool SiteAvailability(string uri)
        {
            bool available;
            try
            {
                var request = WebRequest.Create(uri);
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = (HttpWebResponse)request.GetResponse();
                available = response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                available = false;
            }
            return available;
        }
    }
}