using AutoMapper;
using LostAndFound_API.Domain.Models;
using LostAndFound_API.Extensions;
using LostAndFound_API.Resources.Item;

namespace LostAndFound_API.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemResource>()
                .ForMember(dest => dest.Contact, opt =>
                {
                    opt.MapFrom(src => src.GetPreferredContactInformation());
                });

            CreateMap<ItemResource, Item>();

            CreateMap<SaveItemResource, Item>();

            CreateMap<UpdateItemResource, Item>();
        }
    }
}
