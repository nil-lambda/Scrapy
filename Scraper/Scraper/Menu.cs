using System.Text.RegularExpressions;

using Scraper.Expressions;

namespace Scraper
{
    public partial class Menu : Form
    {
        public Menu() => InitializeComponent();

        private string? DirectoryName { get; set; }

        private string? BoardTag { get; set; }

        private string? BoardId { get; set; }

        private int SuccessCount { get; set; }

        private Dictionary<string, string> RequestConfig;

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
                using (StreamWriter stream = new(File.Create(configFileName)))
                {
                    stream.WriteLine("USER_AGENT=");
                    stream.WriteLine("COOKIE=");
                }

                MessageBox.Show("File \"config.env\" was not found, so it has been created. Add information in it to avoid errors. The scraper will now exit.", "Config error");
                Environment.Exit(0);
            }

            using (StreamReader stream = new StreamReader(configFileName))
            {
                while (!stream.EndOfStream)
                {
                    string currentLine = stream.ReadLine();
                    string[] currentRequestHeader = currentLine.Split('=', 2);

                    if (string.IsNullOrEmpty(currentRequestHeader[1]))
                    {
                        MessageBox.Show($"{currentRequestHeader[0]} has no given value in \"config.env\". You wont be able to scrape.", "Config error");
                        return;
                    }

                    this.RequestConfig[currentRequestHeader[0].Trim()] = currentRequestHeader[1].Trim();

                    /*
                     * currentRequestHeader[0] is the request header (USER_AGENT)
                     * currentRequestHeader[1] is the request header value given by the user (Mozilla/5.0...)
                     */
                }
            }

            configLabel.Text = "[Cookie && Useragent set]";
            configLabel.ForeColor = Color.Green;
        }

        private void CreateDirectory()
        {
            if (Directory.Exists($"C:\\Scraper\\{DirectoryName}"))
                return;

            Directory.CreateDirectory($"C:\\Scraper\\{DirectoryName}");
        }

        private async void ScrapeButton_Click(object sender, EventArgs e)
        {
            CreateDirectory();

            listBox1.Items.Clear();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage statusCode = await client.GetAsync(linkBox.Text))
                {
                    pageStatusLabel.Text = $"[Page returned {statusCode.StatusCode}]";

                    switch (statusCode.IsSuccessStatusCode)
                    {
                        case true: pageStatusLabel.ForeColor = Color.Green; break;
                        case false: pageStatusLabel.ForeColor = Color.Red; return;
                    }

                    Task<string> urlContent = client.GetStringAsync(linkBox.Text);

                    foreach (Match imageMatch in Regexr.ImageLinkRegex.Matches(urlContent.Result))
                    {
                        listBox1.Items.Add(imageMatch.Groups[1].Value);
                    }

                    scrapeTotalLabel.ForeColor = Color.DarkMagenta;
                    scrapeTotalLabel.Text = $"[Trying to scrape {listBox1.Items.Count} items...]";

                    await Download();

                    scrapeTotalLabel.ForeColor = Color.Green;
                    scrapeTotalLabel.Text = $"[Scraped {SuccessCount} items]";
                }
            }
        }

        private async Task Download()
        {
            foreach (string item in listBox1.Items)
            {
                foreach (Match getName in Regexr.FileNameRegex.Matches(item))
                {
                    string fileName = getName.Groups[2].Value;

                    using (FileStream fileWriter = new FileStream($"C:\\Scraper\\{DirectoryName}\\{fileName}", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            using (Stream imageStream = await client.GetStreamAsync($"https://{item}"))
                            {
                                await imageStream.CopyToAsync(fileWriter);
                                SuccessCount++;
                            }
                        }
                    }
                }
            }
        }

        private void LinkBox_TextChanged(object sender, EventArgs e)
        {
            switch (Regexr.ThreadLinkRegex.IsMatch(linkBox.Text))
            {
                case true:
                    scrapeButton.Enabled = true;
                    label2.ForeColor = Color.Green;
                    label2.Text = $"[Thread link detected - /{Regexr.ThreadLinkRegex.Match(linkBox.Text).Groups[2].Value}/]";
                    BoardTag = Regexr.ThreadLinkRegex.Match(linkBox.Text).Groups[2].Value;
                    BoardId = Regexr.ThreadLinkRegex.Match(linkBox.Text).Groups[3].Value;
                    DirectoryName = $"Thread_{BoardId}_{BoardTag}";
                    break;

                case false:
                    scrapeButton.Enabled = false;
                    label2.ForeColor = Color.Red;
                    label2.Text = "[Invalid link format]";
                    break;
            }
        }
    }
}