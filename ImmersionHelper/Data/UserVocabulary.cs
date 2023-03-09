using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    public class UserVocabulary
    {
        public string ApplicationUserId { get; set; }
        public int VocabularyId { get; set; }

        public Vocabulary Vocabulary { get; set; }
        public ApplicationUser User { get; set; }
        [StringLength(200)]
        public string Front { get; set; }

        [StringLength(500)]
        public string Back { get; set; }


        [StringLength(100)]
        public string Hint { get; set; }

        public int NextReviewInterval { get; set; }

        public DateTime ReviewDate { get; set;}
    }
}
