using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    public class PageSource
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string PageLink { get; set; }
        [StringLength(200)]
        public string MainPageUrl { get; set; }

        [StringLength(200)]
        public string PageTitle { get; set; }

        [StringLength(200)]
        public string MainSectionPath { get; set; }

        [StringLength(200)]
        public string ArticleContentPath { get; set; }

        public bool IsImageLinkAbsolute { get; set; }

        public bool IsLinkAbsolute { get; set; }

        public ICollection<Article> Articles { get; set;}
    }
}
