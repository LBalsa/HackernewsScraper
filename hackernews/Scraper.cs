using CommandLine;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace hackernews
{
    public class Scraper
    {
        private const String newsUrl = "https://news.ycombinator.com/";

        public static void Main(string[] args)
        {
            int postCount = 0;

            // validate arguments
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (o.Posts > 0 && o.Posts <= 100)
                    postCount = o.Posts;
                else
                    throw new ArgumentOutOfRangeException("Posts needs to be a number between 1 and 100");
            });

            // compile and print news posts
            Console.WriteLine(JsonConvert.SerializeObject(CompilePosts(postCount), Formatting.Indented));
        }

        public static List<NewsPost> CompilePosts(int postCount)
        {
            List<NewsPost> newsPosts = new List<NewsPost>();
            List<HtmlNode> posts = new List<HtmlNode>();

            int page = 1;

            // iterate through pages untill all posts are retrieved
            while (Math.Abs(postCount) > posts.Count)
            {
                posts.AddRange(ExtractPostsFromUrl(newsUrl + "news?p=" + page).Take(Math.Abs(postCount) - posts.Count));
                page++;
            }

            // serialise posts            
            for (int i = 0; i < postCount && i < posts.Count; i++)
            {
                NewsPost newsPost = NewsPostFromNode(posts[i]);

                if (newsPost != null)
                    newsPosts.Add(newsPost);
            }

            return newsPosts;
        }

        static List<HtmlNode> ExtractPostsFromUrl(String url)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            return doc.DocumentNode.SelectNodes("//tr[@class='athing']").Cast<HtmlNode>().ToList();
        }

        static NewsPost NewsPostFromNode(HtmlNode node)
        {
            NewsPost newsPost = new NewsPost();

            // validate all fields and return null if not to criteria

            // rank
            string rank = node.SelectSingleNode(".//span[@class='rank']").InnerText;
            rank = Regex.Match(rank, @"\d+").Value;
            newsPost.rank = !String.IsNullOrEmpty(rank) ? Int32.Parse(rank) : -1;

            if (newsPost.rank < 0)
                return null;

            // title and url node
            var titleNode = node.SelectSingleNode(".//a[@class='storylink']");

            // title
            if (!String.IsNullOrEmpty(titleNode.InnerText))
                newsPost.title = titleNode.InnerText;
            else
                return null;

            // url
            if (Uri.IsWellFormedUriString(titleNode.GetAttributeValue("href", ""), UriKind.Absolute))
                newsPost.url = titleNode.Attributes["href"].Value;
            else
                return null;

            // subtext node
            var subNode = node.NextSibling.SelectSingleNode(".//td[@class='subtext']");

            // author
            string author = subNode.SelectSingleNode(".//a[@class='hnuser']").InnerText;

            if (!String.IsNullOrEmpty(author))
                newsPost.author = author;
            else
                return null;

            // points
            string score = subNode.SelectSingleNode(".//span[@class='score']").InnerText;
            score = Regex.Match(score, @"\d+").Value;
            newsPost.points = !String.IsNullOrEmpty(score) ? Int32.Parse(score) : -1;

            if (newsPost.points < 0)
                return null;

            // comments
            string comment = subNode.SelectNodes(".//a").Last().InnerText;
            if (comment.Equals("discuss"))
                newsPost.comments = 0;
            else
            {
                comment = Regex.Match(comment, @"\d+").Value;
                newsPost.comments = !String.IsNullOrEmpty(comment) ? Int32.Parse(comment) : -1;

                if (newsPost.comments < 0)
                    return null;
            }

            return newsPost;
        }

        internal class Options
        {
            [Option('p', "posts", Required = true, HelpText = "how many posts to print. A positive integer <= 100")]
            public int Posts { get; set; }
        }

        public class NewsPost
        {
            //NewsPost(string Title, string Url, string Author ) => title, 

            public string title { get; set; }
            public string url { get; set; }
            public string author { get; set; }
            public int points { get; set; }
            public int comments { get; set; }
            public int rank { get; set; }
        }
    }
}
