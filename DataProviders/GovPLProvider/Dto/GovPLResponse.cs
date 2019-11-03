using System.Collections.Generic;
using BipScrappingApp.DataProviders.Models;

namespace BipScrappingApp.DataProviders.GovPLProvider.Dto
{
    public class GovPLResponse
    {
        public int Count { get; set; }

        public IList<GovPLAutctionItem> Results { get; set; }

    }
}