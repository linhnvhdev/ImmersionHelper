using ImmersionHelper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Text;
using static System.Net.WebRequestMethods;
using ImmersionHelper.Services;

namespace ImmersionHelper.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class TestPageModel : PageModel
    {
        ApplicationDbContext _dbContext;
        ArticlesServices _articlesServices;
        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public TestPageModel(ApplicationDbContext dbContext
                            , ArticlesServices articlesServices)
        {
            _dbContext= dbContext;
            _articlesServices = articlesServices;
        }

        public void OnGet()
        {
            var list = _articlesServices.GetArticles();
            Input.Links = list.Select(x => new InputModel.WebContent
            {
                Links = x.Link,
                content = x.Title
            }).ToList();
        }

        public void Test2()
        {
            PageSource pageSource = new PageSource
            {
                MainPageUrl = @"https://www.yomiuri.co.jp/",
                // short story of syosetsu
                PageLink = @"https://www.yomiuri.co.jp/news/",
                MainSectionPath = ".//div[@class='news-top-latest']",
                IsLinkAbsolute = true,
                IsImageLinkAbsolute = false,
                ArticleContentPath = ".//div[@class='p-main-contents ']"
            };
            List<Article> articles = _articlesServices.GetArticles(pageSource);
            Input.Links = articles.Take(3).Select(x => new InputModel.WebContent
            {
                Links = x.Link,
                content = x.Content
            }).ToList();
        }

        public void Test1()
        {
            string baseurl = @"https://www3.nhk.or.jp";
            string url = @"https://www3.nhk.or.jp/news/catnew.html";
            var html = @"https://www3.nhk.or.jp/news/catnew.html";

            HtmlWeb web = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8,
            };

            var htmlDoc = web.Load(html);

            var mainElement = htmlDoc.GetElementbyId("main");
            var links = mainElement.SelectNodes(".//a[@href]")
                                    .Where(a => !a.GetAttributeValue("class", "").Contains("i-word"))
                                   .Select(a => baseurl + a.GetAttributeValue("href", null));
            Input.Links = new List<InputModel.WebContent>();
            foreach (var link in links)
            {
                var doc = web.Load(link);
                var mainContentNode = doc.DocumentNode
                                     .SelectSingleNode(".//section[@class='module--detail-content']");
                var images = mainContentNode.Descendants("img");
                foreach (var img in images)
                {
                    var src = img.GetAttributeValue("data-src", null);
                    img.SetAttributeValue("src", baseurl + src);
                }
                var content = mainContentNode.InnerHtml;
                Input.Links.Add(new InputModel.WebContent
                {
                    Links = link,
                    content = content
                });
            }
        }

        public class InputModel
        {
            public List<Vocabulary> Vocabularies { get; set; }

            public JMDictData.Root Root { get; set; }

            public class WebContent
            {
                public string Links { get; set; }
                public string content { get; set; } 
            }
            public List<WebContent> Links { get; set; }
        }
    }
}
