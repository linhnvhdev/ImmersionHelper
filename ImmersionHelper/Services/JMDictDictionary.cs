using ImmersionHelper.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static ImmersionHelper.Data.JMDictData;

namespace ImmersionHelper.Services
{
    public class JMDictDictionary : IMyDictionary
    {
        private readonly ApplicationDbContext _dbContext;
        public const string JMDICT_VOCAB_LIST = "jmdict-eng-3.3.1.json";
        public const string DICT_NAME = "JMDict";

        public JMDictDictionary(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Vocabulary> InitVocabularies()
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
                    Meaning = SensesToMeaning(senses),
                    Pronunciation = kanas.FirstOrDefault().text,
                    Readings = string.Join(",", kanjis.Select(v => v.text)) + ","
                              + string.Join(",", kanas.Select(v => v.text)),
                    Category = VocabularyCategory.Other,
                    Source = "JMDict"
                });
            }
            return vocabularies;
        }

        public static string SensesToMeaning(List<Sense> senses)
        {
            StringBuilder meaning = new StringBuilder();
            for(int i = 0;i < senses.Count; i++)
            {
                var sense = senses[i];
                meaning.Append(i + ". ");
                meaning.Append(string.Join(",", sense.gloss.Select(x => x.text)));
                meaning.AppendLine();

            }
            string finalMeaning =  meaning.ToString();
            return finalMeaning;
        }
    }
}
