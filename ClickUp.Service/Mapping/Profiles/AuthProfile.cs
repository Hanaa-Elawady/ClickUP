using AutoMapper;
using ClickUp.Data.Entities.IdentityEntities;

namespace ClickUp.Service.Mapping.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile() :base() 
        {
            CreateMap<GoogleUserInfo , ApplicationUser>()
                .ForMember(x => x.UserName,options=>options.MapFrom(src=>src.Name))
                .ForMember(x => x.FirstName,options=>options.MapFrom(src=>src.GivenName))
                .ForMember(x => x.LastName,options=>options.MapFrom(src=>src.FamilyName))
                .ForMember(x => x.Email,options=>options.MapFrom(src=>src.Email))
                .ForMember(x => x.ProfilePictureUrl,options=>options.MapFrom(src=>src.Picture))
                ;

            CreateMap<SignUpModel, ApplicationUser>()
                .ForMember(x => x.UserName, options => options.MapFrom(src => src.FirstName + src.LastName));
        }

    }
}
