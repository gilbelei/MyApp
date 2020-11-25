using AutoMapper;

namespace MyApp.MVC.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            var config = new MapperConfiguration(x =>
            {
                x.AddProfile(new DomainToViewModelMappingProfile());
                x.AddProfile(new ViewModelToDomainMappingProfile());
            });

            var mapper = config.CreateMapper();

        }
    }
}