using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Unsplash
{
    //TODO add unit test
    public class FeedManager
    {
        private Uri image { get; set; }
        private string addToUrl = "";
        public FeedManager()
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);
            string dimens = config.ImageWidth.ToString() + "x" + config.ImageHeight.ToString() + "/";

            if (config.Featured)
            {
                addToUrl += "featured/" + dimens;
            } 
            else if (config.Category != null)
            {
                addToUrl += "category/" + config.Category;
            }
            else
            {
                addToUrl += "random/" + dimens;
            }
            if (config.Schedule != null)
            {
                addToUrl += config.Schedule + "/";
            }
            if (config.SearchTerm != null)
            {
                addToUrl += "?" + config.SearchTerm;
            }
            this.image = new Uri("https://source.unsplash.com/"  + addToUrl , UriKind.Absolute); //TODO add to app.config
            Console.Out.WriteLine("Source: " + image.ToString());
            GrtUrl(image.ToString());
        }

        public void GrtUrl(string url) {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowAutoRedirect = false;  // IMPORTANT

            webRequest.Timeout = 10000;           // timeout 10s
            webRequest.Method = "HEAD";
            // Get the response ...
            HttpWebResponse webResponse;
            using (webResponse = (HttpWebResponse)webRequest.GetResponse()) {
                // Now look to see if it's a redirect
                if ((int)webResponse.StatusCode >= 300 && (int)webResponse.StatusCode <= 399) {
                    string uriString = webResponse.Headers["Location"];
                    Console.Out.WriteLine("Redirect to " + uriString ?? "NULL");

                    webResponse.Close(); // don't forget to close it - or bad things happen!
                    image = new Uri(uriString ?? "NULL");
                }

            }

        }

        public Uri GetWallpaper()
        {
            if (!image.ToString().Contains("/photo-")) {
                Console.Out.WriteLine("New Feed!!");
                this.image = new Uri("https://source.unsplash.com/" + addToUrl, UriKind.Absolute); //TODO add to app.config
                GrtUrl(image.ToString());


            }
            return image;
        }
    }
}
