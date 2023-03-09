using ImmersionHelper.Data;
using Microsoft.AspNetCore.Mvc;
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

        public List<Vocabulary> InitVocabularies()
        {
            var list = new List<Vocabulary>();
            foreach (var item in _myDictionaries)
            {
                list.AddRange(item.InitVocabularies());
            }
            return list;
        }

        public void InitData()
        {
            if (!_dbContext.Vocabularies.Any())
            {
                List<Vocabulary> vocabularies = InitVocabularies();
                // add to database
                if (vocabularies != null)
                {
                    _dbContext.AddRange(vocabularies);
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<UserVocabulary> GetUserVocabulariesList(string userId)
        {
            var list = _dbContext.UserVocabularies
                        .Where(x => x.ApplicationUserId == userId)
                        .ToList();
            return list;
        }

        public UserVocabulary GetUserVocabulary(string userId,int vocabularyId)
        {
            var userVocabulary = _dbContext.UserVocabularies
                        .SingleOrDefault(x => x.ApplicationUserId == userId && x.VocabularyId == vocabularyId);
            return userVocabulary;
        }

        public async Task<UserVocabulary> GetUserVocabularyAsync(string userId, int vocabularyId)
        {
            var userVocabulary = await _dbContext.UserVocabularies
                                        .SingleOrDefaultAsync(
                                        x => x.ApplicationUserId == userId 
                                        && x.VocabularyId == vocabularyId);
            return userVocabulary;
        }

        public IQueryable<UserVocabulary> GetUserVocabulariesQuery(string userId,string front = "",string back = "",string hint = "",bool dateSort = false)
        {
            front ??= "";
            back ??= "";
            hint ??= "";
            var query = _dbContext.UserVocabularies
                        .Where(x => x.ApplicationUserId == userId)
                        .Where(x => x.Front.Contains(front))
                        .Where(x => x.Back.Contains(back))
                        .Where(x => x.Hint.Contains(hint));
            if (dateSort == true)
                query.OrderBy(x => x.ReviewDate);
            return query;
        }

        public int GetUserVocabulariesCount(string userId)
        {
            int count = _dbContext.UserVocabularies
                        .Where(x => x.ApplicationUserId == userId)
                        .Count();
            return count;
        }

        // input a search string and return word contains it
        public async Task<List<Vocabulary>> FindVocabularies(string searchString, int limit = 5)
        {
            var list = await _dbContext.Vocabularies
                            .Where(x => (x.Word.StartsWith(searchString) || x.Pronunciation.StartsWith(searchString)))
                            .OrderBy(x => x.Word.Length)
                            .Take(limit)
                            .ToListAsync();
            return list;
        }

        public async Task AddUserVocabularies(UserVocabulary userVocab)
        {
            if (userVocab == null) return;
            await _dbContext.AddAsync(userVocab);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditUserVocabulary(UserVocabulary userVocabulary)
        {
            if (userVocabulary == null) return;
            if(userVocabulary.ApplicationUserId == null || userVocabulary.VocabularyId == null) return;
            
            _dbContext.Attach(userVocabulary).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserVocabulary(string userId, int id)
        {
            var userVocab = await GetUserVocabularyAsync(userId, id);
            if (userVocab == null)
                return;
            _dbContext.Remove(userVocab);
            await _dbContext.SaveChangesAsync();
        }
    }
}
