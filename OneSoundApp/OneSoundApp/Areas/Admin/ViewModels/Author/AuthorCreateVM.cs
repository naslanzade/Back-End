using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Author
{
    public class AuthorCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
