﻿using Authenticator.API.Model;
using Authenticator.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace Authenticator.API
{
    public class InitializerData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context.Database.EnsureCreated())
                {

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "alice@asp.net",
                            Nome = "Alice Smith",
                            Telefone = "1234-5678"

                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                        new Claim("name", "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alicexxx"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim("email", "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim("address", "Rua Vergueiro, 456"),
                        new Claim("address_details", "8 andar sala 801"),
                        new Claim("phone", "1234-5678"),
                        new Claim("neighborhood", "Vila Mariana"),
                        new Claim("city", "São Paulo"),
                        new Claim("state", "SP"),
                        new Claim("zip_code", "69118")
                    }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("alice created");
                    }
                    else
                    {
                        Console.WriteLine("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Nome = "Bob Smith",
                            Email = "bob@asp.net",
                            Telefone = "1234-5678"

                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim("email", "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }"),
                        new Claim("location", "somewhere"),
                        new Claim("address", "Rua Vergueiro, 456"),
                        new Claim("address_details", "8 andar sala 801"),
                        new Claim("phone", "1234-5678"),
                        new Claim("neighborhood", "Vila Mariana"),
                        new Claim("city", "São Paulo"),
                        new Claim("state", "SP"),
                        new Claim("zip_code", "69118")
                    }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("bob created");
                    }
                    else
                    {
                        Console.WriteLine("bob already exists");
                    }
                }
            }
        }
    }
}
