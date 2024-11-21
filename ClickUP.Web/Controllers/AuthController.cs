using ClickUp.Data.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClickUP.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> CodeExchangeToken([FromQuery] string code)
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
            using var _httpClient = new HttpClient();
            var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequestBody));

            if (!tokenResponse.IsSuccessStatusCode)
                return BadRequest("Error exchanging code for tokens");

            var tokenResponseData = JsonConvert.DeserializeObject<GoogleTokenResponse>(await tokenResponse.Content.ReadAsStringAsync());

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseData?.AccessToken);

            var userInfoResponse = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            if (!userInfoResponse.IsSuccessStatusCode)
                return BadRequest("Error retrieving user information");

            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();

            var data = System.Text.Json.JsonSerializer.Deserialize<GoogleUserInfo>(userInfo);
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user == null)
            {
                var appUser = new ApplicationUser
                {
                    Email = data.Email,
                    UserName = data.GivenName + data.FamilyName,
                    ProfilePictureUrl = data.Picture,
                    FirstName = data.GivenName,
                    LastName = data.FamilyName,
                };

                var result = await _userManager.CreateAsync(appUser);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                    return BadRequest("Error creating user");
                }
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return Redirect($"https://www.youtube.com/");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignUpModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                var appUser = new ApplicationUser
                {

                    Email = input.Email,
                    UserName = input.FirstName + input.LastName,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                };

                var result = await _userManager.CreateAsync(appUser, input.Password);
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                    return Ok(new { message = result.Errors.ToString() });
                }
                else
                {
                    return Ok("done");
                }
            }
            else
            {
                return BadRequest("Email repeated");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogInModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is not null)
            {
                var checkPass = await _userManager.CheckPasswordAsync(user, input.Password);
                if (checkPass)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, input.Password, true, true);
                    if (result.Succeeded)
                        return Ok(new { message = "Login successful" });
                }
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok("Signed out successfully");
        }
    }
}
