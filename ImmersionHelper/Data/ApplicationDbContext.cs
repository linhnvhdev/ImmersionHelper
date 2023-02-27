using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ImmersionHelper.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<UserVocabulary> UserVocabularies { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<UserArticle> UserArticles { get; set; }

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