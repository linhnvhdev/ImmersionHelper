using ImmersionHelper.Data;
using Newtonsoft.Json;
using static ImmersionHelper.Data.JMDictData;

namespace ImmersionHelper.Services
{
    public class JMDictDictionary : IMyDictionary
    {

        public const string JMDICT_VOCAB_LIST = "jmdict-eng-3.3.1.json";
        public List<Vocabulary> GetVocabularies()
        {
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            // Deserialize json file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", JMDICT_VOCAB_LIST);
            string jsonString = File.ReadAllText(filePath);
            JMDictData.Root jmdict = JsonConvert.DeserializeObject<JMDictData.Root>(jsonString);
            foreach (var word in jmdict.words)
            {
                List<Kanji> kanjis = word.kanji;
                List<Kana> kanas = word.kana;
                List<Sense> senses = word.sense;

                vocabularies.Add(new Vocabulary
                {
                    Word = kanjis.Any() ? kanjis.FirstOrDefault().text : kanas.FirstOrDefault().text,
                    Meaning = JsonConvert.SerializeObject(senses),
                    Pronunciation = kanas.FirstOrDefault().text,
                    Readings = string.Join(",", kanjis.Select(v => v.text)) + ","
                              + string.Join(",", kanas.Select(v => v.text)),
                    Category = VocabularyCategory.Other,
                    Source = "JMDict"
                });
            }
            return vocabularies;
        }
    }
}
