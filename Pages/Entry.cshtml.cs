using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace quick_asp_blog.Pages
{
    public class EntryModel : PageModel
    {
        public BlogEntry Entry { get; private set; }
        public string Error { get; private set; }
        
        public void OnGet(string id)
        {
            GetEntry(id);
        }

        void GetEntry(string id)
        {
            if (id == null)
            {
                //create new
                var dt = DateTime.Now;
                Entry = new BlogEntry();
                Entry.Id = dt.ToString("yyyy-MM-dd HH-mm-ss");
                BlogEntry.SaveOne(Entry);
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
            GetEntry(id);

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
            GetEntry(id);

            BlogEntry.DeleteOne(Entry);

            return Redirect("~/");
        }
    }
}
