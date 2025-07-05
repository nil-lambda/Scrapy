using System.Text.RegularExpressions;

using Scraper.Expressions;

namespace Scraper
{
    public partial class Menu : Form
    {
        public Menu() => InitializeComponent();

        private string? DirectoryName;

        private string? BoardTag;

        private string? BoardId;

        private int SuccessCount;

        private bool CanScrape;

        private Dictionary<string, string> RequestConfig;

        private HttpClientHandler ClientHandler;

        private HttpClient Client;

        private void Menu_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("C:\\Scraper"))
                Directory.CreateDirectory("C:\\Scraper");

            ReadAndAssignConfig();
        }

        private void ReadAndAssignConfig()
        {
            const string configFileName = "config.env";
            this.RequestConfig = new Dictionary<string, string>();

            if (!File.Exists(configFileName))
            {
                File.WriteAllLines(configFileName, new[] { "USER_AGENT=", "COOKIE=" });

                MessageBox.Show("File \"config.env\" was not found, so it has been created. Add information in it to avoid errors. The scraper will now exit.", "Config error");
                Environment.Exit(0);
            }

            foreach (string currentLine in File.ReadAllLines(configFileName))
            {
                string[] currentRequestHeader = currentLine.Split('=', 2);

                if (string.IsNullOrEmpty(currentRequestHeader[1]))
                {
                    MessageBox.Show($"{currentRequestHeader[0]} has no given value in \"config.env\". You wont be able to scrape.", "Config error");
                    this.CanScrape = false;

                    return;
                }

                this.RequestConfig[currentRequestHeader[0].Trim()] = currentRequestHeader[1].Trim();

                /*
                 * currentRequestHeader[0] is the request header (USER_AGENT)
                 * currentRequestHeader[1] is the request header value given by the user (Mozilla/5.0...)
                 */
            }

            this.ClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.All
            };

            /* Additional predefined headers to make sure the request goes through successfully */
            this.Client = new HttpClient(this.ClientHandler);
            this.Client.DefaultRequestHeaders.Add("User-Agent", this.RequestConfig["USER_AGENT"]);
            this.Client.DefaultRequestHeaders.Add("Cookie", this.RequestConfig["COOKIE"]);
            this.Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
            this.Client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            this.Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            configLabel.Text = "[Cookie && Useragent set]";
            configLabel.ForeColor = Color.Green;
            this.CanScrape = true;
        }

        private void CreateDirectory()
        {
            if (Directory.Exists($"C:\\Scraper\\{this.DirectoryName}"))
                return;

            Directory.CreateDirectory($"C:\\Scraper\\{this.DirectoryName}");
        }

        private async void ScrapeButton_Click(object sender, EventArgs e)
        {
            CreateDirectory();

            listBox1.Items.Clear();

            using (HttpResponseMessage statusCode = await this.Client.GetAsync(linkBox.Text))
            {
                pageStatusLabel.Text = $"[Page returned {statusCode.StatusCode}]";

                switch (statusCode.IsSuccessStatusCode)
                {
                    case true: pageStatusLabel.ForeColor = Color.Green; break;
                    case false: pageStatusLabel.ForeColor = Color.Red; return;
                }

                Task<string> threadHtmlContent = this.Client.GetStringAsync(linkBox.Text);

                foreach (Match imageMatch in Regexr.ImageLinkRegex.Matches(threadHtmlContent.Result))
                {
                    listBox1.Items.Add(imageMatch.Groups[1].Value);
                }

                scrapeTotalLabel.ForeColor = Color.DarkMagenta;
                scrapeTotalLabel.Text = $"[Trying to scrape {listBox1.Items.Count} items...]";

                await Download();

                scrapeTotalLabel.ForeColor = Color.Green;
                scrapeTotalLabel.Text = $"[Scraped {this.SuccessCount} items]";
                this.SuccessCount = 0;
            }
        }

        private async Task Download()
        {
            foreach (string fileUrl in listBox1.Items)
            {
                string fileName = Regexr.FileNameRegex.Match(fileUrl).Groups[2].Value;

                using (FileStream fileWriter = new FileStream($"C:\\Scraper\\{this.DirectoryName}\\{fileName}", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Stream imageStream = await this.Client.GetStreamAsync($"https://{fileUrl}"))
                    {
                        await imageStream.CopyToAsync(fileWriter);
                    }
                }

                this.SuccessCount++;
            }
        }

        private void LinkBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.CanScrape)
                return;

            Match match = Regexr.ThreadLinkRegex.Match(linkBox.Text);
            if (!match.Success)
            {
                scrapeButton.Enabled = false;
                label2.ForeColor = Color.Red;
                label2.Text = "[Invalid link format]";

                return;
            }

            scrapeButton.Enabled = true;
            label2.ForeColor = Color.Green;
            label2.Text = $"[Thread link detected - /{match.Groups[2].Value}/]";

            this.BoardTag = match.Groups[2].Value;
            this.BoardId = match.Groups[3].Value;
            this.DirectoryName = $"Thread_{this.BoardId}_{this.BoardTag}";
        }
    }
}