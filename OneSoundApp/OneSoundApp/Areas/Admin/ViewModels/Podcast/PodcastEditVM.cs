using OneSoundApp.Models;
using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Podcast
{
    public class PodcastEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<IFormFile> newImages { get; set; }
        public ICollection<PodcastImage> Images { get; set; }
        public int  AuthorId { get; set; }
    }
}
