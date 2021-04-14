// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Weather.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new [] { "role" }),
                new IdentityResource(
                    "country",
                    "Your country",
                    new [] { "country" }),
                new IdentityResource(
                    "subscription",
                    "Your subscription",
                    new [] { "subscriptionlevel" }),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("weatherapi", "Weather API Scope", new [] { "role" }),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            { new Client
                {
                    AccessTokenLifetime = 120,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 120,
                    ClientName = "Weather Client",
                    ClientId = "0D0B79C4-736A-478A-8249-1CEB8A56F818",
                    ClientSecrets =
                    {
                        new Secret("clientsecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:44347/signin-oidc",
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44347/signout-callback-oidc",
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "weatherapi",
                        "country",
                        "subscriptionlevel",

                    },
                    RequirePkce = true,
                },
                new Client
                {
                    AccessTokenLifetime = 120,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 120,
                    ClientName = "Weather API",
                    ClientId = "78752A3E-4FE3-4694-A9BD-F439F22B10B8",
                    ClientSecrets =
                    {
                        new Secret("apisecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:44314/signin-oidc",
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "weatherapi",
                        "country",
                        "subscriptionlevel",
                    },
                    RequirePkce = true,
                }
            };
    }
}