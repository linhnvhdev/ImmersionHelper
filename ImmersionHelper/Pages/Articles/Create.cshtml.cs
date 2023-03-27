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
using Microsoft.AspNetCore.Authorization;

namespace ImmersionHelper.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {

        private readonly ImmersionHelper.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private ArticlesServices _articlesServices;

        public CreateModel(ImmersionHelper.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, ArticlesServices articlesServices)
        {
            _context = context;
            _userManager = userManager;
            _articlesServices = articlesServices;
        }

        public void OnGet()
        {
            PageSourceList = _articlesServices.GetPageSourceName()
                .Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }).ToList();
        }

        [BindProperty]
        public string pageSource { get; set; }

        public List<SelectListItem> PageSourceList { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            await _articlesServices.AddArticles(pageSource);
            return RedirectToPage("./Index");
        }
    }
}
