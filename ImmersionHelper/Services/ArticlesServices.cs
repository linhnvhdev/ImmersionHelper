using HtmlAgilityPack;
using ImmersionHelper.Data;
using ImmersionHelper.Data.Migrations;
using System.Security.Policy;
using System.Text;

namespace ImmersionHelper.Services
{
    public class ArticlesServices
    {
        private ApplicationDbContext _dbContext;
        private HtmlWeb Web { get; set; } = new HtmlWeb
        {
            OverrideEncoding = Encoding.UTF8,
        };

        public ArticlesServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitData()
        {
            var pageSources = _dbContext.PageSources.ToList();
            if (!_dbContext.Articles.Any())
            {
                var list = GetArticles(pageSources);
                if(list != null)
                {
                    _dbContext.AddRange(list);
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<Article> GetArticles(List<PageSource> pageSources)
        {
            List<Article> list = new List<Article>();
            foreach(var ps in pageSources)
            {
                list.AddRange(GetArticles(ps));
            }
            return list;
        }

        public List<Article> GetArticles()
        {
            return _dbContext.Articles.ToList();
        }

        public List<Article> GetArticles(PageSource pageSource)
        {
            List<Article> list = new List<Article>();
            try
            {
                var htmlDoc = Web.Load(pageSource.PageLink);
                var mainElement = htmlDoc.DocumentNode.SelectSingleNode(pageSource.MainSectionPath);
                var linksNode = mainElement.SelectNodes(".//a[@href]");
                if (linksNode == null) return list;
                var links = linksNode.Where(a => !a.GetAttributeValue("class", "").Contains("i-word"))
                                     .Select(a => (pageSource.IsLinkAbsolute ? "" : pageSource.MainPageUrl) + a.GetAttributeValue("href", null))
                                     .Where(a => Uri.IsWellFormedUriString(a,UriKind.Absolute))
                                     .Distinct();
                foreach (var link in links)
                {
                    var doc = Web.Load(link);
                    var pageTitle = doc.DocumentNode.SelectSingleNode("//head/title").InnerText;
                    var mainContentNode = doc.DocumentNode
                                         .SelectSingleNode(pageSource.ArticleContentPath);
                    if (mainContentNode == null) continue;
                    if (!pageSource.IsImageLinkAbsolute)
                    {
                        var images = mainContentNode.Descendants("img");
                        foreach (var img in images)
                        {
                            var src = img.GetAttributeValue("src", null);
                            img.SetAttributeValue("src", pageSource.MainPageUrl + src);
                        }
                    }
                    list.Add(new Article
                    {
                        Content = mainContentNode.InnerText,
                        Link = link,
                        PageSource = pageSource,
                        Title = pageTitle
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return list;
        }
    }
}
