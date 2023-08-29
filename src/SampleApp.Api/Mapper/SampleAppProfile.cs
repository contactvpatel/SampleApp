using AutoMapper;
using SampleApp.Api.Dto;
using SampleApp.Application.Models;

namespace SampleApp.Api.Mapper
{
    public class SampleAppProfile : Profile
    {
        public SampleAppProfile()
        {
            CreateMap<TaskCreateRequest, TaskModel>();
            CreateMap<TaskUpdateRequest, TaskModel>();
            CreateMap<TaskModel, TaskResponseModel>();
            CreateMap<TargetProductModel, Models.TargetProductModel>();
            CreateMap<AmazonProductModel, Models.AmazonProductModel>();
            CreateMap<MultiApiCallModel, MultiApiCallResponseModel>();
        }
    }
}