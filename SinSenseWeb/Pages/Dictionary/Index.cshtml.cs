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
        private readonly SinhalaMorphologyService sinhalaMorphologyService;

        [BindProperty]
        public List<string> words { get; set; }

        [BindProperty]
        public string word { get; set; }

        [BindProperty]
        public string lemma { get; set; }
        [BindProperty]
        public List<string> lemmaWords { get; set; }

        [BindProperty]
        public string stem { get; set; }
        [BindProperty]
        public List<string> stemWords { get; set; }

        public IndexModel(SinhalaDictionaryService sinhalaDictionary,SinhalaMorphologyService sinhalaMorphologyService)
        {
            this.sinhalaDictionary = sinhalaDictionary;
            this.sinhalaMorphologyService = sinhalaMorphologyService;
        }
        public IActionResult OnGet(string q)
        {
            word = q;
            if (!string.IsNullOrWhiteSpace(word))
            {
                words = sinhalaDictionary.GetWords(word);
                lemma = sinhalaMorphologyService.GetLemma(word);
                stem = sinhalaMorphologyService.GetStem(word);

                if(!lemma.Equals(word))
                {
                    lemmaWords = sinhalaDictionary.GetWords(lemma);
                }

                if (!stem.Equals(word) && !stem.Equals(lemma))
                {
                    stemWords = sinhalaDictionary.GetWords(stem);
                }
            }
            return Page();
        }
    }
}
