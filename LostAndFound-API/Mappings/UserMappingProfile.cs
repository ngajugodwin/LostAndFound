using AutoMapper;
using LostAndFound_API.Domain.Models.Identity;
using LostAndFound_API.Resources.User;

namespace LostAndFound_API.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResource>();
               //.ForMember(dest => dest.UserRoles, opt =>
               //{
               //    opt.MapFrom(src => src.UserRoles.Select(c => c.Role.Name));
               //});

            CreateMap<SaveUserResource, User>();

            CreateMap<UpdateUserResource, User>();
        }
    }
}
