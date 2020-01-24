using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackerscrapecmd
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://news.ycombinator.com/");

            var headerNames = doc.DocumentNode.SelectNodes("//a[@class = 'storylink']").ToList();

            foreach (var title in headerNames)
            {
                Console.WriteLine(title.InnerText);
            }

            Console.ReadLine();
            
        }
    }
}
