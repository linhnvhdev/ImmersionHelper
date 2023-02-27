using ImmersionHelper.Data;
using System.Text.Json.Nodes;

namespace ImmersionHelper.Services
{
    public class JLPTDictionary : IMyDictionary
    {
        public const string JLPT_VOCAB_LIST = "jlptvocabularylist.json";
        public List<Vocabulary> GetVocabularies()
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
                    Pronunciation = item!["romaji"].ToString(),
                    Readings = item!["furigana"].ToString(),
                    Category = (VocabularyCategory)((int)item!["level"]),
                    Source = "JLPTDict"
                });
            }
            return vocabularies;
        }
    }
}
