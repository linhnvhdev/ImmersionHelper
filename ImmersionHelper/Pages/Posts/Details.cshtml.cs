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

namespace ImmersionHelper.Pages.Posts
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private PostServices _postsServices;

        public DetailsModel(UserManager<ApplicationUser> userManager, PostServices postsServices)
        {
            _userManager = userManager;
            _postsServices = postsServices;
        }

        public Post Post { get; set; } = default!;

        public List<Post> AnswerPosts { get; set; }

        [BindProperty]
        public string Answer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postsServices.GetPost(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
                AnswerPosts = await _postsServices.GetAnswerPost(id.Value);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var post = new Post
            {
                Content = Answer,
                CreatorId = _userManager.GetUserId(HttpContext.User),
                ReferencePostId = id,
                Title = "Answer post id " + id,
                PostTime = DateTime.Now,
                Type = PostType.Answer
            };

            await _postsServices.AddPost(post);

            return RedirectToPage("Details",new {id = id});
        }
    }
}
