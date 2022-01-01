using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace ProjectEditor
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<Entities.Models.Task, TaskDto>();
            CreateMap<ProjectForCreationDto, Project>();
            CreateMap<TaskForCreationDto, Entities.Models.Task>();
            CreateMap<ProjectForUpdateDto, Project>().ReverseMap();
            CreateMap<TaskForUpdateDto,Entities.Models.Task>().ReverseMap();
        }
    }
}
