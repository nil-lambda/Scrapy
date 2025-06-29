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


        private void Menu_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("C:\\Scraper"))
                Directory.CreateDirectory("C:\\Scraper");

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