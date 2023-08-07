using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Song
{
    public class SongCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }
        [Required]
        public string SongName { get; set; }       
        public int SingerId { get; set; }
        public int AlbumId { get; set; }
    }
}
