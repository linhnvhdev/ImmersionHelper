using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace ImmersionHelper.Pages.Vocabularies
{
    public class CreateModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public CreateModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }

        public IActionResult OnGet()
        {
            
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewData["UserId"] = userId;
            //UserVocabulary.ApplicationUserId = userId;
            return Page();
        }

        [BindProperty]
        public UserVocabularyInput UserVocabulary { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userVocabulary = new UserVocabulary
            {
                ApplicationUserId = _userManager.GetUserId(HttpContext.User),
                VocabularyId = UserVocabulary.VocabularyId,
                Front = UserVocabulary.Front,
                Back = UserVocabulary.Back,
                Hint = UserVocabulary.Hint,
                NextReviewInterval = 1,
                ReviewDate = DateTime.Today.AddDays(1)
            };

            await _dictionaryServices.AddUserVocabularies(userVocabulary);
            TempData["SuccessMessage"] = "Add successfully";

            return RedirectToPage("./Create");
        }

        public async Task<JsonResult> OnGetSearchVocabulary(string searchString)
        {
            var vocabList = await _dictionaryServices.FindVocabularies(searchString);
            /*
            var vocabList = new List<Vocabulary> { new Vocabulary { Id=1,Word="xxx"},
                                                    new Vocabulary { Id=2,Word="xxx"}
                                                    };
            */
            return new JsonResult(vocabList){StatusCode = 200};
        }

        public class UserVocabularyInput
        {
            public int VocabularyId { get; set; }
            [StringLength(200)]
            public string Front { get; set; }

            [StringLength(500)]
            public string Back { get; set; }

            [StringLength(100)]
            public string Hint { get; set; }
        }
    }
}
