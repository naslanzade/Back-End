using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Blog
{
    public class BlogCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int AuthorId { get; set; }      
        public int CategoryId { get; set; }
       
    }
}
