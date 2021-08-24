using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.IO;
using System.Text;

namespace quick_asp_blog.Pages
{
    public class LoginModel : PageModel
    {
        public string ReturnUrl { get; set; }
        public string Error { get; set; }

        public void OnGet(string return_url)
        {
            ViewData["ReturnUrl"] = return_url;
        }

        public IActionResult OnPost()
        {
            string name = Request.Form["name"];
            string pas = Request.Form["pass"];
            string return_url = Request.Form["return_url"];

            GetUser(out string userName, out string userPas);
            if (name == userName && pas == userPas)
            {
                HttpContext.Session.SetString("user", "admin");
                return Redirect(return_url);
            }

            Error = "Invalid user or password";
            ViewData["ReturnUrl"] = return_url;
            return Page();
        }

        void GetUser(out string name, out string pas)
        {
            name = null;
            pas = "";
            using (var f = new StreamReader(PassPath, Encoding.UTF8))
            {
                string text;
                while ((text = f.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(text) || text[0] == '#')
                        continue;

                    if (name == null)
                        name = text;
                    else
                        pas = text;
                }
            }
        }

        static string _path;
        static string PassPath
        {
            get
            {
                if (_path == null)
                {
                    var path = Assembly.GetExecutingAssembly().Location;
                    var dir = Path.GetDirectoryName(path);
                    _path = Path.Combine(dir, "login.txt");
                }

                return _path;
            }
        }
    }
}
