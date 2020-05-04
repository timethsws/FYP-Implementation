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
       
        public IndexModel(ILogger<IndexModel> logger,GoogleTranslateService translaterService)
        {
          
        }

        public void OnGet()
        {
            
        }
    }
}
