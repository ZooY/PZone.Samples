using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace PZone.Samples
{
    public class GetUsers : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

        var result = new StringBuilder();

        result.AppendLine($"Windows Users:");
        result.AppendLine($"   WindowsIdentity = {WindowsIdentity.GetCurrent()?.Name}");
        result.AppendLine($"   HttpContext = {HttpContext.Current?.Request?.LogonUserIdentity?.Name}");

        var entity = new Entity("contact");
        result.AppendLine($"Organization Service Users:");

            IOrganizationService service1 = factory.CreateOrganizationService(null);
        entity["firstname"] = "null";
        service1.Create(entity);
        result.AppendLine($"   null = {GetUserInfo(service1)}");

        var service2 = factory.CreateOrganizationService(Guid.Empty);
        entity["firstname"] = "Empty";
        service2.Create(entity);
        result.AppendLine($"   Guid.Empty ({Guid.Empty}) = {GetUserInfo(service2)}");

        var service3 = factory.CreateOrganizationService(context.UserId);
        entity["firstname"] = "UserId";
        service3.Create(entity);
        result.AppendLine($"   UserId ({context.UserId}) = {GetUserInfo(service3)}");

        var service4 = factory.CreateOrganizationService(context.InitiatingUserId);
        entity["firstname"] = "InitiatingUserId";
        service4.Create(entity);
        result.AppendLine($"   InitiatingUserId ({context.InitiatingUserId}) = {GetUserInfo(service4)}");

        //throw new InvalidPluginExecutionException(result.ToString());
    }

    private static string GetUserInfo(IOrganizationService service)
    {
        var callerIdProperty = service.GetType().GetProperty("CallerId");
        var callerId = (Guid)callerIdProperty.GetValue(service);

        var request = new WhoAmIRequest();
        var response = (WhoAmIResponse)service.Execute(request);
        var userId = response.UserId;
        var user = service.Retrieve("systemuser", userId, new ColumnSet("fullname"));
        var data = $"{userId} | {user.GetAttributeValue<string>("fullname")} | callerId: {callerId}";
        return data;
    }
}
}