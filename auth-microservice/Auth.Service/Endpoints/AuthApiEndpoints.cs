using Auth.Service.ApiModels;
using Auth.Service.Services;

namespace Auth.Service.Endpoints;

public static class AuthApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("login", async Task<IResult> (ITokenService tokenService, LoginRequest loginRequest) =>
        {
            var loginRsult = await tokenService.GenerateAuthenticationToken(loginRequest.Username, loginRequest.Password);

            return loginRequest is null ? TypedResults.Unauthorized() : TypedResults.Ok(loginRsult);
        });
    }
}
