using Microsoft.AspNetCore.Mvc;

namespace ClickUp.Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> HandleGoogleLogin(string code);
        Task<string> Register(SignUpModel input);
        Task<string> Login(LogInModel input);
        Task<string> LogOut();


    }
}
