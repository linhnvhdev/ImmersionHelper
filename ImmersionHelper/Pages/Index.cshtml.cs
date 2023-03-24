using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace ImmersionHelper.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private UserManager<ApplicationUser> _userManager;
        private ArticlesServices _articlesServices;
        private DictionaryServices _dictionaryServices;
        private PostServices _postServices;

        public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, ArticlesServices articlesServices, DictionaryServices dictionaryServices, PostServices postServices)
        {
            _logger = logger;
            _userManager = userManager;
            _articlesServices = articlesServices;
            _dictionaryServices = dictionaryServices;
            _postServices = postServices;
        }

        public bool IsEmptyVocabulary { get; set; }

        public List<UserArticle> UserArticles { get; set; }

        public int CountNew { get; set; }

        public int CountReview { get; set; }

        public int RecommedPostSize { get; set; } = 5;

        public List<Post> Questions { get; set; }

        public async Task OnGet()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            IsEmptyVocabulary = _dictionaryServices.GetUserVocabulariesCount(userId) == 0;
            CountNew = await _dictionaryServices.CountWordNew(userId);
            CountReview = await _dictionaryServices.CountWordReview(userId);
            var articleList = _articlesServices.GetUserArticlesQuery(userId);
            int totalCount = articleList.Count();

            UserArticles = await articleList.Take(RecommedPostSize)
                                            .Include(x => x.Article)
                                            .ThenInclude(x => x.PageSource).ToListAsync();
            Questions = await _postServices.GetUnanswerQuestion();
        }
    }
}