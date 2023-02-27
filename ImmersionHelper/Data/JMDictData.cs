using Newtonsoft.Json;

namespace ImmersionHelper.Data
{
    public class JMDictData
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Gloss
        {
            [JsonIgnore]
            public string lang { get; set; }
            [JsonIgnore]
            public object gender { get; set; }
            [JsonIgnore]
            public string type { get; set; }
            public string text { get; set; }
        }

        public class Kana
        {
            public bool common { get; set; }
            public string text { get; set; }
            [JsonIgnore]
            public List<object> tags { get; set; }
            [JsonIgnore]
            public List<string> appliesToKanji { get; set; }
        }

        public class Kanji
        {
            public bool common { get; set; }
            public string text { get; set; }
            [JsonIgnore]
            public List<object> tags { get; set; }
        }

        public class Root
        {
            public string version { get; set; }
            public List<string> languages { get; set; }
            public bool commonOnly { get; set; }
            public string dictDate { get; set; }
            public List<Word> words { get; set; }
        }

        public class Sense
        {
            [JsonIgnore]
            public List<string> partOfSpeech { get; set; }
            [JsonIgnore]
            public List<string> appliesToKanji { get; set; }
            [JsonIgnore]
            public List<string> appliesToKana { get; set; }
            [JsonIgnore]
            public List<List<string>> related { get; set; }
            [JsonIgnore]
            public List<object> antonym { get; set; }
            [JsonIgnore]
            public List<object> field { get; set; }
            [JsonIgnore]
            public List<object> dialect { get; set; }
            [JsonIgnore]
            public List<object> misc { get; set; }
            [JsonIgnore]
            public List<object> info { get; set; }
            [JsonIgnore]
            public List<object> languageSource { get; set; }
            public List<Gloss> gloss { get; set; }
        }

        public class Word
        {
            [JsonIgnore]
            public string id { get; set; }
            public List<Kanji> kanji { get; set; }
            public List<Kana> kana { get; set; }
            public List<Sense> sense { get; set; }
        }


    }
}
