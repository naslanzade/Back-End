using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Podcast
{
    public class PodcastCreateVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }

    }
}
