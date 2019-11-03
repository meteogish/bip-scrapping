using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BipScrappingApp.DataProviders;
using BipScrappingApp.DataProviders.GovPLProvider;
using BipScrappingApp.DataProviders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BipScrappingApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuctionItemsController : ControllerBase
    {
        private readonly ILogger<AuctionItemsController> _logger;

        public AuctionItemsController(ILogger<AuctionItemsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<AuctionItem> Get([FromQuery] string searchText, [FromQuery] string fromDate)
        {
            var fromDateParsed = DateTime.Parse(fromDate);

            IAuctionItemsProvider[] providers = new [] {
                (IAuctionItemsProvider) new BipKowrAuctionItemsProvider(),
                new GovPLAuctionItemsProvider(),
                new BipSejmikKielceAuctionItemsProvider()
            };

            var tasks = providers.Select(provider => 
                        provider.GetItems(searchText, fromDateParsed, HttpContext.RequestAborted)
                                .ContinueWith(t => t.IsCompletedSuccessfully ? t.Result : Enumerable.Empty<AuctionItem>()))
                                .ToArray();

            Task.WaitAll(tasks);

            var items = tasks
                    .Where(t => t.IsCompletedSuccessfully)
                    .SelectMany(t => t.Result)
                    .Where(x => x.UploadDate >= fromDateParsed)
                    .OrderByDescending(x => x.UploadDate);
                            
            return items;
        }
    }
}
