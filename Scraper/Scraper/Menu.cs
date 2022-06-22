using System.Text;
using System.Text.RegularExpressions;

namespace Scraper
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private readonly Regex linkRegex = new Regex(@"boards\.(4chan|4channel)\.org\/(.*?)\/thread\/(\d+)");
        private readonly Regex imageRegex = new Regex("<a\\s+class=\"fileThumb\"\\s+href=\"\\/\\/(.*?)\"");
        private readonly Regex fileNameRegex = new Regex(@"(.*)\/(.*)");

        private async void ScrapeButton_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage statusCode = await client.GetAsync(linkBox.Text))
                {
                    label3.Text = $"[Page returned {statusCode.StatusCode}]";

                    switch (statusCode.IsSuccessStatusCode)
                    {
                        case true: label3.ForeColor = Color.Green; break;
                        case false: label3.ForeColor = Color.Red; return;
                    }

                    Task<string> urlContent = client.GetStringAsync(linkBox.Text);

                    foreach (Match imageMatch in imageRegex.Matches(urlContent.Result))
                    {
                        listBox1.Items.Add(imageMatch.Groups[1].Value);
                    }

                    label4.Text = $"[Scraped {listBox1.Items.Count} items]";
                    await Task.Run(() => Download());
                }
            }
        }

        private async void Download()
        {
            foreach (string item in listBox1.Items)
            {
                foreach (Match getName in fileNameRegex.Matches(item))
                {
                    string fileName = getName.Groups[2].Value;

                    using (FileStream fileWriter = new FileStream($"C:\\Scraper\\{fileName}", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            using (Stream imageBytes = await client.GetStreamAsync($"https://{item}"))
                            {
                                await Task.Run(() => imageBytes.CopyTo(fileWriter));
                            }
                        }
                    }
                }
            }
        }

        private void LinkBox_TextChanged(object sender, EventArgs e)
        {
            switch (linkRegex.IsMatch(linkBox.Text))
            {
                case true:
                    scrapeButton.Enabled = true;
                    label2.ForeColor = Color.Green;
                    label2.Text = $"[Thread link detected - /{linkRegex.Match(linkBox.Text).Groups[2].Value}/]";
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