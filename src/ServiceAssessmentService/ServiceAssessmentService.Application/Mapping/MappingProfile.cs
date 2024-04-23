using AutoMapper;
using ServiceAssessmentService.Application.Database.Entities;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonModel, Person>();
            CreateMap<Person, PersonModel>();
            // Define more mappings as needed
        }
    }
}
