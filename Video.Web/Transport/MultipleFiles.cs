using System.ComponentModel.DataAnnotations;

namespace Video.Web.Transport
{
    public class MultipleFiles
    {
        [Required]
        [Display(Name = "File")]
        public List<IFormFile> FormFiles { get; set; }
    }
}
