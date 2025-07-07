using System.Text.RegularExpressions;

using Scraper.Expressions;

namespace Scraper
{
    public partial class Menu : Form
    {
        public Menu() => InitializeComponent();

        private string? DirectoryName;

        private string? BoardTag;

        private string? ThreadId;

        private int SuccessCount;

        private string UserAgent;

        private HttpClientHandler ClientHandler;

        private HttpClient Client;

        private const string BASE_URL = "https://a.4cdn.org";
        private const string IMAGE_URL = "https://i.4cdn.org";

        private void Menu_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("C:\\Scraper"))
                Directory.CreateDirectory("C:\\Scraper");

            ReadAndAssignUserAgent();
        }

        private void ReadAndAssignUserAgent()
        {
            const string configFileName = "config.env";

            if (!File.Exists(configFileName))
            {
                File.WriteAllLines(configFileName, new[] { "USER_AGENT=" });

                MessageBox.Show("File \"config.env\" was not found, so it has been created. Add your user-agent in it to avoid errors. The scraper will now exit.", "Config error");
                Environment.Exit(0);
            }

            string[] requestHeader = File.ReadAllText(configFileName).Split('=', 2);

            if (string.IsNullOrWhiteSpace(requestHeader[1]))
            {
                MessageBox.Show($"{requestHeader[0]} has no given user-agent value in \"config.env\". The scraper will now exit.", "Config error");
                Environment.Exit(0);
            }

            this.UserAgent = requestHeader[1].Trim();

            this.ClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.All
            };

            this.Client = new HttpClient(this.ClientHandler);
            this.Client.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);

            configLabel.Text = "[User-Agent set]";
            configLabel.ForeColor = Color.Green;
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

            string transformedThreadUrl = $"{BASE_URL}/{this.BoardTag}/thread/{this.ThreadId}.json";

            using (HttpResponseMessage statusCode = await this.Client.GetAsync(transformedThreadUrl))
            {
                pageStatusLabel.Text = $"[Page returned {statusCode.StatusCode}]";

                switch (statusCode.IsSuccessStatusCode)
                {
                    case true: pageStatusLabel.ForeColor = Color.Green; break;
                    case false: pageStatusLabel.ForeColor = Color.Red; return;
                }

                Task<string> threadContent = this.Client.GetStringAsync(transformedThreadUrl);

                foreach (Match imageMatch in Regexr.ImageRegex.Matches(threadContent.Result))
                {
                    listBox1.Items.Add($"{IMAGE_URL}/{this.BoardTag}/{imageMatch.Groups[2].Value}{imageMatch.Groups[1].Value}");
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
                string fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);

                using (FileStream fileWriter = new FileStream($"C:\\Scraper\\{this.DirectoryName}\\{fileName}", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Stream imageStream = await this.Client.GetStreamAsync(fileUrl))
                    {
                        await imageStream.CopyToAsync(fileWriter);
                    }
                }

                this.SuccessCount++;
            }
        }

        private void LinkBox_TextChanged(object sender, EventArgs e)
        {
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
            this.ThreadId = match.Groups[3].Value;
            this.DirectoryName = $"Thread_{this.ThreadId}_{this.BoardTag}";
        }
    }
}