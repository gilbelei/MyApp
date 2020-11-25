using AutoMapper;
using GetGitHub.Domain.Entities;
using MyApp.API.ViewModels;

namespace MyApp.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<WebScrapingResultModel, WebScrapingResult>();
        }

    }
}