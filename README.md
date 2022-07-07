# Image-Scraper
 * Downloads images from 4chan threads.

# Preview
 ![](https://cdn.discordapp.com/attachments/916391368480415744/989314097499754566/Scraper_Preview.gif)

# Opening the solution file
 * Since the scraper is made using **.NET 6.0 / C#10** you will need Visual Studio 2022 in order to open and compile the project.

# Opening the exectuable file
 * You will need [.NET 6.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime?cid=getdotnetcore) in order to run this application.

# Usage
 * Open the application, paste a valid 4chan thread link and click **scrape**.  
 * I recommend to use it on small threads (30 or less images) since it's downloading files slowly.

# Directories
 * The scraper creates only one directory on launch that is located at **C:\\Scraper**

# Todo
- [X] Thread specific directories.
- [ ] Add catalogue scraper for specific board.
- [ ] More information about the thread (filename, preview url, subject).
- [ ] Option to export the thread information as json.
