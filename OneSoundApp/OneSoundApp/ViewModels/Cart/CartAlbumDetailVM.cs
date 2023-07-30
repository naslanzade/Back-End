using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Cart
{
    public class CartAlbumDetailVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Album> Albums { get; set; }
    }
}
