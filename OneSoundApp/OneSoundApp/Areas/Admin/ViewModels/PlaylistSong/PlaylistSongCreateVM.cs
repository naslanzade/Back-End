using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.PlaylistSong
{
    public class PlaylistSongCreateVM
    {
        [Required]
        public string Name { get; set; }
        public int PlaylistId { get; set; }
    }
}
