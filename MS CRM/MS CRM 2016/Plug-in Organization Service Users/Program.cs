using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;


namespace PZone.Samples
{
public class GetUsers : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

        var result = "";

        //var entity = new Entity("contact");

        var service1 = factory.CreateOrganizationService(null);
        //entity["firstname"] = "null";
        //service1.Create(entity);
        result += $"null = {GetUserInfo(service1)}";

        var service2 = factory.CreateOrganizationService(Guid.Empty);
        //entity["firstname"] = "Empty";
        //service1.Create(entity);
        result += $"Guid.Empty ({Guid.Empty}) = {GetUserInfo(service2)}";

        var service3 = factory.CreateOrganizationService(context.UserId);
        //entity["firstname"] = "UserId";
        //service1.Create(entity);
        result += $"UserId ({context.UserId}) = {GetUserInfo(service3)}";

        var service4 = factory.CreateOrganizationService(context.InitiatingUserId);
        //entity["firstname"] = "InitiatingUserId";
        //service1.Create(entity);
        result += $"InitiatingUserId ({context.InitiatingUserId}) = {GetUserInfo(service4)}";

        throw new InvalidPluginExecutionException(result);
    }

    private static string GetUserInfo(IOrganizationService service)
    {
        var request = new WhoAmIRequest();
        var response = (WhoAmIResponse)service.Execute(request);
        var userId = response.UserId;
        var user = service.Retrieve("systemuser", userId, new ColumnSet("fullname"));
        var data = $"{userId} | {user.GetAttributeValue<string>("fullname")}{Environment.NewLine}";
        return data;
    }
}
}