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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace ImmersionHelper.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private ArticlesServices _articlesServices;
        private DictionaryServices _dictionaryServices;

        public DetailsModel(UserManager<ApplicationUser> userManager, ArticlesServices articlesServices, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _articlesServices = articlesServices;
            _dictionaryServices = dictionaryServices;
        }

        public Article CurArticle { get; set; }

        [BindProperty]
        public AddFormInput Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!_articlesServices.IsArticleExist(id))
            {
                return NotFound();
            }
            CurArticle = await _articlesServices.GetArticle(id.Value);
            return Page();
        }

        public async Task<JsonResult> OnGetSearchVocabulary(string searchString)
        {
            var vocabList = await _dictionaryServices.FindVocabularies(searchString,1);
            if (vocabList == null)
                return null;
            return new JsonResult(vocabList) { StatusCode = 200 };
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var userVocabulary = new UserVocabulary
            {
                ApplicationUserId = userId,
                VocabularyId = Input.Id,
                Front = Input.Front,
                Back = Input.Back,
                Hint = "None",
                NextReviewInterval = 1,
                ReviewDate = DateTime.Today.AddDays(1)
            };

            if (_dictionaryServices.GetUserVocabulary(userId, Input.Id) == null)
            {
                await _dictionaryServices.AddUserVocabularies(userVocabulary);
            }
            else
            {
                await _dictionaryServices.EditUserVocabulary(userVocabulary);
            }
            return new JsonResult(new {message = "Add successfully" }) { StatusCode= 200 };
        }

        public class AddFormInput
        {
            public int Id { get; set; }
            public string Front { get; set; }
            public string Back { get; set; }
        }
    }
}
