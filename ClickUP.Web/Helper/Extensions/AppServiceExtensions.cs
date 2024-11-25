using AspNetCore.Identity.Mongo;
using ClickUp.Data.Contexts;
using ClickUp.Data.Entities.IdentityEntities;
using ClickUp.Repository.Interfaces;
using ClickUp.Repository.Repositories;
using ClickUp.Service.HandleResponse;
using ClickUp.Service.Hubs;
using ClickUp.Service.Interfaces;
using ClickUp.Service.Mapping.Profiles;
using ClickUp.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Store.Service.Services;


namespace ClickUP.Web.Helper.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AuthProfile));

            services.AddSingleton<ClickUpDbContext>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IProjectService,ProjectService>();


            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                identityOptions.User.AllowedUserNameCharacters = null;
                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.SignIn.RequireConfirmedEmail = false;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = configuration.GetConnectionString("IdentityConnection");
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                .Where(model => model.Value?.Errors.Count > 0)
                                .SelectMany(model => model.Value?.Errors)
                                .Select(error => error.ErrorMessage)
                                .ToList();


                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;

        }
    }
}
