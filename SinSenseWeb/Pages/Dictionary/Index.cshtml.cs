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
        public IActionResult OnGet(string q)
        {
            word = q;
            if (!string.IsNullOrWhiteSpace(q))
            {
                words = sinhalaDictionary.GetWords(q);
            }
            return Page();
        }
    }
}
