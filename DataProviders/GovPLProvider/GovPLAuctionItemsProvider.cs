using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BipScrappingApp.DataProviders.GovPLProvider.Dto;
using BipScrappingApp.DataProviders.Models;
using Flurl;
using Flurl.Http;

namespace BipScrappingApp.DataProviders.GovPLProvider
{
    public class GovPLAuctionItemsProvider : IAuctionItemsProvider
    {
        private string BaseUrl { get; } = "https://www.gov.pl/api/data/";
        public async Task<IEnumerable<AuctionItem>> GetItems(string searchText, DateTime fromDate, CancellationToken cancellationToken)
        {   
            var response = await (BaseUrl
                .AppendPathSegment("search")
                .SetQueryParam("query", searchText)
                .SetQueryParam("category", "kategoria")
                .SetQueryParam("period", GetPeriodFromDate(fromDate))
                .SetQueryParam("page", "1")
                .SetQueryParam("size", "50")
                .GetJsonAsync<GovPLResponse>(cancellationToken));

            return response.Results.Where(x => x.PublishDate >= fromDate).Select(x => x.ToCoreModel()).OrderByDescending(x => x.UploadDate);    
        }

        private int GetPeriodFromDate(DateTime fromDate)
        {
            int daysBetween = (DateTime.Now - fromDate).Days;
            
            if (daysBetween <= 7)
            {
                return 90;
            }
            else if (daysBetween <= 30)
            {
                return 90;
            }
            else if (daysBetween <= 90)
            {
                return 90;
            }
            else 
            {
                return 365;
            }
        }
    }
}