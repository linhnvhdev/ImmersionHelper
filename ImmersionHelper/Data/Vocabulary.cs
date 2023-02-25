namespace ImmersionHelper.Data
{
    public class Vocabulary
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Pronunciation { get; set; }

        public string Readings { get; set; }

        public string Meaning { get; set; }
        public VocabularyCategory Category { get; set; }
        public ICollection<UserVocabulary> UserVocabularies { get; set; }
    }
}
