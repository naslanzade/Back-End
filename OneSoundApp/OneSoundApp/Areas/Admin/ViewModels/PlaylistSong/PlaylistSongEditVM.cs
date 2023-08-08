using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.PlaylistSong
{
    public class PlaylistSongEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int PlaylistId { get; set; }
    }
}
