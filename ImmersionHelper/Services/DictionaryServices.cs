using ImmersionHelper.Data;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Debugger.Contracts.EditAndContinue;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using static ImmersionHelper.Data.JMDictData;
using static System.Formats.Asn1.AsnWriter;

namespace ImmersionHelper.Services
{
    public class DictionaryServices
    {
        private IEnumerable<IMyDictionary> _myDictionaries;
        private ApplicationDbContext _dbContext;


        public DictionaryServices(IEnumerable<IMyDictionary> myDictionaries, ApplicationDbContext dbContext) 
        {
            _myDictionaries = myDictionaries;
            _dbContext = dbContext;
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

        public void InitData()
        {
            if (!_dbContext.Vocabularies.Any())
            {
                List<Vocabulary> vocabularies = GetAllVocabularies();
                // add to database
                if (vocabularies != null)
                {
                    _dbContext.AddRange(vocabularies);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
