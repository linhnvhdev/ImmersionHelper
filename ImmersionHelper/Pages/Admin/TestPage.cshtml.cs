using ImmersionHelper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace ImmersionHelper.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class TestPageModel : PageModel
    {
        ApplicationDbContext _dbContext;

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public TestPageModel(ApplicationDbContext dbContext)
        {
            _dbContext= dbContext;
        }

        public void OnGet()
        {
            /*
            var list = _dbContext.Vocabularies.Take(100).ToList();
            Input.Vocabularies = _dbContext.Vocabularies.Take(1000).ToList();
            */
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "jmdict-eng-3.3.1.json");
            string jsonString = System.IO.File.ReadAllText(filePath);
            //Input.Root = JsonSerializer.Deserialize<JMDictData.Root>(jsonString);
            Input.Root = JsonConvert.DeserializeObject<JMDictData.Root>(jsonString);
        }

        public class InputModel
        {
            public List<Vocabulary> Vocabularies { get; set; }

            public JMDictData.Root Root { get; set; }
        }
    }
}
