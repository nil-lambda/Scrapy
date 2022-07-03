using System.Text.RegularExpressions;

namespace Scraper.Expressions
{
    internal class Regexr
    {
        public static Regex ThreadLinkRegex = new Regex(@"boards\.(4chan|4channel)\.org\/(.*?)\/thread\/(\d+)");

        public static Regex ImageLinkRegex = new Regex("<a\\s+class=\"fileThumb\"\\s+href=\"\\/\\/(.*?)\"");

        public static Regex FileNameRegex = new Regex(@"(.*)\/(.*)");
    }
}
