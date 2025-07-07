using System.Text.RegularExpressions;

namespace Scraper.Expressions
{
    internal class Regexr
    {
        public static Regex ThreadLinkRegex = new Regex(@"boards\.(4chan|4channel)\.org\/(.*?)\/thread\/(\d+)");

        public static Regex ImageRegex = new Regex("\"ext\":\\s*\"(.*?)\".*?\"tim\":\\s*(\\d+)", RegexOptions.Singleline);
    }
}
