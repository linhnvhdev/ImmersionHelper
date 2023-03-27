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
using Microsoft.AspNetCore.Authorization;

namespace ImmersionHelper.Pages.Vocabularies
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public DeleteModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }

        [BindProperty]
        public UserVocabulary UserVocabulary { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            UserVocabulary = await _dictionaryServices.GetUserVocabularyAsync(userId, id.Value);

            if (UserVocabulary == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(HttpContext.User);

            await _dictionaryServices.DeleteUserVocabulary(userId, id.Value);

            return RedirectToPage("./Index");
        }
    }
}
