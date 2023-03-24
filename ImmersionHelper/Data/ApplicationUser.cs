using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace ImmersionHelper.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        ICollection<UserVocabulary> UserVocabularies { get; set;}
        ICollection<UserArticle> UserArticles { get; set;}

        ICollection<Post> Posts { get; set; }
    }
}
