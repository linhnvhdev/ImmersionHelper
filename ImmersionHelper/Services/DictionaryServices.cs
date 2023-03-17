using ImmersionHelper.Data;
using ImmersionHelper.Data.Migrations;
using ImmersionHelper.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Debugger.Contracts.EditAndContinue;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using static ImmersionHelper.Data.JMDictData;
using static System.Formats.Asn1.AsnWriter;

namespace ImmersionHelper.Services
{
    public class DictionaryServices
    {
        private IEnumerable<IMyDictionary> _myDictionaries;
        private ApplicationDbContext _dbContext;
        private readonly IConfiguration Configuration;


        public DictionaryServices(IEnumerable<IMyDictionary> myDictionaries, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _myDictionaries = myDictionaries;
            _dbContext = dbContext;
            Configuration = configuration;
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

        public UserVocabulary GetUserVocabulary(string userId, int vocabularyId)
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

        public IQueryable<UserVocabulary> GetUserVocabulariesQuery(string userId, string front = "", string back = "", string hint = "", bool dateSort = false)
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
                query = query.OrderBy(x => x.ReviewDate);
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
            var uv = await GetUserVocabularyAsync(userVocab.ApplicationUserId, userVocab.VocabularyId);
            if (uv != null) return;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbContext.AddAsync(userVocab);
                await _dbContext.SaveChangesAsync();
                // Check if article have this word to add to number of word in articles known
                var articles = _dbContext.Articles.Select(x => new
                {
                    Id = x.Id,
                    Content = x.Content
                }).ToList();
                string word = (await _dbContext.Vocabularies.FirstAsync(x => x.Id == userVocab.VocabularyId)).Word;
                foreach (var article in articles)
                {
                    var userArticles = await _dbContext.UserArticles.FirstOrDefaultAsync(x => x.ApplicationUserId == userVocab.ApplicationUserId
                                                        && x.ArticleId == article.Id);
                    string content = article.Content;
                    if (userArticles == null)
                    {
                        userArticles = new UserArticle
                        {
                            ArticleId = article.Id,
                            ApplicationUserId = userVocab.ApplicationUserId,
                            IsRightLevel = 0,
                            IsSaved = false,
                            WordKnowCount = await CountWordsKnowAsync(content, userVocab.ApplicationUserId)
                        };
                        _dbContext.Add(userArticles);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        int charOccur = DictionaryUtilities.CountCharacterOccurence(content, word);
                        userArticles.WordKnowCount += charOccur;
                        if (charOccur > 0)
                            await EditUserArticle(userArticles);
                    }

                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Add Vocabulary");
                Console.WriteLine(e.Message);
            }
        }

        public async Task EditUserArticle(UserArticle userArticcle)
        {
            if (userArticcle == null) return;
            if (userArticcle.ApplicationUserId == null || userArticcle.ArticleId == null) return;
            _dbContext.Attach(userArticcle).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }



        public async Task EditUserVocabulary(UserVocabulary userVocabulary)
        {
            if (userVocabulary == null) return;
            if (userVocabulary.ApplicationUserId == null || userVocabulary.VocabularyId == null) return;

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

        public int CountByCategory(string content, string category)
        {
            VocabularyCategory categoryEnum = (VocabularyCategory)Enum.Parse(typeof(VocabularyCategory), category);
            var list = _dbContext.Vocabularies.Where(x => x.Category == categoryEnum).Select(x => x.Word).ToList();
            int count = 0;
            foreach (var word in list)
            {
                count += DictionaryUtilities.CountCharacterOccurence(content, word);
            }
            return count;
        }

        public async Task<int> CountWordsKnowAsync(string content, string userId)
        {
            int count = 0;
            var list = await _dbContext.UserVocabularies.Where(x => x.ApplicationUserId == userId)
                                             .Include(x => x.Vocabulary)
                                             .Select(x => x.Vocabulary.Word).ToListAsync();
            foreach (var word in list)
            {
                count += DictionaryUtilities.CountCharacterOccurence(content, word);
            }
            return count;
        }

        public async Task<int> CountWordReview(string userId)
        {
            int count = 0;
            count = await _dbContext.UserVocabularies.Where(x => x.ApplicationUserId == userId)
                                                        .Where(x => x.ReviewDate.Date < DateTime.Today.Date)
                                                        .CountAsync();
            return count;
        }

        public async Task<int> CountWordNew(string userId)
        {
            int count = 0;
            count = await _dbContext.UserVocabularies.Where(x => x.ApplicationUserId == userId)
                                                     .Where(x => x.ReviewDate.Date == DateTime.Today.Date)
                                                     .CountAsync();
            return count;
        }

        public async Task<UserVocabulary> GetUserVocabularyReviewAsync(string userId)
        {
            var userVocabulary = await _dbContext.UserVocabularies
                                        .Where(x => x.ApplicationUserId == userId)
                                        .Where(x => x.ReviewDate.Date <= DateTime.Today.Date)
                                        .OrderBy(x => x.ReviewDate)
                                        .FirstOrDefaultAsync();
            return userVocabulary;
        }

        public async Task ReviewVocabulary(UserVocabulary userVocabulary, string action)
        {
            switch (action)
            {
                case "Forget":
                    userVocabulary.ReviewDate = DateTime.Now.AddMinutes(int.Parse(Configuration["SRSSettingsDefault:ForgetInterval"]));
                    userVocabulary.NextReviewInterval = int.Parse(Configuration["SRSSettingsDefault:FirstInterval"]);
                    break;
                case "Hard":
                    userVocabulary.NextReviewInterval = (int)Math.Ceiling(userVocabulary.NextReviewInterval * double.Parse(Configuration["SRSSettingsDefault:Hard"]));
                    userVocabulary.ReviewDate = DateTime.Now.AddDays(userVocabulary.NextReviewInterval);
                    break;
                case "Easy":
                    userVocabulary.NextReviewInterval = (int)(userVocabulary.NextReviewInterval * double.Parse(Configuration["SRSSettingsDefault:Easy"]));
                    userVocabulary.ReviewDate = DateTime.Now.AddDays(userVocabulary.NextReviewInterval);
                    break;
                default:
                    break;
            }
            await EditUserVocabulary(userVocabulary);
        }

        public async Task AddJLPTVocabularies(string userId, string level)
        {
            var listToAdd = new List<Vocabulary>(); 
            if(level == "N5")
            {
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N5"));
            }
            if (level == "N4")
            {
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N5"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N4"));
            }
            if (level == "N3")
            {
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N5"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N4"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N3"));
            }
            if (level == "N2")
            {
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N5"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N4"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N3"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N2"));
            }
            if (level == "N1")
            {
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N5"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N4"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N3"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N2"));
                listToAdd.AddRange(await GetJLPTVocabulariesAsync("N1"));
            }
            foreach(var vocab in listToAdd)
            {
                var userVocabulary = new UserVocabulary
                {
                    ApplicationUserId = userId,
                    VocabularyId = vocab.Id,
                    Front = vocab.Word,
                    Back = vocab.Meaning,
                    Hint = "None",
                    NextReviewInterval = 9999,
                    ReviewDate = DateTime.Today.AddDays(9999)
                };
                await AddUserVocabularies(userVocabulary);
            }
        }

        public async Task<List<Vocabulary>> GetJLPTVocabulariesAsync(string level)
        {
            var list = new List<Vocabulary>();
            VocabularyCategory categoryEnum = (VocabularyCategory)Enum.Parse(typeof(VocabularyCategory), level);
            list = await _dbContext.Vocabularies.Where(x => x.Category == categoryEnum).ToListAsync();
            return list;
        }
    }
}
