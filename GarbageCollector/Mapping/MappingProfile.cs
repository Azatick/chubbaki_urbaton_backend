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
            #region Dbo -> Model

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

            #endregion

            #region ViewModel <-> Model

            CreateMap<UserViewModel, GarbageAppUser>()
                .IncludeAllDerived()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(model => model.TrashCans, opt => opt.Ignore())
                .ReverseMap();
            
            CreateMap<Location, LocationViewModel>()
                .IncludeAllDerived()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Coordinates.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Coordinates.X))
                .ReverseMap();

            #endregion


            CreateMap<WasteTakePointDbo, WasteTakePointViewModel>()
                .ForMember(x => x.Location, x => x.MapFrom(s => s.Location))
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Name));

            CreateMap<LocationDbo, LocationViewModel>()
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Coordinates.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Coordinates.X));

            CreateMap<WasteTakePointDbo, WasteTakePointViewModel>()
                .ForMember(x => x.Location, x => x.MapFrom(s => s.Location))
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Name));
        }
    }
}