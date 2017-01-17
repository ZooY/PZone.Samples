using System;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";

            var contactRef = new EntityReference("contact", new Guid("{B409D332-C5B7-E611-80C4-005056A5013C}"));

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                // Создание примечания с текстом и вложением.
                service.Create(new Entity("annotation")
                {
                    Attributes =
                    {
                        ["objectid"] = contactRef,
                        ["subject"] = "Тестовое примечание с текстом и вложением",
                        ["notetext"] = "Содержимое тестового примечания с текстовым содержимым и вложением в виде текстового файла.",
                        ["filename"] = "test.txt",
                        ["mimetype"] = "text/plain",  // список возможных MIME-типов https://ru.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_MIME-%D1%82%D0%B8%D0%BF%D0%BE%D0%B2
                        ["documentbody"] = Convert.ToBase64String(Encoding.UTF8.GetBytes("Содержимое текстового файла."))
                    }
                });

                // Создание примечания только с текстом.
                service.Create(new Entity("annotation")
                {
                    Attributes =
                    {
                        ["objectid"] = contactRef,
                        ["subject"] = "Тестовое примечание с текстом",
                        ["notetext"] = "Содержимое тестового примечания только с текстовым содержимым."
                    }
                });
                
                // Создание примечания только с вложением.
                service.Create(new Entity("annotation")
                {
                    Attributes =
                    {
                        ["objectid"] = contactRef,
                        ["subject"] = "Тестовое примечание с вложением",
                        ["filename"] = "test.txt",
                        ["mimetype"] = "text/plain",
                        ["documentbody"] = Convert.ToBase64String(Encoding.UTF8.GetBytes("Содержимое текстового файла."))
                    }
                });
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}