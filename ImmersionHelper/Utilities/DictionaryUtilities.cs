using System.Text.RegularExpressions;

namespace ImmersionHelper.Utilities
{
    public class DictionaryUtilities
    {
        public static int CountCharacterOccurence(string content, string word)
        {
            int wordOccerence = Regex.Matches(content, word).Count;
            return wordOccerence * word.Length;
        }
    }
}
