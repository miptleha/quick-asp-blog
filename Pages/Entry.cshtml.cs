using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;

namespace quick_asp_blog.Pages
{
    public class EntryModel : PageModel
    {
        public BlogEntry Entry { get; private set; }
        public string Error { get; private set; }
        public bool Login { get; private set; }

        public IActionResult OnGetCreate()
        {
            if (!HttpContext.Session.Keys.Contains("user"))
            {
                string value = Request.PathBase + Request.Path + Request.QueryString.Value;
                return RedirectToPage("Login", new { return_url = value });
            }
                
            return OnGet(null);
        }
        
        public IActionResult OnGet(string id)
        {
            Login = HttpContext.Session.Keys.Contains("user");
            GetEntry(id);
            return Page();
        }

        void GetEntry(string id, bool create = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                //create new
                var dt = DateTime.Now;
                Entry = new BlogEntry();
                if (create)
                {
                    Entry.Id = dt.ToString("yyyy-MM-dd HH-mm-ss");
                    BlogEntry.SaveOne(Entry);
                }
            }
            else
            {
                //edit
                Entry = BlogEntry.ReadOne(id);
            }
        }

        public IActionResult OnPostRegister()
        {
            var id = Request.Form["id"][0];
            GetEntry(id, true);

            var caption = Request.Form["caption"][0];
            var text = Request.Form["text"][0];

            Entry.Caption = caption;
            Entry.Text = text;

            BlogEntry.SaveOne(Entry);

            return Redirect("~/");
        }

        public IActionResult OnPostDelete()
        {
            var id = Request.Form["id"][0];
            if (!string.IsNullOrEmpty(id))
            {
                GetEntry(id);
                BlogEntry.DeleteOne(Entry);
            }

            return Redirect("~/");
        }

        public IActionResult OnPostBack()
        {
            return Redirect("~/");
        }
    }
}
