using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace Project_PRN222.Pages.Login
{
    public class ForgetPasswordModel : PageModel
    {


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string RecaptchaResponse { get; set; }



    }
}
