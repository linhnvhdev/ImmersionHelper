namespace ImmersionHelper.Data
{
    public class UserArticle
    {
        public string ApplicationUserId { get; set; }

        public int ArticleId { get; set; }

        public ApplicationUser User { get; set; }
        public Article Article { get; set; }

        public bool IsSaved { get; set; }

        public int IsRightLevel { get; set; }

    }
}
