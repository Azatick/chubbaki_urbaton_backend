using System.Linq;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using GarbageCollector.ViewModels;

namespace GarbageCollector.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GarbageAppUserDbo, GarbageAppUser>()
                .IncludeAllDerived()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            CreateMap<LocationDbo, Location>()
                .IncludeAllDerived()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            CreateMap<TrashCanDbo, TrashCan>()
                .IncludeAllDerived()
                .ForMember(tc => tc.WasteCategories,
                    opt => opt.MapFrom(dbo => dbo.LinksToCategories.Select(x => x.Category).ToHashSet()))
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            CreateMap<WasteCategoryDbo, WasteCategory>()
                .IncludeAllDerived()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
                
            CreateMap<WasteTakePointDbo, WasteTakePoint>()
                .IncludeAllDerived()
                .ForMember(tc => tc.AcceptingCategories,
                    opt => opt.MapFrom(dbo => dbo.LinksToCategories.Select(x => x.Category).ToHashSet()))
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            CreateMap<LocationDbo, LocationViewModel>()
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Coordinates.X))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Coordinates.Y));

            CreateMap<WasteTakePointDbo, WasteTakePointViewModel>()
                .ForMember(x => x.Location, x => x.MapFrom(s => s.Location))
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Name));
        }
    }
}