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
using System.Data;

namespace ImmersionHelper.Pages.Posts
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private PostServices _postsServices;
        private ArticlesServices _articlesServices;

        private readonly ImmersionHelper.Data.ApplicationDbContext _context;

        public CreateModel(ImmersionHelper.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, PostServices postsServices, ArticlesServices articlesServices)
        {
            _context = context;
            _userManager = userManager;
            _postsServices = postsServices;
            _articlesServices = articlesServices;
        }

        public IActionResult OnGet()
        {
            Types = Enum.GetValues<PostType>().Cast<int>().Where(x => x != (int) PostType.Answer).Select(x => new SelectListItem
            {
                Text = Enum.GetName(typeof(PostType), x),
                Value = x.ToString()
            }).ToList();
            ArticlesListItem = _context.Articles.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
            ArticlesListItem.Add(new SelectListItem
            {
                Text = "None",
                Value = null,
                Selected = true
            });
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public List<SelectListItem> Types { get; set; }

        public List<SelectListItem> ArticlesListItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Posts == null || Post == null)
            {
                return Page();
            }

            Post.CreatorId = _userManager.GetUserId(HttpContext.User);

            await _postsServices.AddPost(Post);
            return RedirectToPage("./Index");
        }
    }
}
