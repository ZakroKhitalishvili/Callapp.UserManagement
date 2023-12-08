using Callapp.UserManagement.Application.Dtos.Users;
using Callapp.UserManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;

namespace Callapp.UserManagement.Web.Routes;

public static class AuthRoutes
{
    public static IEndpointRouteBuilder MapAuthRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async ([FromBody] UserLoginDto userLoginDto, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            var result = await userService.LoginAsync(userLoginDto, cancellationToken);

            if (result.Successful)
            {
                var claimsPrincipal = new ClaimsPrincipal(
              new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.Email, result.Data!.Email),
                    new Claim(ClaimTypes.Name, result.Data!.Email)
                },
            BearerTokenDefaults.AuthenticationScheme
          )
        );

                return Results.SignIn(claimsPrincipal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties { AllowRefresh = false });
            }

            return Results.BadRequest(result);
        }).AllowAnonymous();

        app.MapPost("/api/auth/register", async ([FromBody] UserCreateDto userCreateDto, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.CreateAsync(userCreateDto, cancellationToken);
        }).AllowAnonymous();

        app.MapPost("/api/auth/logout", (CancellationToken cancellationToken) =>
        {
            return Results.SignOut();
        });

        return app;
    }
}
