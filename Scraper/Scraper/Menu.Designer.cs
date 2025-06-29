namespace Scraper
{
    partial class Menu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            linkBox = new TextBox();
            listBox1 = new ListBox();
            scrapeButton = new Button();
            label2 = new Label();
            pageStatusLabel = new Label();
            scrapeTotalLabel = new Label();
            configLabel = new Label();
            SuspendLayout();
            // 
            // linkBox
            // 
            linkBox.Location = new Point(9, 26);
            linkBox.Name = "linkBox";
            linkBox.Size = new Size(330, 23);
            linkBox.TabIndex = 0;
            linkBox.TextAlign = HorizontalAlignment.Center;
            linkBox.TextChanged += LinkBox_TextChanged;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(9, 85);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(330, 109);
            listBox1.TabIndex = 2;
            // 
            // scrapeButton
            // 
            scrapeButton.Enabled = false;
            scrapeButton.Location = new Point(8, 55);
            scrapeButton.Name = "scrapeButton";
            scrapeButton.Size = new Size(332, 24);
            scrapeButton.TabIndex = 3;
            scrapeButton.Text = "Scrape";
            scrapeButton.UseVisualStyleBackColor = true;
            scrapeButton.Click += ScrapeButton_Click;
            // 
            // label2
            // 
            label2.Location = new Point(12, 5);
            label2.Name = "label2";
            label2.Size = new Size(327, 15);
            label2.TabIndex = 4;
            label2.Text = "[Waiting for thread URL]";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pageStatusLabel
            // 
            pageStatusLabel.AutoSize = true;
            pageStatusLabel.Location = new Point(5, 197);
            pageStatusLabel.Name = "pageStatusLabel";
            pageStatusLabel.Size = new Size(122, 15);
            pageStatusLabel.TabIndex = 5;
            pageStatusLabel.Text = "[Page status: Waiting]";
            // 
            // scrapeTotalLabel
            // 
            scrapeTotalLabel.Anchor = AnchorStyles.Right;
            scrapeTotalLabel.Location = new Point(181, 197);
            scrapeTotalLabel.Name = "scrapeTotalLabel";
            scrapeTotalLabel.Size = new Size(162, 15);
            scrapeTotalLabel.TabIndex = 6;
            scrapeTotalLabel.Text = "[Not scraping yet]";
            scrapeTotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // configLabel
            // 
            configLabel.AutoSize = true;
            configLabel.ForeColor = Color.Red;
            configLabel.Location = new Point(5, 216);
            configLabel.Name = "configLabel";
            configLabel.Size = new Size(181, 15);
            configLabel.TabIndex = 7;
            configLabel.Text = "[No cookie and/or useragent set]";
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(347, 235);
            Controls.Add(configLabel);
            Controls.Add(scrapeTotalLabel);
            Controls.Add(pageStatusLabel);
            Controls.Add(label2);
            Controls.Add(scrapeButton);
            Controls.Add(listBox1);
            Controls.Add(linkBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Menu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Scraper - 4chan";
            Load += Menu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox linkBox;
        private ListBox listBox1;
        private Button scrapeButton;
        private Label label2;
        private Label pageStatusLabel;
        private Label scrapeTotalLabel;
        private Label configLabel;
    }
}