using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                // Значения списка атрибута сущности
                {
                    var request = new RetrieveAttributeRequest
                    {
                        EntityLogicalName = "contact",
                        LogicalName = "paymenttermscode"
                    };
                    var response = (RetrieveAttributeResponse)service.Execute(request);
                    var attributeMetadata = (PicklistAttributeMetadata)response.AttributeMetadata;
                    Console.WriteLine($"Entity {request.EntityLogicalName}, picklist attribute {request.LogicalName}:");
                    foreach (var option in attributeMetadata.OptionSet.Options)
                        Console.WriteLine($"{option.Value} - {option.Label.UserLocalizedLabel.Label}");
                }

                // Значения глобального списка
                {
                    var request = new RetrieveOptionSetRequest
                    {
                        Name = "budgetstatus"
                    };
                    var response = (RetrieveOptionSetResponse)service.Execute(request);
                    var attributeMetadata = (OptionSetMetadata)response.OptionSetMetadata;
                    Console.WriteLine($"Global picklist {request.Name}:");
                    foreach (var option in attributeMetadata.Options)
                        Console.WriteLine($"{option.Value} - {option.Label.UserLocalizedLabel.Label}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
