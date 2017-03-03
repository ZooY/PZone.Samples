using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;


namespace WebResource_Dependencies
{
    class Program
    {
        static void Main()
        {
            var serviceUrl = "http://crmdevapp01/DEV01/XRMServices/2011/Organization.svc";
            var connectionString = "Data Source=crmdevdb01;Initial Catalog=DEV01_MSCRM;Integrated Security=True;Connect Timeout=60";

            var webResourceName = "npf_/sdk/common.crm.js";

            var credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            using (var con = new SqlConnection(connectionString))
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                con.Open();

                // Получаем ID веб-ресурса.
                var webResource = service.RetrieveMultiple(new QueryByAttribute("webresource")
                {
                    ColumnSet = new ColumnSet("webresourceid", "name"),
                    Attributes = { "name" },
                    Values = { webResourceName }
                }).Entities.First();
                var webResourceId = webResource.Id;
                Console.WriteLine("Web Resource");
                Console.WriteLine("\tname: " + webResource.GetAttributeValue<string>("name"));
                Console.WriteLine("\tid: " + webResourceId);

                var dependencies = (RetrieveDependentComponentsResponse)service.Execute(new RetrieveDependentComponentsRequest
                {
                    ComponentType = 61 /* Web Resource https://msdn.microsoft.com/en-us/library/gg328546.aspx#bkmk_componenttype*/,
                    ObjectId = webResourceId
                });
                foreach (var entity in dependencies.EntityCollection.Entities)
                {
                    Console.WriteLine();
                    var requiredObjectId = entity.GetAttributeValue<Guid>("requiredcomponentobjectid");
                    string prefix;
                    if (requiredObjectId == webResourceId)
                    {
                        prefix = "dependent";
                        Console.WriteLine("~~~ Зависимый компонент ~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                    else
                    {
                        prefix = "required";
                        Console.WriteLine("~~~ Родительский компонент ~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                    var objectId = entity.GetAttributeValue<Guid>($"{prefix}componentobjectid");
                    var objectType = entity.GetAttributeValue<OptionSetValue>($"{prefix}componenttype").Value;
                    Console.WriteLine($"Solution ID = {entity.GetAttributeValue<Guid>($"{prefix}componentbasesolutionid")}");
                    Console.WriteLine($"Parent ID = {entity.GetAttributeValue<Guid>($"{prefix}componentparentid")}");
                    Console.WriteLine($"Type = ({objectType}) {entity.FormattedValues[$"{prefix}componenttype"]}");
                    Console.WriteLine($"Object ID = {objectId}");
                    GetObjectInfo(service, con, objectId, objectType, webResourceName);
                }
            }

            Console.ReadKey();
        }


        private static void GetObjectInfo(IOrganizationService service, SqlConnection con, Guid objectId, int objectType, string webResourceName)
        {
            if (objectType == 60)
                GetFormInfo(service, objectId, webResourceName);
            if (objectType == 50)
                GetRibbonInfo(con, objectId, webResourceName);
        }


        private static void GetRibbonInfo(SqlConnection con, Guid objectId, string webResourceName)
        {
            Console.WriteLine("\tИнформация о Ribbon:");
            using (var cmd= con.CreateCommand())
            {
                cmd.CommandText = $"SELECT [CommandDefinition], [Entity], [Command] FROM [RibbonCommand] WHERE [RibbonCustomizationId]='{objectId}'";
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var xml = rdr["CommandDefinition"].ToString();
                        var doc = XElement.Parse(xml);
                        foreach (var action in doc.Element("Actions").Elements())
                        {
                            if (action.Name != "JavaScriptFunction" || action.Attribute("Library").Value != "$webresource:" + webResourceName)
                                continue;
                            Console.WriteLine($"\tEntity Name = {rdr["Entity"]}, Command = {rdr["Command"]}, Function = {action.Attribute("FunctionName").Value}");
                        }
                    }
                }
            }
        }


        private static void GetFormInfo(IOrganizationService service, Guid objectId, string webResourceName)
        {
            Console.WriteLine("\tИнформация о форме:");
            var form = service.Retrieve("systemform", objectId, new ColumnSet("objecttypecode", "formid", "type", "name", "description", "formxml"));
            Console.WriteLine($"\tEntity Name = {form.GetAttributeValue<string>("objecttypecode")}");
            Console.WriteLine($"\tId = {form.GetAttributeValue<Guid>("formid")}");
            Console.WriteLine($"\tType = ({form.GetAttributeValue<OptionSetValue>("type").Value}) {form.FormattedValues["type"]}");
            Console.WriteLine($"\tName = {form.GetAttributeValue<string>("name")}");
            Console.WriteLine($"\tDescription = {form.GetAttributeValue<string>("description")}");
            var xml = form.GetAttributeValue<string>("formxml");

            Console.WriteLine("\t\tИспользование на форме:");
            var element = XElement.Parse(xml);

            foreach (var el in element.Element("formLibraries").Elements("Library"))
                if (el.Attribute("name")?.Value == webResourceName)
                    Console.WriteLine("\t\t- загружен как библиотека формы");

            foreach (var eventEl in element.Element("events").Elements("event"))
            {
                var eventName = eventEl.Attribute("name").Value;
                foreach (var handler in eventEl.Element("Handlers").Elements("Handler"))
                    if (handler.Attribute("libraryName")?.Value == webResourceName)
                        Console.WriteLine($"\t\t- используется в обработчике события {eventName}, функция {handler.Attribute("functionName").Value}.");
            }
        }
    }
}