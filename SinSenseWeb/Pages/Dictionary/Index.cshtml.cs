using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SinSense.Infastructure.Services;

namespace SinSense.Web.Pages.Dictionary
{
    public class IndexModel : PageModel
    {
        private readonly SinhalaDictionaryService sinhalaDictionary;

        [BindProperty]
        public List<string> words { get; set; }

        [BindProperty]
        public string word { get; set; }

        public IndexModel(SinhalaDictionaryService sinhalaDictionary)
        {
            this.sinhalaDictionary = sinhalaDictionary;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var wordStr = Request.Form["dictionary"];
            if (!string.IsNullOrWhiteSpace(wordStr))
            {
                words = sinhalaDictionary.GetWords(wordStr);
            }
            return Page();
        }
    }
}
