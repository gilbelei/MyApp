using AutoMapper;
using MyApp.Domain.Entities;
using MyApp.MVC.ViewModels;

namespace MyApp.MVC.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<WebScrapingResultViewModel, WebScrapingResult>();
        }
    }
}