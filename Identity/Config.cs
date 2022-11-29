using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using static System.Net.WebRequestMethods;

namespace Identity
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "catalogClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "catalgoAPI.read" },
            },
            new Client
            {
                ClientId = "writtenOffClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "writtenOffAPI.read",  "writtenOffAPI.write"},
            },
            new Client
            {
                ClientId = "webClient",
                ClientName = "Web Client Application",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string> {"https://localhost:7124/signin-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "role",
                    "catalgoAPI.read"
                },
                RequirePkce = true,
                AllowPlainTextPkce = false
            }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalgoAPI.read", "Read Catalog API"),
            new ApiScope("catalgoAPI.write", "Write Catalog API"),
            new ApiScope("recordAPI.read", "Read Record API"),
            new ApiScope("recordAPI.write", "Write Record API"),
            new ApiScope("writtenOffAPI.read", "Read WrittenOff API"),
            new ApiScope("writtenOffAPI.write", "Write WrittenOff API"),
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource
            {
                Name = "catalogAPI",
                DisplayName = "Catalog API",
                Description = "Allow the application to access Catalog API",
                Scopes = new List<string> { "catalgoAPI.read", "catalgoAPI.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            },
            new ApiResource
            {
                Name = "recordAPI",
                DisplayName = "WrittenOff API",
                Description = "Allow the application to access Catalog API",
                Scopes = new List<string> { "recordAPI.read", "recordAPI.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            },
            new ApiResource
            {
                Name = "writtenOffAPI",
                DisplayName = "WrittenOff API",
                Description = "Allow the application to access Catalog API",
                Scopes = new List<string> { "writtenOffAPI.read", "writtenOffAPI.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            },

        };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };
    }
}
