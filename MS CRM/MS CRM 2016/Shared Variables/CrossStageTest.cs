using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Samples
{
    public class CrossStageTest : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.Stage == 20)
            {
                context.SharedVariables.Add("test_id", Guid.NewGuid());
            }
            else
            {
                var id = context.SharedVariables.Contains("test_id") ? context.SharedVariables["test_id"] : "<no>";
                throw new InvalidPluginExecutionException(id.ToString());
            }
        }
    }
}