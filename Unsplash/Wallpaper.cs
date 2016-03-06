using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace Unsplash
{
    public static class Wallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Tile = -1,
            Center = 0,
            Stretch = 2,
            Fill = 10,
            Fit = 6,
            Span = 22
        }

        public static void Set(Uri uri, Style style)
        {
            Stream s = new System.Net.WebClient().OpenRead(uri.AbsoluteUri);
            String uris = uri.ToString();
            Image img = Image.FromStream(s);
            
            uris = uris.Substring(uris.IndexOf("ph"));
            uris = uris.Substring(0, uris.LastIndexOf("?"));

            string tempPath = "C:/Users/" + Environment.UserName + "/Pictures/Unsplash/" +  uris + ".jpg";
            Directory.CreateDirectory("C:/Users/" + Environment.UserName + "/Pictures/Unsplash");
            img.Save(tempPath, ImageFormat.Jpeg);
            Console.Out.WriteLine(tempPath);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Tile)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString(CultureInfo.InvariantCulture));
                key.SetValue(@"TileWallpaper", 1.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                key.SetValue(@"WallpaperStyle", ((int)style).ToString(CultureInfo.InvariantCulture));
                key.SetValue(@"TileWallpaper", 0.ToString(CultureInfo.InvariantCulture));
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,0,tempPath,SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
