
using System.Runtime.Serialization;

namespace GetGitHub.Domain.Entities
{
    [DataContract]
    public class WebScrapingResult
    {
        [DataMember]
        public string FileExtension { get; set; }
        [DataMember]
        public long NumberOfLines { get; set; }
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public string Result { get; set; }
    }
}
