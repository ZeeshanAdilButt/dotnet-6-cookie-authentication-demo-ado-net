using CookiesAuthenticationPOC.Controllers;

namespace CookiesAuthenticationPOC.Infrastructure
{
    public interface IAccountInfrastructure
    {
        string ConnectionString { get; }

        Task<bool> IsAuthenticated(User user);
    }
}