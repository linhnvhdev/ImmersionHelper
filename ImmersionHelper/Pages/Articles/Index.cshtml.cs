using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using static ImmersionHelper.Pages.Vocabularies.IndexModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ImmersionHelper.Pages.Articles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private ArticlesServices _articlesServices;

        public IndexModel(UserManager<ApplicationUser> userManager, ArticlesServices articlesServices)
        {
            _userManager = userManager;
            _articlesServices = articlesServices;
        }

        public List<UserArticle> UserArticles { get;set; } = default!;

        public int PageSize { get; private set; } = 10;

        public int PageIndex { get; private set; } = 1;
        public int TotalPages { get; private set; }

        public int TotalCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public FilterInput FilterData { get; set; }

        public List<SelectListItem> PageSourceList { get; set; }

        public async Task OnGetAsync(int pageIndex=1)
        {
            PageIndex = pageIndex;
            var userId = _userManager.GetUserId(HttpContext.User);
            if (FilterData.SortBy == null) FilterData.SortBy = "Most Word you know";
            var articleList = _articlesServices.GetUserArticlesQuery(userId,
                                                                    FilterData.Title,
                                                                    FilterData.PageSource,
                                                                    FilterData.SortBy);
            int totalCount = articleList.Count();
            int skip = (PageIndex - 1) * PageSize;

            PageSourceList = new List<SelectListItem>()
            {
                new SelectListItem{Text = "All", Value=""}
            };
            PageSourceList.AddRange(_articlesServices.GetPageSourceName()
                .Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }).ToList());
            

            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            TotalCount = totalCount;

            UserArticles = await articleList.Skip(skip)
                                            .Take(PageSize)
                                            .Include(x => x.Article)
                                            .ThenInclude(x => x.PageSource).ToListAsync();
        }

        public class FilterInput
        {
            public string Title { get; set; }
            public string PageSource { get; set; }

            [Display(Name = "Sort by")]
            public string SortBy { get; set; }
        }
    }
}
