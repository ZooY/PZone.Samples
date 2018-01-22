using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Newtonsoft.Json;


namespace Ping
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ping
            using (var pingSender = new System.Net.NetworkInformation.Ping())
            {
                object response;
                try
                {
                    response = pingSender.Send("crmdevapp02");
                }
                catch (PingException ex)
                {
                    response = ex.InnerException;
                }
                catch (Exception ex)
                {
                    response = ex;
                }

                var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter> { new IpAddressConverter(), new IpEndPointConverter() }
                });
                Console.WriteLine(json);
            }


            // IIS Status
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo("iisreset", "crmdevapp02 /status")
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
                Console.Write(line);
                if (line != null && line.Contains("IISADMIN"))
                {
                    Console.Write(" - IIS запущен");
                }
                Console.WriteLine();
            }


            Console.ReadKey();
        }
    }
}