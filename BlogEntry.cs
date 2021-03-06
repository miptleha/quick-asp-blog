using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace quick_asp_blog
{
    public class BlogEntry
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Text { get; set; }
        public bool More { get; set; }

        public static List<BlogEntry> ReadAll()
        {
            var files = Directory.GetFiles(BlogPath, "*.txt").Select(f => new FileInfo(f))
                .OrderByDescending(f => f.CreationTime).Select(f => f.FullName).ToArray();
            var res = new List<BlogEntry>();
            for (int i = 0; i < files.Length; i++)
            {
                var f = files[i];
                var id = Path.GetFileNameWithoutExtension(f);
                var e = ReadOne(id, 3);
                res.Add(e);
            }
            return res;
        }

        public static BlogEntry ReadOne(string id, int maxRows = 0)
        {
            var path = Path.Combine(BlogPath, id + ".txt");
            var res = new BlogEntry();
            res.Id = id;
            res.DateCreated = File.GetCreationTime(path);
            res.DateModified = File.GetLastWriteTime(path);

            //each file contains caption (first line) and text
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                res.Caption = sr.ReadLine();

                List<string> text = new List<string>();
                string s;
                int lines = 0;
                for (int i = 0; (s = sr.ReadLine()) != null; i++)
                {
                    if (maxRows > 0 && string.IsNullOrWhiteSpace(s)) //trim on main page, not trim when edit
                        continue;
                    lines++;
                    if (maxRows != 0 && lines > maxRows)
                    {
                        res.More = true;
                        break;
                    }
                    text.Add(s);
                }
                if (res.More)
                {
                    while (text.Count >= maxRows)
                    {
                        text.RemoveAt(text.Count - 1);
                    }
                }

                res.Text = string.Join('\n', text);
            }

            return res;
        }

        public static void SaveOne(BlogEntry e)
        {
            var path = Path.Combine(BlogPath, e.Id + ".txt");

            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine(e.Caption);
                sw.Write(e.Text);
            }
        }

        public static void DeleteOne(BlogEntry e)
        {
            var path = Path.Combine(BlogPath, e.Id + ".txt");
            File.Delete(path);
        }

        static string _path;
        static string BlogPath
        {
            get
            {
                if (_path == null)
                {
                    var path = Assembly.GetExecutingAssembly().Location;
                    var dir = Path.GetDirectoryName(path);
                    _path = Path.Combine(dir, "blog_entries");
                    if (!Directory.Exists(_path))
                        Directory.CreateDirectory(_path);
                }

                return _path;
            }
        }
    }
}
