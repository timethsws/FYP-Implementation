using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SinSense.Core.Models;
using SinSense.Infastructure.Services;
using SinSense.Infastructure.Services.Sinhala;

namespace SinSense.Web.Pages.Sense
{
    public class IndexModel : PageModel
    {
        private readonly SinhalaDisambiguatorService sinhalaDisambiguatorService;

        [BindProperty]
        public DisambiguationResponse res { get; set; }

        [BindProperty]
        public string sentence { get; set; }

        public IndexModel(SinhalaDisambiguatorService sinhalaDisambiguatorService)
        {
            this.sinhalaDisambiguatorService = sinhalaDisambiguatorService;
        }

        public IActionResult OnGet(string q)
        {
            sentence = q;
            if(!string.IsNullOrWhiteSpace(q))
            {
                res = sinhalaDisambiguatorService.Disambiguate(sentence);
            }

            return Page();
        }
    }
}
