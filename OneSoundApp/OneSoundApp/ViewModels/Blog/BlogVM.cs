using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Blog
{
    public class BlogVM
    {
        public List<Models.Blog> Blogs { get; set; }
        public List<Category> Categories { get; set; }
        public List<Advert> Adverts { get; set; }
        public Paginate<Models.Blog> PaginatedDatas { get; set; }
    }
}
