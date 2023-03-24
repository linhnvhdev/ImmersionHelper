using ImmersionHelper.Data;
using ImmersionHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace ImmersionHelper.Pages.Vocabularies
{
    public class ImportModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private DictionaryServices _dictionaryServices;

        public ImportModel(UserManager<ApplicationUser> userManager, DictionaryServices dictionaryServices)
        {
            _userManager = userManager;
            _dictionaryServices = dictionaryServices;
        }


        [Required(ErrorMessage = "Please choose at least one file.")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "png,jpg,jpeg,gif",ErrorMessage =("Please only choose txt file type"))]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public string Message { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Content { get; set; }

        public void OnGet()
        {

        }

        public List<List<String>> Result { get; set; }
        public List<List<String>> ResultShow { get; set; }

        [BindProperty]

        public int columnForFront { get; set; }

        [BindProperty]
        public int columnForBack { get; set; }

        [BindProperty]
        public int columnForHint { get; set; }

        [BindProperty]

        public string ResultSerialize { get; set; }



        public async Task<IActionResult> OnPostAsync()
        {
            if (FileUploads != null)
            {
                if(FileUploads.Length != 1)
                {
                    Message = "Please upload 1 file";
                    return Page();
                }
                Result = new List<List<string>>();
                var fileUpload = FileUploads[0];
                var result = new StringBuilder();
                using (var reader = new StreamReader(fileUpload.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();
                        if (line.StartsWith("#")) continue;
                        Result.Add(line.Split("\t").ToList());
                        result.AppendLine(line);
                    }
                }
                ResultShow = Result.Take(5).ToList();
                ResultSerialize = JsonSerializer.Serialize(Result);
            }
            else
            {
                Message = "Can't upload file";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (ResultSerialize == null) return NotFound();
            var list = JsonSerializer.Deserialize<List<List<String>>>(ResultSerialize);
            foreach(var row in list)
            {
                var vocabulary = await _dictionaryServices.FindVocabulariesExact(row[columnForFront],1);
                if(vocabulary == null || vocabulary.Count == 0) continue;
                if (columnForFront >= row.Count || columnForBack >= row.Count || columnForHint >= row.Count) continue;
                var test = columnForHint == -1 ? "None" : columnForHint.ToString();
                var userVocabulary = new UserVocabulary
                {
                    ApplicationUserId = _userManager.GetUserId(HttpContext.User),
                    VocabularyId = vocabulary[0].Id,
                    Front = row[columnForFront],
                    Back = row[columnForBack],
                    Hint = columnForHint == -1 ? "None" : row[columnForHint],
                    NextReviewInterval = 1,
                    ReviewDate = DateTime.Today.AddDays(1)
                };
                await _dictionaryServices.AddUserVocabularies(userVocabulary);
            }
            Message = "Add Success";
            return Page();
        }

    }
}
