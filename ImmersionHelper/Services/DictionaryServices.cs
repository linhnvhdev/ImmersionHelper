using ImmersionHelper.Data;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.VisualStudio.Debugger.Contracts.EditAndContinue;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using static ImmersionHelper.Data.JMDictData;

namespace ImmersionHelper.Services
{
    public class DictionaryServices
    {
        private IEnumerable<IMyDictionary> _myDictionaries;

        public DictionaryServices(IEnumerable<IMyDictionary> myDictionaries) 
        {
            _myDictionaries = myDictionaries;
        }

        public List<Vocabulary> GetAllVocabularies()
        {
            var list = new List<Vocabulary>();
            foreach (var item in _myDictionaries)
            {
                list.AddRange(item.GetVocabularies());
            }
            return list;
        }
    }
}
