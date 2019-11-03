using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BipScrappingApp.DataProviders.Models;

namespace BipScrappingApp.DataProviders
{
    public interface IAuctionItemsProvider
    {
        Task<IEnumerable<AuctionItem>> GetItems(string searchText, DateTime fromDate, CancellationToken cancellationToken);
    }
}