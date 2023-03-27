using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImmersionHelper.Pages.Vocabularies
{
    [Authorize]
    public class CreateRangeModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public CreateRangeModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }

        public void OnGet()
        {

        }

        public async Task<JsonResult> OnGetJLPTAsync(string JLPTLevel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            await _dictionaryServices.AddJLPTVocabularies(userId, JLPTLevel);
            return new JsonResult(new { message = "Add successfully" }) { StatusCode = 200 };
        }
    }
}
