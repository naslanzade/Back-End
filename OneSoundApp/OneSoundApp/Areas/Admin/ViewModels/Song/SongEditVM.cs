using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Song
{
    public class SongEditVM
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
        [Required]
        public string SongName { get; set; }       
        public int SingerId { get; set; }
        public int AlbumId { get; set; }
    }
}
