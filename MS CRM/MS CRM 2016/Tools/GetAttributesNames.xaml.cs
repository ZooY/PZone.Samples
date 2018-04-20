using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace Tools
{
    /// <summary>
    /// Interaction logic for GetAttributesNames.xaml
    /// </summary>
    public partial class GetAttributesNames : Window
    {
        public GetAttributesNames()
        {
            InitializeComponent();

            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                var request = new RetrieveAllEntitiesRequest { EntityFilters = EntityFilters.Entity };
                var response = (RetrieveAllEntitiesResponse)service.Execute(request);
                var entities = response.EntityMetadata.Select(entityMetadata => entityMetadata.LogicalName + " - " + (entityMetadata.DisplayName?.UserLocalizedLabel == null ? "" : entityMetadata.DisplayName.UserLocalizedLabel.Label)).OrderBy(s => s).ToList();
                foreach (var entity in entities.OrderBy(s => s))
                    Entities.Items.Add(entity);
            }
        }


        private void OnEntitySelect(object sender, SelectionChangedEventArgs e)
        {
            EntityName.Text = Entities.SelectedValue?.ToString().Split('-').FirstOrDefault()?.Trim();
        }


        private void OnGo(object sender, RoutedEventArgs e)
        {
            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {

                // Получаем метаданные сущности "contact", включая данные самой сущности и ее атрибутов
                var request = new RetrieveEntityRequest
                {
                    LogicalName = EntityName.Text,
                    EntityFilters = EntityFilters.Attributes,
                    RetrieveAsIfPublished = false
                };
                var response = (RetrieveEntityResponse)service.Execute(request);
                var entityAttributes = response.EntityMetadata.Attributes;

                var attributes = Regex.Split(SourceString.Text, @"\s").Where(s => s.Length > 3).ToList();
                ResultString.Text = "";
                foreach (var attribute in attributes)
                    ResultString.Text += attribute + "\t" + entityAttributes.Where(a => a.LogicalName == attribute).Select(a => a.DisplayName?.UserLocalizedLabel?.Label).FirstOrDefault() + Environment.NewLine;


                //Console.WriteLine("Contact entity");
                //// Вывод данных о сущности
                //Console.WriteLine($"   Logical Name = {contactMetadata.LogicalName}");
                //Console.WriteLine($"   Display Name = {contactMetadata.DisplayName.UserLocalizedLabel.Label}");
                //Console.WriteLine($"   Primary ID Attribute = {contactMetadata.PrimaryIdAttribute}");
                //Console.WriteLine($"   Primary Name Attribute = {contactMetadata.PrimaryNameAttribute}");
                //Console.WriteLine("Attributes");
                //// Вывод данных об атрибутах сущности
                //foreach (var attributeMetadata in contactMetadata.Attributes.Where(a => a.AttributeType.HasValue && a.DisplayName.UserLocalizedLabel != null))
                //{
                //    // вывод данных, общих для всех атрибутов
                //    Console.WriteLine($"   {attributeMetadata.DisplayName.UserLocalizedLabel.Label} ({attributeMetadata.LogicalName})");
                //    // вывод данных, специфических для разных типов атрибутов
                //    switch (attributeMetadata.AttributeType.Value)
                //    {
                //        case AttributeTypeCode.Picklist:
                //            var picklistAttributeMetadata = attributeMetadata as PicklistAttributeMetadata;
                //            foreach (var option in picklistAttributeMetadata.OptionSet.Options)
                //            {
                //                Console.WriteLine($"      {option.Label.UserLocalizedLabel.Label} = {option.Value}");
                //            }
                //            break;
                //        case AttributeTypeCode.Lookup:
                //            var lookupAttributeMetadata = attributeMetadata as LookupAttributeMetadata;
                //            foreach (var target in lookupAttributeMetadata.Targets)
                //            {
                //                Console.WriteLine($"      {target}");
                //            }
                //            break;
                //    }
                //}
            }
        }
    }
}