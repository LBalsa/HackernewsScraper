using hackernews;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Scraper scraper = new Scraper();

            Assert.Pass();
        }
    }
}