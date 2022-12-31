using Microsoft.AspNetCore.Authentication;

namespace TestAllPipelines2.Api.Tests.Helpers
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        // Transfer values from host builder to authentication handler:
        // role names etc...
    }
}
