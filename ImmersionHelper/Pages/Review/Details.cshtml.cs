using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImmersionHelper.Pages.Review
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;
        private readonly IConfiguration Configuration;
        
        public DetailsModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices,IConfiguration configuration)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
            Configuration = configuration;
        }

        public string Side { get; set; }

        public UserVocabulary UserVocab { get; set; }

        public int NextVocabulary { get; set; }

        [BindProperty]
        public int VocabularyId { get; set; }

        [BindProperty]
        public string Action { get; set; }

        public async Task<IActionResult> OnGet(string side)
        {
            if (side == null)
            {
                return NotFound();
            }
            Side = side;
            if(side == "Finish")
            {
                return Page();
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            UserVocab = await _dictionaryServices.GetUserVocabularyReviewAsync(userId);
            if(UserVocab == null)
            {
                return Redirect("/Review/Details/Finish");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            UserVocab = await _dictionaryServices.GetUserVocabularyAsync(userId, VocabularyId);
            if(UserVocab == null)
            {
                return Page();
            }
            await _dictionaryServices.ReviewVocabulary(UserVocab, Action);
            return RedirectToPage("/Review/Details",new {Side="Front"});
        }
    }
}
