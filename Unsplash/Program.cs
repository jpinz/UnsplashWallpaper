using System;
using System.IO;
using Newtonsoft.Json;

namespace Unsplash
{
    class Program
    {
        static void Main(string[] args)
        {
            var feedManager = new FeedManager();
            var url = feedManager.GetWallpaper();
          
            if (url != null && url.IsWellFormedOriginalString())
            {
                if (!url.ToString().Contains("/photo-"))
                {
                    url = feedManager.GetWallpaper();
                }
                Wallpaper.Set(url, Wallpaper.Style.Fill); //TODO make style available in config
            }
        }
    }
}
