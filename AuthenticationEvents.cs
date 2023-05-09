using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System.Security.Claims;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace vk_test_api;

public class AuthenticationEvents : BasicAuthenticationEvents
{
    public override Task ValidatePrincipalAsync(ValidatePrincipalContext context)
    {
        var credentials = JObject.Parse(File.ReadAllText(@".\appsettings.json")).GetValue("BasicCredentials");
        var login = credentials.GetValue<string>("login");
        var password = credentials.GetValue<string>("password");

        if ((context.UserName == login) && (context.Password == password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            context.Principal = principal;
        }
        else context.AuthenticationFailMessage = "Authentication failed.";

        return Task.CompletedTask;
    }
}
