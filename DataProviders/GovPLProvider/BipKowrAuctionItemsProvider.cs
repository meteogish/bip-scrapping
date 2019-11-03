using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BipScrappingApp.DataProviders.Models;
using HtmlAgilityPack;

namespace BipScrappingApp.DataProviders.GovPLProvider
{
    public class BipKowrAuctionItemsProvider : IAuctionItemsProvider
    {
        private string BaseUrl { get; } = "http://bip.kowr.gov.pl/informacje-publiczne/zbedne-skladniki-majatkowe-kowr";
        public async Task<IEnumerable<AuctionItem>> GetItems(string searchText, DateTime fromDate, CancellationToken cancellationToken)
        {
            HtmlWeb web = new HtmlWeb();

            var document = await web.LoadFromWebAsync(BaseUrl);

            var itemNodes = document.DocumentNode.SelectNodes("//li[contains(@class, 'news-list-item')]");

            return itemNodes.Select(itemNode => {
                var linkNode = itemNode.SelectSingleNode(".//h3/a");
                var linkSelf = linkNode.GetAttributeValue("href", string.Empty);
                var title = HtmlEntity.DeEntitize(linkNode.InnerText);

                var description = HtmlEntity.DeEntitize(itemNode.SelectSingleNode(".//p").InnerText);
                var time = itemNode.SelectSingleNode(".//time").GetAttributeValue("datetime", string.Empty);

                return new AuctionItem(title, DateTime.Parse(time), linkSelf, description);
            })
                .Where(x => x.ContainsWords(Regex.Split(searchText, @"\W")));
        }
    }
}