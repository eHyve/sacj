using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sacj.shopify;

namespace Sacj.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopifyController : ControllerBase
    {
        /**
         * Sheets
         * Client ID: 279327866088-04o4adg64hfgu8bqv6u5nvrqof7mkoce.apps.googleusercontent.com
         * Secret: tNw_2SmfXTqHx_zsT2clNWyk
         * 
         * Docs
         * Client ID: 279327866088-04o4adg64hfgu8bqv6u5nvrqof7mkoce.apps.googleusercontent.com
         * Secret: tNw_2SmfXTqHx_zsT2clNWyk
         *
         */

        private readonly ILogger<ShopifyController> _logger;

        public ShopifyController(ILogger<ShopifyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> result = new List<string>();
            result.Add("Hello World");
            return result;
        }

        [HttpGet("orders")]
        public async Task<Orders> GetOrders()
        {
            var ordersRepo = new OrderRepo();
            var result = await ordersRepo.GetOrders();
            return result;
        }

        [HttpGet("gift_cards")]
        public async Task<GiftCards> GetGiftCards()
        {
            var giftCardsRepo = new GiftCardRepo();
            var result = await giftCardsRepo.GetGiftCards();
            return result;
        }

        [HttpGet("statement/all")]
        public async Task<int> GenerateAllStatements()
        {
            var reportRepo = new ReportRepo();
            var result = await reportRepo.GenerateAllReports();
            return result;
        }

        [HttpGet("statement/{productId}")]
        public async Task<int> GenerateStatementById(long productId)
        {
            var reportRepo = new ReportRepo();
            var result = await reportRepo.GenerateReportsById(productId);
            return result;
        }

        [HttpGet("conso/{productId}")]
        public async Task<string> DoConso(long productId)
        {
            var consoRepo = new ConsoRepo();
            //var result = await consoRepo.GetConsoByProductId(4720732143752);
            var result = await consoRepo.GetAllConso();
            return "";
        }

        [HttpGet("google/all")]
        public async Task<List<Merchant>> GetAllMerchants()
        {
            var googleRepo = new GoogleRepo();
            var result = await googleRepo.GetAllMerchants();
            return result;
        }
    }
}
