using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImmersionHelper.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ImmersionHelper.Pages.Posts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ImmersionHelper.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public IndexModel(ImmersionHelper.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Post> Post { get; set; } = default!;

        public string UserId { get; set; }

        public async Task OnGetAsync()
        {
            UserId = _userManager.GetUserId(HttpContext.User);
            if (_context.Posts != null)
            {
                Post = await _context.Posts
                .Where(x => x.Type != PostType.Answer)
                .Include(p => p.Creator)
                .OrderBy(x => x.PostTime)
                .ToListAsync();
            }
        }
    }
}
