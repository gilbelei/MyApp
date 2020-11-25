using AutoMapper;
using MyApp.Domain.Entities;
using MyApp.MVC.ViewModels;

namespace MyApp.MVC.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<WebScrapingResult, WebScrapingResultViewModel>();
        }
    }
}