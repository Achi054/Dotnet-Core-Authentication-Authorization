using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
            => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource { Name = "rc_pub_scope", UserClaims = new [] { "rc.pubclaim" } }
            };

        public static IEnumerable<ApiResource> GetApiResources()
            => new[]
            {
                new ApiResource("ApiOne", new[] { "rc.pubclaim" }),
                new ApiResource("ApiTwo")
            };

        public static IEnumerable<Client> GetClients()
            => new[] {
                new Client
                {
                    ClientId = "054",
                    ClientSecrets = new[] { new Secret("sujith_acharya".ToSha256()) },

                    // client-to-client communication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = new[] { "ApiOne" }
                },
                new Client
                {
                    ClientId = "054_mvc",
                    ClientSecrets = new[] { new Secret("sujith_acharya_mvc".ToSha256()) },

                    // user-to-client communication
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = new[] { "https://localhost:44399/signin-oidc" },
                    PostLogoutRedirectUris = new[] { "https://localhost:44399/home/index" },

                    AllowedScopes = new[] {
                        "ApiOne",
                        "ApiTwo",
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        "rc_pub_scope"
                    },

                    AllowOfflineAccess = true,

                    RequireConsent = false,
                },
                new Client
                {
                    ClientId = "054_js",
                    ClientSecrets = new[] { new Secret("sujith_acharya_js".ToSha256()) },

                    // user-identity communication
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = new[] { "https://localhost:44340/home/signin" },
                    PostLogoutRedirectUris = new[] { "https://localhost:44340/home/index" },

                    AllowedCorsOrigins = new[] { "https://localhost:44340" },

                    AllowedScopes = new[] {
                        "ApiOne",
                        OidcConstants.StandardScopes.OpenId,
                        "rc_pub_scope",
                    },

                    RequireConsent = false,

                    AllowAccessTokensViaBrowser = true,
                }
            };
    }
}
