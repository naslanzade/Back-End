using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Album
{
    public class AlbumCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
        public int CategoryId { get; set; }
        public int SingerId { get; set; }
        public decimal Price { get; set; }

    }
}
