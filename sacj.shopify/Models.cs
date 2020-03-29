using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace sacj.shopify
{
    public class Orders
    {
        public List<Order> orders { get; set; }
        public Orders()
        {
            orders = new List<Order>();
        }
    }

    public class GiftCards
    {
        public IEnumerable<GiftCard> gift_cards { get; set; }
        public GiftCards()
        {
            gift_cards = new List<GiftCard>();
        }
    }

    public class OrderItemPair
    {
        public Order Order { get; set; }
        public LineItem Item { get; set; }

        public List<GiftCard> GiftCards { get; set; }
    }

    public class ShopMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class PresentmentMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class PriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TaxLine
    {
        public string price { get; set; }
        public double rate { get; set; }
        public string title { get; set; }
        public PriceSet price_set { get; set; }
    }

    public class TotalLineItemsPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalDiscountsSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalShippingPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class SubtotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalTaxSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalDiscountSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }


    public class OriginLocation
    {
        public long id { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class LineItem
    {
        public long id { get; set; }
        public long variant_id { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public string fulfillment_service { get; set; }
        public long product_id { get; set; }
        public bool requires_shipping { get; set; }
        public bool taxable { get; set; }
        public bool gift_card { get; set; }
        public string name { get; set; }
        public object variant_inventory_management { get; set; }
        public IList<object> properties { get; set; }
        public bool product_exists { get; set; }
        public int fulfillable_quantity { get; set; }
        public int grams { get; set; }
        public string price { get; set; }
        public string total_discount { get; set; }
        public object fulfillment_status { get; set; }
        public PriceSet price_set { get; set; }
        public TotalDiscountSet total_discount_set { get; set; }
        public IList<object> discount_allocations { get; set; }
        public string admin_graphql_api_id { get; set; }
        public IList<TaxLine> tax_lines { get; set; }
        public OriginLocation origin_location { get; set; }
    }

    public class BillingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public object phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public object province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string address2 { get; set; }
        public object company { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public object province_code { get; set; }
    }

    public class ClientDetails
    {
        public string browser_ip { get; set; }
        public string accept_language { get; set; }
        public string user_agent { get; set; }
        public object session_hash { get; set; }
        public int browser_width { get; set; }
        public int browser_height { get; set; }
    }

    public class PaymentDetails
    {
        public string credit_card_bin { get; set; }
        public object avs_result_code { get; set; }
        public object cvv_result_code { get; set; }
        public string credit_card_number { get; set; }
        public string credit_card_company { get; set; }
    }

    public class DefaultAddress
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public object province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public object phone { get; set; }
        public string name { get; set; }
        public object province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }

        //public bool default { get; set; }
    }

    public class Customer
    {
        public long id { get; set; }
        public string email { get; set; }
        public bool accepts_marketing { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public object last_order_id { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public object phone { get; set; }
        public string tags { get; set; }
        public object last_order_name { get; set; }
        public string currency { get; set; }
        public DateTime accepts_marketing_updated_at { get; set; }
        public object marketing_opt_in_level { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DefaultAddress default_address { get; set; }
    }

    public class Order
    {
        public long id { get; set; }
        public string email { get; set; }
        public object closed_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int number { get; set; }
        public object note { get; set; }
        public string token { get; set; }
        public string gateway { get; set; }
        public bool test { get; set; }
        public string total_price { get; set; }
        public string subtotal_price { get; set; }
        public int total_weight { get; set; }
        public string total_tax { get; set; }
        public bool taxes_included { get; set; }
        public string currency { get; set; }
        public string financial_status { get; set; }
        public bool confirmed { get; set; }
        public string total_discounts { get; set; }
        public string total_line_items_price { get; set; }
        public string cart_token { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public string name { get; set; }
        public string referring_site { get; set; }
        public string landing_site { get; set; }
        public object cancelled_at { get; set; }
        public object cancel_reason { get; set; }
        public string total_price_usd { get; set; }
        public string checkout_token { get; set; }
        public object reference { get; set; }
        public object user_id { get; set; }
        public object location_id { get; set; }
        public object source_identifier { get; set; }
        public object source_url { get; set; }
        public DateTime processed_at { get; set; }
        public object device_id { get; set; }
        public object phone { get; set; }
        public string customer_locale { get; set; }
        public int app_id { get; set; }
        public string browser_ip { get; set; }
        public object landing_site_ref { get; set; }
        public int order_number { get; set; }
        public IList<object> discount_applications { get; set; }
        public IList<object> discount_codes { get; set; }
        public IList<object> note_attributes { get; set; }
        public IList<string> payment_gateway_names { get; set; }
        public string processing_method { get; set; }
        public long checkout_id { get; set; }
        public string source_name { get; set; }
        public object fulfillment_status { get; set; }
        public IList<TaxLine> tax_lines { get; set; }
        public string tags { get; set; }
        public string contact_email { get; set; }
        public string order_status_url { get; set; }
        public string presentment_currency { get; set; }
        public TotalLineItemsPriceSet total_line_items_price_set { get; set; }
        public TotalDiscountsSet total_discounts_set { get; set; }
        public TotalShippingPriceSet total_shipping_price_set { get; set; }
        public SubtotalPriceSet subtotal_price_set { get; set; }
        public TotalPriceSet total_price_set { get; set; }
        public TotalTaxSet total_tax_set { get; set; }
        public IList<LineItem> line_items { get; set; }
        public IList<object> fulfillments { get; set; }
        public IList<object> refunds { get; set; }
        public string total_tip_received { get; set; }
        public string admin_graphql_api_id { get; set; }
        public IList<object> shipping_lines { get; set; }
        public BillingAddress billing_address { get; set; }
        [JsonIgnore]
        public ClientDetails client_details { get; set; }
        public PaymentDetails payment_details { get; set; }
        public Customer customer { get; set; }
    }

    public class GiftCard
    //Expired?,Enabled?
    {
        [Name("Id")]
        public long id { get; set; }
        [Name("Balance")]
        public string balance { get; set; }
        [Name("Created At")]
        public DateTime created_at { get; set; }

        //public DateTime updated_at { get; set; }
        [Name("Currency")]
        public string currency { get; set; }
        [Name("Initial Value")]
        public string initial_value { get; set; }
        [Name("Disabled At")]
        public string disabled_at { get; set; }
        //public long line_item_id { get; set; }
        //public object api_client_id { get; set; }
        //public object user_id { get; set; }
        //public long customer_id { get; set; }
        [Name("Note")]
        public string note { get; set; }
        [Name("Expires On")]
        public string expires_on { get; set; }
        //public object template_suffix { get; set; }
        [Name("Last Characters")]
        public string last_characters { get; set; }
        //public long order_id { get; set; }
        [Name("Customer Name")]
        public string customer_name { get; set; }
        [Name("Email")]
        public string customer_email { get; set; }
        [Name("Order Name")]
        public string order_name { get; set; }
    }

    public class Merchant
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string IBAN { get; set; }
    }

    public class Payment
    {
        [Name("Id")]
        public long Id { get; set; }
        [Name("Date")]
        public string Date { get; set; }
        [Name("IBAN")]
        public string IBAN { get; set; }
        [Name("Nom")]
        public string Nom { get; set; }
        [Name("Total")]
        public decimal Total { get; set; }
    }

    public class PainItem
    {
        [Name("Id")]
        public long Id { get; set; }
        [Name("IBAN")]
        public string IBAN { get; set; }
        [Name("BIC")]
        public string BIC { get; set; }
        [Name("Name")]
        public string Name { get; set; }
        [Name("Address")]
        public string Address { get; set; }
        [Name("Amount")]
        public decimal Amount { get; set; }
    }

}
