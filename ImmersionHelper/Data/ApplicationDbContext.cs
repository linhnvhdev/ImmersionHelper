using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ImmersionHelper.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vocabulary> Vocabularies;
        public DbSet<UserVocabulary> UserVocabularies;
        public DbSet<Article> Articles;
        public DbSet<UserArticle> UserArticles;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserVocabulary>().HasKey(uv => new { uv.VocabularyId, uv.ApplicationUserId });
            modelBuilder.Entity<UserArticle>().HasKey(uv => new { uv.ArticleId, uv.ApplicationUserId });
            modelBuilder.Entity<Vocabulary>()
                        .Property(e => e.Category)
                        .HasConversion(
                            v => v.ToString(),
                            v => (VocabularyCategory)Enum.Parse(typeof(VocabularyCategory),v));
        }
    }
}