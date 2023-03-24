using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [StringLength(500)]
        public string Content { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public int? ReferencePostId { get; set; }

        public int? RelatedToArticleId { get; set; }

        public Article RelatedToArticle { get; set; }

        public DateTime PostTime { get; set; }

        public PostType Type { get; set; }

    }
}
