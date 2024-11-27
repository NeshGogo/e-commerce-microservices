using ECommerce.Shared.Authentication;

namespace Auth.Service.Services;

public static class TokenStartupExtensions
{
    public static void RegisterTokenService(this IServiceCollection services, IConfigurationManager configuration)
    {
        AuthOptions authOptions = new();
        configuration.GetSection(AuthOptions.AuthenticationSectionName).Bind(authOptions);
        services.AddSingleton(authOptions);

        services.AddScoped<ITokenService, JwtTokenService>();
    }
}
