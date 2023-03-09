using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    public class Article
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public string Content { get; set; }

        [StringLength(200)] 
        public string Link { get; set; }

        public PageSource PageSource { get; set; }

        public ICollection<UserArticle> UserArticles { get; set; }
    }
}
