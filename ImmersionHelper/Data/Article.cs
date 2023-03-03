namespace ImmersionHelper.Data
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Link { get; set; }

        public PageSource PageSource { get; set; }

        public ICollection<UserArticle> UserArticles { get; set; }
    }
}
