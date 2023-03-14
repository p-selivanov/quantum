using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Yarp.ReverseProxy.Configuration;

namespace CustomerGateway;

public class ReverseProxyConfig
{
    private readonly IConfiguration _configuration;

    public ReverseProxyConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IReadOnlyList<RouteConfig> GetRoutes() => new List<RouteConfig>
    {
        new RouteConfig
        {
            RouteId = "registration",
            Match = new RouteMatch
            {
                Path = "/api/profile",
                Methods = new List<string> { "POST" },
            },
            AuthorizationPolicy = "anonymous",
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["PathSet"] = "/customers",
                }
            },
            ClusterId = "customer-api",
        },
        new RouteConfig
        {
            RouteId = "profile",
            Match = new RouteMatch
            {
                Path = "/api/profile",
                Methods = new List<string> { "GET", "PATCH" },
            },
            AuthorizationPolicy = "ValidToken",
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["PathPattern"] = "/customers/{userId}",
                }
            },
            ClusterId = "customer-api",
        },
        new RouteConfig
        {
            RouteId = "accounts",
            Match = new RouteMatch
            {
                Path = "/api/accounts/{**remainder}",
                Methods = new List<string> { "GET" },
            },
            AuthorizationPolicy = "ValidToken",
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["PathPattern"] = "/customers/{userId}/accounts/{**remainder}",
                }
            },
            ClusterId = "account-api",
        },
        new RouteConfig
        {
            RouteId = "deposits",
            Match = new RouteMatch
            {
                Path = "/api/deposits",
                Methods = new List<string> { "POST" },
            },
            AuthorizationPolicy = "ValidToken",
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["PathPattern"] = "/customers/{userId}/deposits",
                }
            },
            ClusterId = "account-api",
        },
        new RouteConfig
        {
            RouteId = "withdrawals",
            Match = new RouteMatch
            {
                Path = "/api/withdrawals",
                Methods = new List<string> { "POST" },
            },
            AuthorizationPolicy = "ValidToken",
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["PathPattern"] = "/customers/{userId}/withdrawals",
                }
            },
            ClusterId = "account-api",
        },
    };

    public IReadOnlyList<ClusterConfig> GetClusters() => new List<ClusterConfig>
    {
        new ClusterConfig
        {
            ClusterId = "customer-api",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                ["default"] = new DestinationConfig
                {
                    Address = _configuration.GetValue<string>("CustomerApiUri"),
                },
            },
        },
        new ClusterConfig
        {
            ClusterId = "account-api",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                ["default"] = new DestinationConfig
                {
                    Address = _configuration.GetValue<string>("AccountApiUri"),
                },
            },
        },
    };
}
