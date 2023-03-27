using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ImmersionHelper.Pages.Vocabularies
{
    [Authorize]
    public class ExportModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public ExportModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var vocabList = _dictionaryServices.GetUserVocabulariesList(userId);
            StringBuilder builder = new StringBuilder();
            foreach(var item in vocabList)
            {
                var vocab = await _dictionaryServices.GetVocabulary(item.VocabularyId);
                builder.AppendLine($"{vocab.Word}\t{vocab.Meaning.Replace(System.Environment.NewLine, " ")}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/plain","VocabularyList.txt");
        }
    }
}
