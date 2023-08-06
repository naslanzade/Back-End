namespace OneSoundApp.Areas.Admin.ViewModels.Blog
{
    public class BlogDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }      
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
    }
}
