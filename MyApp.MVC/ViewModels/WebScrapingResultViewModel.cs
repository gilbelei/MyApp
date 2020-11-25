using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApp.MVC.ViewModels
{
    public class WebScrapingResultViewModel
    {
        [Key]
        public int WebScrapingResultId { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [MaxLength(255, ErrorMessage = "Maximum of {0} characters ")]
        [DisplayName("File Extension")]
        public string FileExtension { get; set; }

        [DisplayName("Number Of Lines")]
        public int NumberOfLines { get; set; }

        public int Size { get; set; }

        public string Result { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedAt { get; set; }

    }
}