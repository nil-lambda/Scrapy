## About
Scrapy is media scraper for [4chan.org](https://4chan.org/) for archiving purposes. It downloads all media files off a thread locally.

## Compatibility & Requirements
Scrapy is developed for **Windows**. It may run on Linux with [Wine](https://www.winehq.org/) but this is untested.
* Requires [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime?cid=getdotnetcore) to run.

If you wish to compile the project yourself and/or make adjustments, you'll need:
* Visual Studio 2022 to open the project.
  * The project is built with .NET 6.0 (C# 10)

## Downloading
Download `Scraper.rar` from the [latest release](https://github.com/nil-lambda/Scrapy/releases/latest), extract it and run the executable file corresponding to your system.

## Errors and their meaning

If the **Page Status** turns red, check the message:
* `NotFound` - The thread is archived and no longer accessible. The scraper currently doesn't support scraping archived threads.
* `TooManyRequests` - Try changing your User-Agent in `config.env` file.

> The Scraper uses `Mozilla/5.0 (X11; Linux)` User-Agent by default specified by `config.env` file. It is not hard-coded for flexibility.

## Where is the media saved
All downloaded media is saved under `C:\Scraper`. Each thread gets its own subdirectory.