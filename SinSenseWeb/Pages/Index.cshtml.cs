using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SinSense.Infastructure.Services;
using SinSense.Infastructure.Services.External;

namespace SinSense.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GoogleTranslateService _translaterService;

        [BindProperty]
        public String text { get; set; }

        [BindProperty]
        public string translated { get; set; }
        public IndexModel(ILogger<IndexModel> logger,GoogleTranslateService translaterService)
        {
            _logger = logger;
            _translaterService = translaterService;
        }

        public void OnGet()
        {
            text = DateTime.Now.ToLongDateString();
        }
        public IActionResult OnPost()
        {
            //var translate = Request.Form["translate"];
            //if (!string.IsNullOrWhiteSpace(translate))
            //{
            //    translated = _translaterService.Translate(translate);
            //}
            //text = $"Translated \"{translate}\"";
            translated = "turned off for now :(";
            text = $"We'll be back soon :)";
            return Page();
        }
        
    }
}
