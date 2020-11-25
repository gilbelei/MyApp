
namespace MyApp.API.ViewModels
{
    public class WebScrapingResultModel
    {
        public string FileExtension { get; set; }

        public long NumberOfLines { get; set; }

        public long Size { get; set; }

        public string Result { get; set; }
    }
}