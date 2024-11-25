using AutoMapper;
using ClickUp.Data.Entities.IdentityEntities;
using ClickUp.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace ClickUp.Service.Services
{
    public class AuthService : IAuthService
    {
        #region Dependancy Injection 

        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(

             IConfiguration configuration 
            ,UserManager<ApplicationUser> userManager
            ,ILogger<AuthService> logger
            ,SignInManager<ApplicationUser> signInManager
            ,IMapper mapper
            ,ITokenService tokenService

            )
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        #endregion

        #region google Register / Login

        public async Task<string> HandleGoogleLogin(string code)
        {
            var tokenRequestBody = getTokenRequestBody(code);
            var userData = GetUserData(tokenRequestBody);

            var user = await _userManager.FindByEmailAsync(userData.Result.Email);
            string token;
            if(user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                token = _tokenService.GenerateUserToken(user);
            }
            else 
            {
                var userToRegister = _mapper.Map<ApplicationUser>(userData.Result);
                var result = await _userManager.CreateAsync(userToRegister);
                await _signInManager.SignInAsync(userToRegister, isPersistent: false);
                token= _tokenService.GenerateUserToken(userToRegister);
            }
            return $"https://www.youtube.com/?token={token}";
        }
        private Dictionary<string, string> getTokenRequestBody(string code)
        {
            IConfiguration googleAuth = _configuration.GetSection("Authentication:Google");

            var tokenRequestBody = new Dictionary<string, string>
            {
                {"code", code},
                {"client_id", googleAuth["ClientId"]},
                {"client_secret", googleAuth["ClientSecret"]},
                {"redirect_uri", googleAuth["RedirectUri"]},
                {"grant_type", "authorization_code"}
            };

            return tokenRequestBody;
        }
        private async Task<GoogleUserInfo> GetUserData(Dictionary<string, string> tokenRequestBody)
        {
            using var _httpClient = new HttpClient();
            var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequestBody));


            var tokenResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(await tokenResponse.Content.ReadAsStringAsync());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseData?.AccessToken);
            var userInfoResponse = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");

            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<GoogleUserInfo>(userInfo);

            return data;
        }
        #endregion

        #region Normal Register 
        public async Task<string> Register(SignUpModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                var appUser = _mapper.Map<ApplicationUser>(input);
                var result = await _userManager.CreateAsync(appUser, input.Password);
                if (!result.Succeeded)
                {
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                        errors.AppendLine(error.Description);
                    }
                    return errors.ToString();
                }
                else
                {
                    return "Register Succeeded";
                }
            }
            else
            {
                return "Email repeated";
            }
        }
        #endregion

        #region Normal Login
        public async Task<string> Login(LogInModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is not null)
            {
                var checkPass = await _userManager.CheckPasswordAsync(user, input.Password);
                if (checkPass)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, input.Password, true, true);
                    if (result.Succeeded)
                        return _tokenService.GenerateUserToken(user);
                }
            }

            return "Invalid email or password" ;
        }
        #endregion

        #region Logout

        public async Task<string> LogOut()
        {
            await _signInManager.SignOutAsync();
            return "LoggedOut Succeeded";
        }
        #endregion
    }
}
