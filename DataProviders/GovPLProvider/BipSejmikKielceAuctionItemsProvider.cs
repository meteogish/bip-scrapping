using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BipScrappingApp.DataProviders.Models;
using Flurl;
using HtmlAgilityPack;

namespace BipScrappingApp.DataProviders.GovPLProvider
{
    public class BipSejmikKielceAuctionItemsProvider : IAuctionItemsProvider
    {
        private string BaseUrl { get; } = "http://bip.sejmik.kielce.pl/component/mijosearch/search.html";
        public async Task<IEnumerable<AuctionItem>> GetItems(string searchText, DateTime fromDate, CancellationToken cancellationToken)
        {
            HtmlWeb web = new HtmlWeb();

            var document = await web.LoadFromWebAsync(BaseUrl.SetQueryParam("query", searchText));

            var itemNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'artykuly1_box1')]");

            return itemNodes.Skip(1).Select(itemNode => {
                var linkNode = itemNode.SelectSingleNode(".//div[contains(@class, 'artykuly1_tytul1')]/a");

                var linkSelf = "http://bip.sejmik.kielce.pl" + linkNode.GetAttributeValue("href", string.Empty);

                var title = HtmlEntity.DeEntitize(linkNode.InnerText);

                var description = string.Empty;

                var dateNodeText = itemNode.SelectSingleNode(".//div[contains(@class, 'srodek_osoby1')]//tr[position() = 2]").InnerText;
                string dateText = dateNodeText.Split(',')[1].TrimStart();

                DateTime date = DateTime.ParseExact(dateText, "d MMMM yyyy", new System.Globalization.CultureInfo("pl-PL"));

                return new AuctionItem(title, date, linkSelf, description);
            })
                .Where(x => x.UploadDate > fromDate && x.ContainsWords(Regex.Split(searchText, @"\W")));
        }
    }
}