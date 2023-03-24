using ImmersionHelper.Data;
using Microsoft.EntityFrameworkCore;

namespace ImmersionHelper.Services
{
    public class PostServices
    {
        private ApplicationDbContext _dbContext;

        public PostServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPost(Post post)
        {
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetPost(int id)
        {
            var post = await _dbContext.Posts.Include(x => x.Creator).FirstOrDefaultAsync(m => m.Id == id);
            return post;
        }

        public async Task<List<Post>> GetAnswerPost(int id)
        {
            var AnswerPosts = await _dbContext.Posts.Where(x => x.ReferencePostId == id).Include(x => x.Creator).ToListAsync();
            return AnswerPosts;
        }

        public async Task<List<Post>> GetUnanswerQuestion()
        {
            var listAnswered = await _dbContext.Posts.Where(x => x.ReferencePostId != null).Select(x => x.ReferencePostId).Distinct().ToListAsync();
            var questions = await _dbContext.Posts.Where(x => x.Type == PostType.Question 
                                                            && !listAnswered.Any(la => la == x.Id))
                                                    .Include(x => x.Creator).ToListAsync();
            return questions;
        }

        public async Task<List<Post>> GetUserArticleQuestionAsync(string userId,int articleId)
        {
            var posts = await _dbContext.Posts.Where(x => x.RelatedToArticleId == articleId && x.CreatorId == userId && x.Type == PostType.Question)
                                               .Include(x => x.Creator)
                                               .ToListAsync();
            return posts;
        }
    }
}
