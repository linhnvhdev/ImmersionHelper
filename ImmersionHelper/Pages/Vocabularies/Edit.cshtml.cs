using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;

namespace ImmersionHelper.Pages.Vocabularies
{
    public class EditModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public EditModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _dictionaryServices= dictionaryServices;
            _userManager = userManager;
        }

        [BindProperty]
        public UserVocabulary UserVocabulary { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(HttpContext.User);

            var uservocabulary = await _dictionaryServices.GetUserVocabularyAsync(userId, id.Value);
            if (uservocabulary == null)
            {
                return NotFound();
            }
            UserVocabulary = uservocabulary;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("UserVocabulary.User");
            ModelState.Remove("UserVocabulary.Vocabulary");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _dictionaryServices.EditUserVocabulary(UserVocabulary);

            return RedirectToPage("./Details",new {id = UserVocabulary.VocabularyId});
        }

    }
}
