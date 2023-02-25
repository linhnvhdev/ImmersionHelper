namespace ImmersionHelper.Data
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime? PublicationDate { get; set; }

        public ICollection<UserArticle> UserArticles { get; set; }
    }
}
