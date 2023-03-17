using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    public class Article
    {
        public int Id { get; set; }
        [StringLength(300)]
        public string Title { get; set; }
        public string Content { get; set; }

        [StringLength(300)] 
        public string Link { get; set; }

        public PageSource PageSource { get; set; }

        public ICollection<UserArticle> UserArticles { get; set; }

        public int CharacterCount { get; set; }
        public int N5WordCount { get; set; }
        public int N4WordCount { get; set; }
        public int N3WordCount { get; set; }
        public int N2WordCount { get; set; }
        public int N1WordCount { get; set; }
    }
}
