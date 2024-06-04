# Scraper
 * Downloads images and media from 4chan threads.

# Preview
 ![](https://i.imgur.com/9CEe34G.gif)

# Opening the solution file
 * Since the scraper is made using **.NET 6.0 / C#10** you will need Visual Studio 2022 in order to open and compile the project.

# Opening the exectuable file
 * You will need [.NET 6.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime?cid=getdotnetcore) in order to run this application.

# Usage
 * Open the application, paste a valid 4chan thread link and click **scrape**.

# Notice
 * The download is single-threaded therefore scraping big threads will take longer time.

# Directories
 * The scraper creates initial directory located at **C:\\Scraper**
 * Each thread has its own specific directory located at **C:\\Scraper\\Thread_ID_BOARD**

# Todo
- [X] Thread specific directories.
- [ ] Add catalog scraper for specific board.
- [ ] More information about the thread (filename, preview url, subject).
- [ ] Option to export the thread information as json.
