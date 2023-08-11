using OneSoundApp.Models;
using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Album
{
    public class AlbumEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<IFormFile> newImages { get; set; }
        public ICollection<AlbumImage> Images { get; set; }
        public int CategoryId { get; set; }
        public int SingerId { get; set; }
        public decimal Price { get; set; }


    }
}
