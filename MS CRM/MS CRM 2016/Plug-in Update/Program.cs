using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Description;

namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyFileName = @"C:\PZone.MyPlugin.dll";
            var serviceUrl = "https://crmtest.tpcorp.ru/TEST/XRMServices/2011/Organization.svc";

            var assemblyName = AssemblyName.GetAssemblyName(assemblyFileName);
            var assemblyVersion = $"{assemblyName.Version.Major}.{assemblyName.Version.Minor}";
            var assemblyPublicKeyToken = GetPublicKeyToken(assemblyName);

            var credentials = new ClientCredentials { Windows = { ClientCredential = CredentialCache.DefaultNetworkCredentials } };
            using (var service = new OrganizationServiceProxy(new Uri(serviceUrl), null, credentials, null))
            {
                // Find Plug-in
                var query = new QueryExpression("pluginassembly")
                {
                    ColumnSet = new ColumnSet("name", "publickeytoken", "version", "culture"),
                    Criteria = new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new ConditionExpression("name", ConditionOperator.Equal, assemblyName.Name),
                            new ConditionExpression("publickeytoken", ConditionOperator.Equal, assemblyPublicKeyToken),
                            new ConditionExpression("version", ConditionOperator.BeginsWith, assemblyVersion),
                        }
                    }
                };
                var assemblyEntity = service.RetrieveMultiple(query).Entities.First();

                // Update Plug-in
                var bytes = File.ReadAllBytes(assemblyFileName);
                var entity = new Entity(assemblyEntity.LogicalName, assemblyEntity.Id)
                {
                    ["content"] = Convert.ToBase64String(bytes)
                };
                service.Update(entity);
            }

            Console.ReadKey();
        }


        private static string GetPublicKeyToken(AssemblyName assemblyName)
        {
            var publicKeyTokenString = "";
            var token = assemblyName.GetPublicKeyToken();
            for (var i = 0; i < token.GetLength(0); i++)
                publicKeyTokenString += token[i].ToString("x2");
            return publicKeyTokenString;
        }
    }
}