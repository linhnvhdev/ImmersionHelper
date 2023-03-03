namespace ImmersionHelper.Data
{
    public class PageSource
    {
        public int Id { get; set; }

        public string PageLink { get; set; }
        public string MainPageUrl { get; set; }

        public string PageTitle { get; set; }

        public string MainSectionPath { get; set; }

        public string ArticleContentPath { get; set; }

        public bool IsImageLinkAbsolute { get; set; }

        public bool IsLinkAbsolute { get; set; }

        public ICollection<Article> Articles { get; set;}
    }
}
