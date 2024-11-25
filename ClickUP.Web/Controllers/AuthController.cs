using ClickUp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClickUP.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> CodeExchangeToken([FromQuery] string code)
        {
            var response =  await _authService.HandleGoogleLogin(code);
            return Redirect($"{response}");
        }

        [HttpPost]
        public async Task<string> Register([FromBody] SignUpModel input)
        {
            return await _authService.Register(input);
        }

        [HttpPost]
        public async Task<string> Login([FromBody] LogInModel input)
        {
            return await _authService.Login(input);
        }

        [HttpGet]
        public async Task<string> LogOut()
        {
            return  await _authService.LogOut();
        }
    }
}
