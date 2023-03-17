using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImmersionHelper.Pages.Review
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

        public int CountNew { get; set; }

        public int CountReview { get; set; }

        public async Task OnGet()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            CountNew = await _dictionaryServices.CountWordNew(userId);
            CountReview = await _dictionaryServices.CountWordReview(userId);
        }
    }
}
