using System;
using System.Timers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;


namespace PZone.Samples
{
    /// <summary>
    /// При потере соединения с CRM сервис будет генерировать исключение. После восстанления 
    /// подлючения сервис снова начнет выполнять команды. Дополнительных действий по переподлючению
    /// не требуется, главное чтобы приложение не "упало" при получении исключений. 
    /// </summary>
    class Program
    {
        static object _lock = new object();
        CrmServiceClient _service;

        static void Main(string[] args)
        {
            var connectionString = @"ServiceUri=https://myorg.crmdomain.ru:444/myorg; AuthType=IFD; Domain=crmdomain; UserName=crmdomain\crmuser; Password=mypassword; LoginPrompt=Never; RequireNewInstance=True;";
            CrmServiceClient service = new CrmServiceClient(connectionString);
                                   
            var app = new Program(service);
                                 
            var timer = new Timer(3000);
            timer.Elapsed += app.Do;
            timer.Start();

            Console.ReadKey();
            timer.Stop();
            service.Dispose();
        }


        public Program(CrmServiceClient service)
        {
            _service = service;
        }


        private void Do(object sender, ElapsedEventArgs e)
        {
            try
            {
                var currentUser = (WhoAmIResponse)_service.Execute(new WhoAmIRequest());
                Console.WriteLine($"User ID: {currentUser.UserId}");
                Console.WriteLine($"Organization ID: {currentUser.OrganizationId}");
            }
            catch (Exception ex)
            {
                lock (_lock)
                {
                    Console.WriteLine(ex.GetType());
                    var ee = ex.InnerException;
                    var tab = "  - ";
                    while (ee != null)
                    {
                        Console.WriteLine(tab + ee.GetType());
                        ee = ee.InnerException;
                        tab = "  " + tab;
                    }

                    // При потере соединения с CRM будут возникать следующие ошибки:
                    // System.ServiceModel.CommunicationException
                    //     - System.Net.WebException
                    //         - System.IO.IOException
                    //             - System.Net.Sockets.SocketException
                    // System.ServiceModel.EndpointNotFoundException
                    //     -System.Net.WebException
                    //         - System.Net.Sockets.SocketException
                }
            }
        }
    }
}