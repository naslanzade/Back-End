using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }

        [Required]       
        public string? Name { get; set; }
    }
}
