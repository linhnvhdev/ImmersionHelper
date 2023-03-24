using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace ImmersionHelper.Pages.Vocabularies
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public IndexModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }

        public List<UserVocabulary> UserVocabularies { get; set; }

        public int PageSize { get; private set; } = 5;

        public int PageIndex { get; private set; } = 1;
        public int TotalPages { get; private set; }

        public int TotalCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public FilterInput FilterData { get; set; }
        public void OnGet(int pageIndex = 1)
        {
            PageIndex = pageIndex;
            var userId = _userManager.GetUserId(HttpContext.User);
            var vocabList = _dictionaryServices
                            .GetUserVocabulariesQuery(userId,
                                                      FilterData.Front,
                                                      FilterData.Back,
                                                      FilterData.Hint,
                                                      FilterData.IsDateSort);
            int totalCount = vocabList.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            int skip = (PageIndex - 1) * PageSize;
            
            UserVocabularies = vocabList.Skip(skip).Take(PageSize).ToList();
            foreach(UserVocabulary vocabulary in UserVocabularies)
            {
                if(vocabulary.Back.Length >= 50)
                {
                    vocabulary.Back = vocabulary.Back.Substring(0, 50);
                    vocabulary.Back += "...";
                }
            }
            
            TotalCount = totalCount;
        }

        public class FilterInput
        {
            [StringLength(50)]
            public string? Front { get; set; }
            [StringLength(50)]
            public string? Back { get; set; }
            public string? Hint { get; set; }

            [Display(Name = "Sort by date")]
            public bool IsDateSort { get; set; }
        }
    }
}
