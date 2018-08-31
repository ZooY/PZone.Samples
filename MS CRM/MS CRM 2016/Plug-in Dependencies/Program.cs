using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var serviceUrl = "http://chcrmtestapp01/TEST01/XRMServices/2011/Organization.svc";

            var pluginAssemblyName = "PZone.SomeTools";
            var pluginAssemblyVersion = "1.2.0.0";

            var credentials = new ClientCredentials { Windows = { ClientCredential = CredentialCache.DefaultNetworkCredentials } };
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                // Получаем ID сборки.
                var assembly = service.RetrieveMultiple(new QueryByAttribute("pluginassembly")
                {
                    ColumnSet = new ColumnSet("pluginassemblyid", "name"),
                    Attributes = { "name", "version" },
                    Values = { pluginAssemblyName, pluginAssemblyVersion }
                }).Entities.First();
                Console.WriteLine("Assembly");
                Console.WriteLine("\tname: " + assembly.GetAttributeValue<string>("name"));
                Console.WriteLine("\tid: " + assembly.Id);


                // Получаем плагины.
                var plugins = service.RetrieveMultiple(new QueryByAttribute("plugintype")
                {
                    ColumnSet = new ColumnSet("plugintypeid", "name"),
                    Attributes = { "pluginassemblyid" },
                    Values = { assembly.Id }
                }).Entities;

                foreach (var plugin in plugins)
                {
                    Console.WriteLine("Plug-in");
                    Console.WriteLine("\tname: " + plugin.GetAttributeValue<string>("name"));
                    Console.WriteLine("\tid: " + plugin.Id);

                    var dependencies = (RetrieveDependenciesForDeleteResponse)service.Execute(new RetrieveDependenciesForDeleteRequest
                    {
                        ComponentType = 90 /* Plugin Type https://msdn.microsoft.com/en-us/library/gg328546.aspx#bkmk_componenttype */,
                        ObjectId = plugin.Id
                    });
                    foreach (var entity in dependencies.EntityCollection.Entities)
                    {
                        Console.WriteLine();
                        var requiredObjectId = entity.GetAttributeValue<Guid>("requiredcomponentobjectid");
                        string prefix;
                        if (requiredObjectId == plugin.Id)
                        {
                            prefix = "dependent";
                            Console.WriteLine("\t~~~ Зависимый компонент ~~~~~~~~~~~~~~~~~~~~~~~~~~");
                        }
                        else
                        {
                            prefix = "required";
                            Console.WriteLine("\t~~~ Родительский компонент ~~~~~~~~~~~~~~~~~~~~~~~");
                        }
                        var objectId = entity.GetAttributeValue<Guid>($"{prefix}componentobjectid");
                        var objectType = entity.GetAttributeValue<OptionSetValue>($"{prefix}componenttype").Value;
                        Console.WriteLine($"\tSolution ID = {entity.GetAttributeValue<Guid>($"{prefix}componentbasesolutionid")}");
                        Console.WriteLine($"\tParent ID = {entity.GetAttributeValue<Guid>($"{prefix}componentparentid")}");
                        Console.WriteLine($"\tType = ({objectType}) {entity.FormattedValues[$"{prefix}componenttype"]}");
                        Console.WriteLine($"\tObject ID = {objectId}");
                        GetObjectInfo(service, objectType, objectId);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }


        private static void GetObjectInfo(OrganizationServiceProxy service, int objectType, Guid objectId)
        {
            if (objectType == 29)
                GetWorkflowInfo(service, objectId);
        }


        private static void GetWorkflowInfo(OrganizationServiceProxy service, Guid objectId)
        {
            var workflow = service.Retrieve("workflow", objectId, new ColumnSet("name", "primaryentity"));
            Console.WriteLine($"\tName = {workflow.GetAttributeValue<string>("name")}");
            Console.WriteLine($"\tEntity Name = ({workflow.GetAttributeValue<string>("primaryentity")}) {(workflow.FormattedValues.Contains("primaryentity") ? workflow.FormattedValues["primaryentity"] : "")}");
        }
    }
}