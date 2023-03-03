using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ImmersionHelper.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<UserVocabulary> UserVocabularies { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<UserArticle> UserArticles { get; set; }
        public DbSet<PageSource> PageSources  { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserVocabulary>().HasKey(uv => new { uv.VocabularyId, uv.ApplicationUserId });
            modelBuilder.Entity<UserArticle>().HasKey(uv => new { uv.ArticleId, uv.ApplicationUserId });
            modelBuilder.Entity<Vocabulary>()
                        .Property(e => e.Category)
                        .HasConversion(
                            v => v.ToString(),
                            v => (VocabularyCategory)Enum.Parse(typeof(VocabularyCategory),v));
            modelBuilder.Entity<PageSource>()
                        .HasData(
                            new PageSource
                            {
                                Id = 1,
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
                                Id = 2,
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
                                Id = 3,
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
                                Id = 4,
                                PageTitle = "nhk news",
                                MainPageUrl = @"https://www3.nhk.or.jp",
                                PageLink = @"https://www3.nhk.or.jp/news/catnew.html",
                                MainSectionPath = ".//main[@id='main']",
                                IsLinkAbsolute = false,
                                IsImageLinkAbsolute = false,
                                ArticleContentPath = ".//section[@class='module--detail-content']"
                            }
                        );
        }
    }
}