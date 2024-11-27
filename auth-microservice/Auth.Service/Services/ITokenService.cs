using Auth.Service.Models;

namespace Auth.Service.Services;

public interface ITokenService
{
    Task<AuthToken?> GenerateAuthenticationToken(string userName, string password); 
}
