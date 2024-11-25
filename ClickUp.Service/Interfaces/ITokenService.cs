using ClickUp.Data.Entities.IdentityEntities;

namespace ClickUp.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateUserToken(ApplicationUser user);
    }
}
