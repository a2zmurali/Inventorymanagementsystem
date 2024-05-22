using System.ComponentModel.DataAnnotations;

namespace TevaInventorymanagementsystem.Models
{
    public class UploadViewModel
    {

        [Required]
        public IFormFile ExcelFile { get; set; }

    }
}
