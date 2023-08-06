using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Author
{
    public class AuthorEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
