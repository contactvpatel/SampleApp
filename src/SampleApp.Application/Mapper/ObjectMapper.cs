using AutoMapper;
using SampleApp.Application.Models;

namespace SampleApp.Application.Mapper
{
    // The best implementation of AutoMapper for class libraries -> https://www.abhith.net/blog/using-automapper-in-a-net-core-class-library/
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod != null && (p.GetMethod.IsPublic || p.GetMethod.IsAssembly);
                cfg.AddProfile<SampleAppDtoMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class SampleAppDtoMapper : Profile
    {
        public SampleAppDtoMapper()
        {
            CreateMap<TaskModel, Domain.Entities.Task>().ReverseMap();
            CreateMap<TargetProductModel, Domain.Models.TargetProductModel>().ReverseMap();
            CreateMap<AmazonProductModel, Domain.Models.AmazonProductModel>().ReverseMap();
        }
    }
}
