using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;

namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // On-premises, Integrated security
            var serviceUrl = "https://crmtest/dev01/XRMServices/2011/Organization.svc";
            var credentials = new ClientCredentials
            {
                Windows = { ClientCredential = CredentialCache.DefaultNetworkCredentials }
            };
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                // Execute request to CRM
                var currentUser = (WhoAmIResponse)service.Execute(new WhoAmIRequest());
                Console.WriteLine($"User ID: {currentUser.UserId}");
                Console.WriteLine($"Organization ID: {currentUser.OrganizationId}");
            }
        }
    }
}