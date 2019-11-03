using System;
using System.Linq;

namespace BipScrappingApp.DataProviders.Models
{
    public class AuctionItem
    {
        public string Title { get; }
        public DateTime UploadDate { get; }
        public Uri Link { get; }
        
        public string Description { get; }

        public AuctionItem(string title, DateTime uploadDate, string uri, string description)
        {
            Title = title;
            UploadDate = uploadDate;
            Link = new Uri(uri);
            Description = description;
        }

        internal bool ContainsWords(string[] words)
        {
            string forSearch = Description + Title;

            return words.Any(w => forSearch.Contains(w, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}