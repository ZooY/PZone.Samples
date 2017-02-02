using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                #region All Entities
                {
                    // Получение списка всех сущностей, включая атрибуты этих сущностей
                    var request = new RetrieveAllEntitiesRequest
                    {
                        EntityFilters = EntityFilters.Entity | EntityFilters.Attributes
                    };
                    var response = (RetrieveAllEntitiesResponse)service.Execute(request);
                    foreach (var entityMetadata in response.EntityMetadata)
                    {
                        Console.WriteLine(
                            entityMetadata.LogicalName + " - " +
                            (entityMetadata.DisplayName?.UserLocalizedLabel == null ? "" : entityMetadata.DisplayName.UserLocalizedLabel.Label) +
                            " (атрибутов " + entityMetadata.Attributes.Length + ")");
                    }
                }
                #endregion


                #region One Entity
                {
                    // Получаем метаданные сущности "contact", включая данные самой сущности и ее атрибутов
                    var request = new RetrieveEntityRequest
                    {
                        LogicalName = "contact",
                        EntityFilters = EntityFilters.Entity | EntityFilters.Attributes,
                        RetrieveAsIfPublished = false
                    };
                    var response = (RetrieveEntityResponse)service.Execute(request);
                    var contactMetadata = response.EntityMetadata;

                    Console.WriteLine("Contact entity");
                    // Вывод данных о сущности
                    Console.WriteLine($"   Logical Name = {contactMetadata.LogicalName}");
                    Console.WriteLine($"   Display Name = {contactMetadata.DisplayName.UserLocalizedLabel.Label}");
                    Console.WriteLine($"   Primary ID Attribute = {contactMetadata.PrimaryIdAttribute}");
                    Console.WriteLine($"   Primary Name Attribute = {contactMetadata.PrimaryNameAttribute}");
                    Console.WriteLine("Attributes");
                    // Вывод данных об атрибутах сущности
                    foreach (var attributeMetadata in contactMetadata.Attributes.Where(a => a.AttributeType.HasValue && a.DisplayName.UserLocalizedLabel != null))
                    {
                        // вывод данных, общих для всех атрибутов
                        Console.WriteLine($"   {attributeMetadata.DisplayName.UserLocalizedLabel.Label} ({attributeMetadata.LogicalName})");
                        // вывод данных, специфических для разных типов атрибутов
                        switch (attributeMetadata.AttributeType.Value)
                        {
                            case AttributeTypeCode.Picklist:
                                var picklistAttributeMetadata = attributeMetadata as PicklistAttributeMetadata;
                                foreach (var option in picklistAttributeMetadata.OptionSet.Options)
                                {
                                    Console.WriteLine($"      {option.Label.UserLocalizedLabel.Label} = {option.Value}");
                                }
                                break;
                            case AttributeTypeCode.Lookup:
                                var lookupAttributeMetadata = attributeMetadata as LookupAttributeMetadata;
                                foreach (var target in lookupAttributeMetadata.Targets)
                                {
                                    Console.WriteLine($"      {target}");
                                }
                                break;
                        }
                    }
                }
                #endregion
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}