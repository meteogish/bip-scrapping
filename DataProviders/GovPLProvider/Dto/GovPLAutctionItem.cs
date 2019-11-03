using System;
using BipScrappingApp.DataProviders.Models;

namespace BipScrappingApp.DataProviders.GovPLProvider.Dto
{
    public class GovPLAutctionItem
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public Uri Url { get; set; }

        public AuctionItem ToCoreModel()
        {
            return new AuctionItem(Title, PublishDate, "https://www.gov.pl" + Url.ToString(), null);
        }
    }
}