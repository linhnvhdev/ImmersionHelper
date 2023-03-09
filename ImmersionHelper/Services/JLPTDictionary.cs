using ImmersionHelper.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace ImmersionHelper.Services
{
    public class JLPTDictionary : IMyDictionary
    {
        private readonly ApplicationDbContext _dbContext;

        public JLPTDictionary(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public const string JLPT_VOCAB_LIST = "jlptvocabularylist.json";
        public const string DICT_NAME = "JLPTDict";
        public List<Vocabulary> InitVocabularies()
        {
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            // Deserialize json file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", JLPT_VOCAB_LIST);
            string jsonString = File.ReadAllText(filePath);
            JsonNode jsonNode = JsonNode.Parse(jsonString);
            var items = jsonNode.AsArray();
            foreach (var item in items)
            {
                vocabularies.Add(new Vocabulary
                {
                    Word = item!["word"].ToString(),
                    Meaning = item!["meaning"].ToString(),
                    Pronunciation = item!["furigana"].ToString(),
                    Readings = item!["furigana"].ToString(),
                    Category = (VocabularyCategory)((int)item!["level"]),
                    Source = "JLPTDict"
                });
            }
            return vocabularies;
        }

        public List<Vocabulary> GetVocabularies(string searchString,int limit = 10)
        {

            // Search by Word directly
            var list = _dbContext.Vocabularies.Where(x => x.Source == DICT_NAME && x.Word.StartsWith(searchString))
                                            .OrderBy(x => x.Word.Length)
                                            .Take(limit)
                                            .ToList();
            if (list.Count > 0)
                return list;
            // Search By readings
            var vocabularies = _dbContext.Vocabularies.Where(x => x.Source == DICT_NAME && x.Pronunciation.StartsWith(searchString))
                                          .OrderBy(x => x.Word.Length)
                                          .Take(limit)
                                          .ToList();
            return vocabularies;
        }
    }
}
