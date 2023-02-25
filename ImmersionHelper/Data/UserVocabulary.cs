namespace ImmersionHelper.Data
{
    public class UserVocabulary
    {
        public string ApplicationUserId { get; set; }
        public int VocabularyId { get; set; }

        public Vocabulary Vocabulary { get; set; }
        public ApplicationUser User { get; set; }
        public string Front { get; set; }

        public string Back { get; set; }

        public string Hint { get; set; }

        public int NextReviewInterval { get; set; }

        public DateTime ReviewDate { get; set;}
    }
}
