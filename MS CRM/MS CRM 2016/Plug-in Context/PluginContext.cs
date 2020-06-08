using System;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;


namespace PZone.Samples
{
    public class PluginContext : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(null); 

            var data = new { 
                message = context.MessageName,
                stage = context.Stage,
                inputParameters = context.InputParameters, 
                outputParameters = context.OutputParameters 
            };

            var json = JsonConvert.SerializeObject(data);

            var entity = new Entity("gpbl_setting") {
                ["gpbl_key"] = "test" + DateTime.Now.ToString("yyyyMMddHHmmssfffffff"),
                ["gpbl_description"] = "test",
                ["gpbl_value"] = json.Length > 2000 ? json.Substring(0, 2000) : json
            };
            service.Create(entity);


            //throw new InvalidPluginExecutionException(json);
        }
    }
}