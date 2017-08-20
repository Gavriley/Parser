using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string htmlCode;

            List<Post> posts = new List<Post>();

            MatchCollection matchBlocks;
            Match matchTitle;
            MatchCollection matchDates;
            Match matchAuthor;
            Match matchContent;
            Match matchTags;
            
            Regex regexBlock = new Regex("<article(.|\r|\n)*?</article>", RegexOptions.Multiline);
            Regex regexTitle = new Regex("<a href=\".*?\" rel=\"bookmark\">.*?</a>");
            Regex regexDate = new Regex("datetime=\".*?\"");
            Regex regexAuthor = new Regex("<a class=\"url.*?>(.|\r|\n)*?</a>");
            Regex regexContent = new Regex("<p>.*?&#8230;");
            Regex regexTags = new Regex("Tags:.*?</li>");

            Regex clearMatch = new Regex("<.*?>|\r|\n|&#8230;|\\[MSFT\\]"); 
            Regex clearDate = new Regex("(datetime=|\")");

            using (WebClient client = new WebClient())
            {
                int count = 1;

                //do
                //{
                //    try
                //    {
                //        htmlCode = client.DownloadString("https://blogs.msdn.microsoft.com/dotnet/page/" + count);
                //    }
                //    catch(System.Net.WebException)
                //    {
                //        break;
                //    }
                //    matchBlocks = regexBlock.Matches(htmlCode);

                //    foreach (Match block in matchBlocks)
                //    {
                //        matchTitle = regexTitle.Match(block.Value);
                //        matchDates = regexDate.Matches(block.Value);
                //        matchAuthor = regexAuthor.Match(block.Value);
                //        matchContent = regexContent.Match(block.Value);
                //        matchTags = regexTags.Match(block.Value);

                //        string title = clearMatch.Replace(matchTitle.Value, string.Empty);
                //        string createdDate = clearDate.Replace(matchDates[0].Value, string.Empty);
                //        string updatedDate = (matchDates.Count > 1) ?
                //            clearDate.Replace(matchDates[1].Value, string.Empty) : createdDate;
                //        string author = clearMatch.Replace(matchAuthor.Value, string.Empty);
                //        string content = clearMatch.Replace(matchContent.Value, string.Empty);
                //        string tags = clearMatch.Replace(matchTags.Value, string.Empty);

                //        posts.Add(new Post
                //        {
                //            Title = title,
                //            CreatedDate = DateTime.Parse(createdDate),
                //            UpdatedDate = DateTime.Parse(updatedDate),
                //            Author = author,
                //            Content = content,
                //            Tags = tags
                //        });
                //    }

                //    Console.WriteLine(count);

                //    ++count;
                //}
                //while (true);

                PrintInFileAsync(posts);

                Console.WriteLine("Finish");
            }

            Console.ReadKey();
        }

        public static async Task PrintInFileAsync(List<Post> posts)
        {
            await Task.Delay(10000);
            await Task.Run(() =>
            {
                if (!File.Exists("posts.txt"))
                {
                    File.Create("posts.txt");
                }
            });

            using (StreamWriter file = new StreamWriter("posts.txt"))
            {
                foreach (Post post in posts)
                {
                    file.WriteLine("Title: " + post.Title);
                    file.WriteLine("CreatedDate: " + post.CreatedDate);
                    file.WriteLine("UpdatedDate: " + post.UpdatedDate);
                    file.WriteLine("Author: " + post.Author);
                    file.WriteLine("Content: " + post.Content);
                    if (post.Tags != string.Empty)
                    {
                        file.WriteLine("Tags: " + post.Tags);
                    }
                    file.WriteLine("--------------------------------------------------");
                }
            }
        }
    }

    public class Post
    {
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
    }
}

