-Welcome to my hackernews post scraper-

-Running-

Via command on windows
1. navigate to "hackernews" inside this directory on the command prompt
2. Enter "dotnet run hackernews --posts x", x being an integer argument for the amount of posts being printed


Using docker:
1. set docker to use windows containers
2. navigate to "hackernews" inside this directory on the command prompt
3. run "docker build hackernews"
4. run "docker run hackernews --posts x", x being an integer argument for the amount of posts being printed

-Libraries-

CommandLineParser:

Altough a simple argument parser could be build with split / match / contains etc, the point to illustrate is that it is counter ituitive to build a primal parser when there is a much better tool ready to use.

It allows for easy expansion of the arguments if necessary and easily handling of them.

HtmlAgilityPack:

I had done webscraping in Python before, but not C#, this library had good documentation.

Newstonsoft.Json:

As with the parser, it would be possible to code the printing out, but that would both take time and not be as robust.

-Argument Handling-

From the "Input arguments" section (hackernews --posts n), I understood hackernews to be the app name and --posts the only parameter, I understand handling of extra websites could be programmed but it was obsolete in this case.
--posts has been made mandatory as default behaviour was not specified for the output (first post / first page / infinite scroll until user quits).

-Testing-

I had problems with the project dependency not working. I ended up creating another project and copying the code over, however there is no more time to add the tests, despite being a requirement.