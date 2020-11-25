using AutoMapper;
using GetGitHub.Domain.Entities;
using MyApp.API.ViewModels;

namespace MyApp.API.AutoMapper
{
    internal class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<WebScrapingResult, WebScrapingResultModel>();
        }
    }
}