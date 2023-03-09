using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ImmersionHelper.Data
{
    [Index(nameof(Word),nameof(Pronunciation))]
    public class Vocabulary
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Word { get; set; }
        [StringLength(100)]
        public string Pronunciation { get; set; }
        [StringLength(300)]
        public string Readings { get; set; }

        [StringLength(5000)]
        public string Meaning { get; set; }

        [StringLength(20)]
        public string Source { get; set; }
        public VocabularyCategory Category { get; set; }
        public ICollection<UserVocabulary> UserVocabularies { get; set; }
    }
}
