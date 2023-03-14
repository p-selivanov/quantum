using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomerGateway.Infrastructure;

/// <summary>
/// Dummy auth handler that checks Authorization header of the following token: Token#{customerId}
/// </summary>
public class FakeTokenAuthHandler : AuthenticationHandler<FakeTokenAuthOptions>
{
    public FakeTokenAuthHandler(
        IOptionsMonitor<FakeTokenAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header is empty"));
        }

        var userId = ParseUserId(token);
        if (string.IsNullOrEmpty(userId))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization token is invalid"));
        }

        var identity = new ClaimsIdentity("FakeToken", userId, null);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        Context.GetRouteData().Values.Add("userId", userId);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private static string ParseUserId(string token)
    {
        if (token.StartsWith("Token#") == false)
        {
            return null;
        }

        var userId = token.Substring(6);

        return userId;
    }
}
