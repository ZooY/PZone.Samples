using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;


namespace Discovery_Service_Connection
{
    class Program
    {
        static void Main(string[] args)
        {
            var discoverServiceUrl = "http://host/XRMServices/2011/Discovery.svc";


            // On-premises, User credentials
            // Для идентификации пользователя используется учетная запись в AD.
            {
                var userName = "myusername";
                var userPass = "mypassword";
                var userDomain = "mydomain";

                var credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new NetworkCredential(userName, userPass, userDomain);

                using (var service = new DiscoveryServiceProxy(new Uri(discoverServiceUrl), null, credentials, null))
                {
                    ShowOrganizations(service);
                }
            }


            // On-premises, Integrated security
            // Для идентификации пользователя используется его текущая учетная запись.
            {
                var credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

                using (var service = new DiscoveryServiceProxy(new Uri(discoverServiceUrl), null, credentials, null))
                {
                    ShowOrganizations(service);
                }
            }


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }


        /// <summary>
        /// Метод получает список организаций на сервере CRM.
        /// </summary>
        /// <param name="service">Ссылка на сервис обнаружения CRM.</param>
        private static void ShowOrganizations(IDiscoveryService service)
        {
            var request = new RetrieveOrganizationsRequest
            {
                AccessType = EndpointAccessType.Default,
                Release = OrganizationRelease.Current
            };
            var response = (RetrieveOrganizationsResponse)service.Execute(request);
            foreach (var organization in response.Details)
                Console.WriteLine($"{organization.UniqueName}\t{organization.OrganizationVersion}\t{organization.FriendlyName}");
        }
    }
}