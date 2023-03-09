using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImmersionHelper.Data;

namespace ImmersionHelper.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly ImmersionHelper.Data.ApplicationDbContext _context;

        public DetailsModel(ImmersionHelper.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            else 
            {
                Article = article;
            }
            return Page();
        }
    }
}
