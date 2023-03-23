using HtmlAgilityPack;
using ImmersionHelper.Data;
using ImmersionHelper.Data.Migrations;
using ImmersionHelper.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;

namespace ImmersionHelper.Services
{
    public class ArticlesServices
    {
        private ApplicationDbContext _dbContext;
        private DictionaryServices _dictionaryServices;
        private UserManager<ApplicationUser> _userManager;
        private HtmlWeb Web { get; set; } = new HtmlWeb
        {
            OverrideEncoding = Encoding.UTF8,
        };

        public ArticlesServices(ApplicationDbContext dbContext, DictionaryServices dictionaryServices, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _dictionaryServices = dictionaryServices;
            _userManager = userManager;
        }

        public void InitData()
        {
            if (!_dbContext.PageSources.Any())
            {
                InitPageSources();
            }
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

        private void InitPageSources()
        {
            List<PageSource> list = new List<PageSource>()
            {
                new PageSource
                {
                    PageTitle = "yomiuri",
                    MainPageUrl = @"https://www.yomiuri.co.jp/",
                    // short story of syosetsu
                    PageLink = @"https://www.yomiuri.co.jp/news/",
                    MainSectionPath = ".//div[@class='news-top-latest']",
                    IsLinkAbsolute = true,
                    IsImageLinkAbsolute = false,
                    ArticleContentPath = ".//div[@class='p-main-contents ']"

                },
                new PageSource
                {
                    PageTitle = "nishinippon",
                    MainPageUrl = @"https://www.nishinippon.co.jp/",
                    PageLink = @"https://www.nishinippon.co.jp/theme/easy_japanese/",
                    MainSectionPath = ".//div[@class='c-articleList']",
                    IsLinkAbsolute = false,
                    IsImageLinkAbsolute = false,
                    ArticleContentPath = ".//article[@class='articleDetail-content']"
                },
                new PageSource
                {
                    PageTitle = "syosetu",
                    MainPageUrl = @"https://ncode.syosetu.com/",
                    // short story of syosetsu
                    PageLink = @"https://yomou.syosetu.com/search.php?word=&notword=&genre=&type=t&mintime=&maxtime=&minlen=&maxlen=&min_globalpoint=&max_globalpoint=&minlastup=&maxlastup=&minfirstup=&maxfirstup=&order=hyoka",
                    MainSectionPath = ".//div[@id='main_search']",
                    IsLinkAbsolute = true,
                    IsImageLinkAbsolute = true,
                    ArticleContentPath = ".//div[@id='novel_honbun']"
                },
                new PageSource
                {
                    PageTitle = "nhk news",
                    MainPageUrl = @"https://www3.nhk.or.jp",
                    PageLink = @"https://www3.nhk.or.jp/news/catnew.html",
                    MainSectionPath = ".//main[@id='main']",
                    IsLinkAbsolute = false,
                    IsImageLinkAbsolute = false,
                    ArticleContentPath = ".//section[@class='module--detail-content']"
                }
            };
            _dbContext.AddRange(list);
            _dbContext.SaveChanges();
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

        public List<Article> GetArticles(int limit)
        {
            return _dbContext.Articles.Take(limit).ToList();
        }
        public IQueryable<Article> GetArticlesQuery(string pageTitle, string pageSource)
        {
            return _dbContext.Articles
                            .Include(x => x.PageSource)
                            .Where(x => x.Title.Contains(pageTitle))
                            .Where(x => x.PageSource.PageTitle == pageTitle);
        }

        public IQueryable<UserArticle> GetUserArticlesQuery(string userId, string pageTitle = "", string pageSource = "", string SortBy = "Most Word you know", bool IsIncludeRead = false)
        {
            var list = _dbContext.UserArticles.Where(x => x.ApplicationUserId == userId);
            if (!String.IsNullOrEmpty(pageTitle))
                list = list.Where(x => x.Article.Title.Contains(pageTitle));
            if (!String.IsNullOrEmpty(pageSource))
                list = list.Where(x => x.Article.PageSource.PageTitle == pageSource);
            if (!IsIncludeRead)
            {
                list = list.Where(x => x.IsRead == false);
            }
            switch (SortBy)
            {
                case "Most Word you know":
                    list = list.OrderByDescending(x =>  x.WordKnowCount * 1d / x.Article.CharacterCount * 100);
                    break;
                case "Least Word you know":
                    list = list.OrderBy(x => x.WordKnowCount * 1d / x.Article.CharacterCount * 100);
                    break;
                case "Most Character count":
                    list = list.OrderByDescending(x => x.Article.CharacterCount);
                    break;
                case "Least Character count":
                    list = list.OrderBy(x => x.Article.CharacterCount);
                    break;
                default:
                    break;
            }
            return list;
        }

        public IQueryable<UserArticle> GetFavoriteUserArticlesQuery(string userId, string pageTitle, string pageSource, string SortBy, bool IsIncludeRead = false)
        {
            var list = _dbContext.UserArticles.Where(x => x.ApplicationUserId == userId && x.IsSaved);
            if (!String.IsNullOrEmpty(pageTitle))
                list = list.Where(x => x.Article.Title.Contains(pageTitle));
            if (!String.IsNullOrEmpty(pageSource))
                list = list.Where(x => x.Article.PageSource.PageTitle == pageSource);
            if (!IsIncludeRead)
            {
                list = list.Where(x => x.IsRead == false);
            }
            switch (SortBy)
            {
                case "Most Word you know":
                    list = list.OrderByDescending(x => x.WordKnowCount * 1d / x.Article.CharacterCount * 100);
                    break;
                case "Least Word you know":
                    list = list.OrderBy(x => x.WordKnowCount * 1d / x.Article.CharacterCount * 100);
                    break;
                case "Most Character count":
                    list = list.OrderByDescending(x => x.Article.CharacterCount);
                    break;
                case "Least Character count":
                    list = list.OrderBy(x => x.Article.CharacterCount);
                    break;
                default:
                    break;
            }
            return list;
        }

        public UserArticle GetUserArticle(string userId, int articleId)
        {
            var userArticle = _dbContext.UserArticles.Where(x => x.ApplicationUserId == userId && x.ArticleId == articleId).SingleOrDefault();
            return userArticle;
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
                    string contentPlain = String.Concat(mainContentNode.InnerText.Where(c => !Char.IsWhiteSpace(c)));
                    if (contentPlain.Length == 0) continue;
                    list.Add(new Article
                    {
                        Content = mainContentNode.InnerHtml,
                        Link = link,
                        PageSource = pageSource,
                        Title = pageTitle,
                        CharacterCount = contentPlain.Length,
                        N1WordCount = _dictionaryServices.CountByCategory(contentPlain, "N1"),
                        N2WordCount = _dictionaryServices.CountByCategory(contentPlain, "N2"),
                        N3WordCount = _dictionaryServices.CountByCategory(contentPlain, "N3"),
                        N4WordCount = _dictionaryServices.CountByCategory(contentPlain, "N4"),
                        N5WordCount = _dictionaryServices.CountByCategory(contentPlain, "N5"),
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return list;
        }

        public List<string> GetPageSourceName()
        {
            return _dbContext.PageSources.Select(x => x.PageTitle).ToList();
        }

        public bool IsArticleExist(int? id)
        {
            if (id == null) return false;
            return (_dbContext.Articles.Any(x => x.Id == id.Value));
        }

        public Task<Article> GetArticle(int id)
        {
            return _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SavePost(UserArticle userArticle)
        {
            if (userArticle == null) return false;
            if(_dbContext.UserArticles.SingleOrDefault(x => x.ApplicationUserId == userArticle.ApplicationUserId
                                                        && x.ArticleId == userArticle.ArticleId) == null)
            {
                return false;
            }
            userArticle.IsSaved = !userArticle.IsSaved;
            try
            {
                _dbContext.Attach(userArticle).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;

        }

        public async Task<bool> ReadArticle(UserArticle userArticle)
        {
            if (userArticle == null) return false;
            if (_dbContext.UserArticles.SingleOrDefault(x => x.ApplicationUserId == userArticle.ApplicationUserId
                                                        && x.ArticleId == userArticle.ArticleId) == null)
            {
                return false;
            }
            if (userArticle.IsRead == true)
                return true;
            try
            {
                userArticle.IsRead = true;
                _dbContext.Attach(userArticle).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;

        }

        public async Task AddArticles(string pageSource)
        {
            PageSource ps = _dbContext.PageSources.SingleOrDefault(x => x.PageTitle == pageSource);
            if(ps == null) return;
            var list = GetArticles(ps);
            var userIds = _userManager.Users.Select(x => x.Id).ToList();
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                foreach (var article in list)
                {
                    // exist
                    if (_dbContext.Articles.Any(x => x.Link == article.Link))
                    {
                        continue;
                    }
                    await _dbContext.AddAsync(article);
                    await _dbContext.SaveChangesAsync();
                    foreach (var userId in userIds)
                    {
                        var userArticles = new UserArticle
                        {
                            ArticleId = article.Id,
                            ApplicationUserId = userId,
                            IsRightLevel = 0,
                            IsRead = false,
                            IsSaved = false,
                            WordKnowCount = await _dictionaryServices.CountWordsKnowAsync(article.Content, userId)
                        };
                        _dbContext.Add(userArticles);
                        _dbContext.SaveChanges();
                    }
                }
                transaction.Commit();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
