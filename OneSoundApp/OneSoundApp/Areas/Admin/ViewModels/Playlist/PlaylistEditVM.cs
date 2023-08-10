using OneSoundApp.Models;
using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Playlist
{
    public class PlaylistEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<IFormFile> newImages { get; set; }
        public ICollection<PlaylistImage> Images { get; set; }
        public int CategoryId { get; set; }

    }
}
