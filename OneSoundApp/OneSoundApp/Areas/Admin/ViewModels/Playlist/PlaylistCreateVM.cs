using OneSoundApp.Models;
using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Playlist
{
    public class PlaylistCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }

        public int CategoryId { get; set; }

    }
}
