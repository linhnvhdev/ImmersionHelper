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
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ImmersionHelper.Pages.Posts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ImmersionHelper.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private PostServices _postsServices;
        private ArticlesServices _articlesServices;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, PostServices postsServices, ArticlesServices articlesServices)
        {
            _context = context;
            _userManager = userManager;
            _postsServices = postsServices;
            _articlesServices = articlesServices;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public List<SelectListItem> Types { get; set; }

        public List<SelectListItem> ArticlesListItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post =  await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            Post = post;

            Types = Enum.GetValues<PostType>().Cast<int>().Where(x => x != (int)PostType.Answer).Select(x => new SelectListItem
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Post.CreatorId = _userManager.GetUserId(HttpContext.User);
            _context.Attach(Post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
