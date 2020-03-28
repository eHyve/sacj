using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sacj.shopify
{


    public class GiftCardRepo
    {
        private readonly HttpClient client = new HttpClient();

        public GiftCardRepo()
        {
            
        }

        public async Task<GiftCards> GetGiftCards()
        {
            var giftCards = new GiftCards();
            try
            {
                using (var reader = new StreamReader("./gift_cards_export_1.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<GiftCardMap>();
                    var records = csv.GetRecords<GiftCard>().ToList();
                    giftCards.gift_cards = records;
                    return giftCards;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

    }

    public sealed class GiftCardMap : ClassMap<GiftCard>
    {
        public GiftCardMap()
        {
            Map(g => g.id).Name("Id");
            Map(g => g.balance).Name("Balance");
            Map(g => g.created_at).Name("Created At");
            Map(g => g.currency).Name("Currency");
            Map(g => g.initial_value).Name("Initial Value");
            Map(g => g.disabled_at).Name("Disabled At");
            Map(g => g.note).Name("Note");
            Map(g => g.expires_on).Name("Expires On");
            Map(g => g.last_characters).Name("Last Characters");
            Map(g => g.customer_name).Name("Customer Name");
            Map(g => g.customer_email).Name("Email");
            Map(g => g.order_name).Name("Order Name");
        }
    }

}
