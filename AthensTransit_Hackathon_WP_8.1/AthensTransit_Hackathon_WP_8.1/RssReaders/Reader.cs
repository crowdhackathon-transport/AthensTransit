using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace RssReader.RssReaders
{
    #region FEED ITEMS
    public class oasaFeedItem
    {
        public string Title { set; get; }
        public Uri Link { set; get; }
        public DateTime PubDate { set; get; }
    }

    public class apergiaFeedItem
    {
        public string Title { set; get; }
        public string Summary { set; get; }
        public string Status { set; get; }

        public Uri Link { set; get; }
        public DateTime PubDate { set; get; }
        public DateTime StrikeDate { set; get; }

    }

    #endregion

    #region READERS
    public class OasaReader
    {
        public static async Task<ObservableCollection<oasaFeedItem>> getFeedsAsync(string url)
        {
           //The web object that will retrieve our feeds..
           SyndicationClient client = new SyndicationClient();
           //The URL of our feeds..
           Uri feedUri = new Uri(url);
           //Retrieve async the feeds..
           var feed = await client.RetrieveFeedAsync(feedUri);
           //The list of our feeds..
           ObservableCollection<oasaFeedItem> feedData = new ObservableCollection<oasaFeedItem>();
           //Fill up the list with each feed content..
           foreach (SyndicationItem item in feed.Items)
           {               
                oasaFeedItem of = new oasaFeedItem();
                of.PubDate = item.PublishedDate.DateTime;
                of.Title = item.Title.Text;
                try{ of.Link = item.Links[0].Uri; }catch(Exception){}

                 feedData.Add(of);
             }
             return feedData;
        }
    }

    public class ApergiaReader
    {
        public static async Task<ObservableCollection<apergiaFeedItem>> getFeedsAsync(string url)
        {
            //The web object that will retrieve our feeds..
            SyndicationClient client = new SyndicationClient();
            //The URL of our feeds..
            Uri feedUri = new Uri(url);
            //Retrieve async the feeds..
            var feed = await client.RetrieveFeedAsync(feedUri);
            //The list of our feeds..
            ObservableCollection<apergiaFeedItem> feedData = new ObservableCollection<apergiaFeedItem>();
            //Fill up the list with each feed content..
            foreach (SyndicationItem item in feed.Items)
            {
                apergiaFeedItem af = new apergiaFeedItem();
                af.PubDate = item.PublishedDate.DateTime;
                af.Title = item.Title.Text;
                af.Summary = extractSummary(item.Summary.Text);
                try { af.Link = item.Links[0].Uri; }
                catch (Exception) { }

                feedData.Add(af);
            }
            return feedData;
        }

        private static string extractSummary(string text)
        {
            //Regex rx = new Regex(@"\b\/:strong\b.*\bstrong\b", RegexOptions.Multiline);
            //Regex rx = new Regex(@"(?<=<(\w+)>).*(?=<\/\1>)", RegexOptions.Multiline);
            //Regex rx = new Regex(@"((?<!^)\b[A-Z])(?=[^<>]+<[^\/>][^>]+>)", RegexOptions.Multiline);
            
            //string [] matches = rx.Split(text);
            //MatchCollection matches = rx.Matches(text);

            //Debug.WriteLine("=================================================");
            //Debug.WriteLine(array[2]);
            //foreach (var match in array)
            //{
            //    Debug.WriteLine(match);
            //}
            //Debug.WriteLine("tipotis");

            string[] array = text.Split(new string[] { "<strong>", "</strong>" }, StringSplitOptions.None);

            return array[2];
        }
    }
    #endregion
}
