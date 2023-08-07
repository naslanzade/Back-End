using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Song
{
    public class SongDetailVM
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string Image { get; set; }       
        public string SingerName { get; set; }        
        public string AlbumName { get; set; }
        public string CreatedDate { get; set; }

    }
}
