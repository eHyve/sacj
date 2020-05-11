using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sacj.shopify
{
    public class ConsoRepo 
    {
        public async Task<IEnumerable<IGrouping<long, OrderItemPair>>> GetAllConso()// Recherche toutes les données en 1 seule fois. 
        {
            try
            {
                var ordersRepo = new OrderRepo();
                var orders = await ordersRepo.GetOrders();

                var giftCardsRepo = new GiftCardRepo();
                var giftCards = await giftCardsRepo.GetGiftCards();

                var ordersByProdId = orders.orders
                                     .SelectMany(o => o.line_items, (order, item) => new OrderItemPair { Order = order, Item = item })
                                     .GroupBy(pair => pair.Item.product_id)
                                     .Select(group => {
                                         group.Select(orderItem => {
                                             orderItem.GiftCards = giftCards.gift_cards.Where(gc => gc.initial_value.Equals(orderItem.Item.price) &&
                                                                                                   gc.order_name.Equals(orderItem.Order.name) &&
                                                                                                   //gc.customer_name.Equals(orderItem.Order.billing_address.name) &&
                                                                                                   //gc.customer_email.Equals(orderItem.Order.contact_email)).Take(orderItem.Item.quantity).ToList();
                                                                                                   gc.line_item_id == orderItem.Item.id).ToList();
                                             return orderItem;
                                         }).ToList();
                                         return group;
                                     }).ToList();



                return ordersByProdId;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public class MyGC
        {
            public long id { get; set; }
            public long line_item_id { get; set; }
            public string masked_code { get; set; }
        }
        
        public class Receipt
        {
            public List<MyGC> gift_cards { get; set; }
        }


        public class Fullfillement
        {
            public long id { get; set; }
            public Receipt receipt { get; set; }
        }

        public async Task<int> GetFullfillements()
        {
            try
            {
                var ordersRepo = new OrderRepo();
                var orders = await ordersRepo.GetOrders();

                //var ffs = orders.orders.SelectMany(o => o.fulfillments).Select(f => ((Fullfillement)f).receipt).SelectMany(fl => fl.gift_cards).ToList();
                var ffs = orders.orders.SelectMany(o => o.fulfillments).Select(f => JsonConvert.DeserializeObject<Fullfillement>(f.ToString()).receipt).SelectMany(fl => fl.gift_cards).ToList();

                using (var writer = new StreamWriter("./fullfillements.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(ffs);
                }

                return ffs.Count;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<IGrouping<long, OrderItemPair>>> GetConsoByProductId(long productId) //Recherches toutes les conso par commerçant
        {
            try
            {
                var ordersRepo = new OrderRepo();
                var orders = await ordersRepo.GetOrders();

                var giftCardsRepo = new GiftCardRepo();
                var giftCards = await giftCardsRepo.GetGiftCards();

                var ordersByProdId = orders.orders
                                     .SelectMany(o => o.line_items, (order, item) => new OrderItemPair { Order = order, Item = item })
                                     .Where(pair => pair.Item.product_id == productId)
                                     .GroupBy(pair => pair.Item.product_id)
                                     .Select(group => {
                                         if (productId == 4716981780616) // HOT FIX Coup de Patte
                                         {
                                             group.Select(orderItem =>
                                             {
                                                 if (orderItem.Item.name.StartsWith("Au coup de patte"))
                                                 {
                                                     orderItem.GiftCards = giftCards.gift_cards.Where(gc => gc.initial_value.Equals(orderItem.Item.price) &&
                                                                                                            gc.order_name.Equals(orderItem.Order.name) &&
                                                                                                            //gc.customer_name.Equals(orderItem.Order.billing_address.name) &&
                                                                                                            //gc.customer_email.Equals(orderItem.Order.contact_email)).Take(orderItem.Item.quantity).ToList();
                                                                                                            gc.line_item_id == orderItem.Item.id).ToList();
                                                 }
                                                 return orderItem;
                                             }).Select(o => o.GiftCards != null).ToList();
                                             return group;
                                         }
                                         else
                                         {
                                             group.Select(orderItem =>
                                             {
                                                 orderItem.GiftCards = giftCards.gift_cards.Where(gc => gc.initial_value.Equals(orderItem.Item.price) &&
                                                                                                        gc.order_name.Equals(orderItem.Order.name) &&
                                                                                                        //gc.customer_name.Equals(orderItem.Order.billing_address.name) &&
                                                                                                        //gc.customer_email.Equals(orderItem.Order.contact_email)).Take(orderItem.Item.quantity).ToList();
                                                                                                        gc.line_item_id == orderItem.Item.id).ToList();
                                                 return orderItem;
                                             }).ToList();
                                             return group;
                                         }
                                     }).ToList();



                if (productId == 4720725131400)
                {
                    var ordersCoupPattes = orders.orders
                                     .SelectMany(o => o.line_items, (order, item) => new OrderItemPair { Order = order, Item = item })
                                     .Where(pair => pair.Item.product_id == 4716981780616)
                                     .GroupBy(pair => pair.Item.product_id)
                                     .Select(group => {
                                         group.Select(orderItem => {
                                             orderItem.GiftCards = giftCards.gift_cards.Where(gc => gc.initial_value.Equals(orderItem.Item.price) &&
                                                                                                   gc.order_name.Equals(orderItem.Order.name) &&
                                                                                                   //gc.customer_name.Equals(orderItem.Order.billing_address.name) &&
                                                                                                   //gc.customer_email.Equals(orderItem.Order.contact_email)).Take(orderItem.Item.quantity).ToList();
                                                                                                   gc.line_item_id == orderItem.Item.id).ToList();
                                             return orderItem;
                                         }).ToList();
                                         return group;
                                     }).ToList();
                    var falseCoups = ordersCoupPattes.First().Where(o => o.Item.name.StartsWith("Atelier de Coiffure")).ToList();
                    var x = ordersByProdId.ElementAt(0);
                    var plop = ordersByProdId.ElementAt(0).Concat(falseCoups);
                    
                    var i = ordersByProdId.ElementAt(0).Count();
                    //var fuck = i + x;
                    //ordersByProdId.Select(group => { return group.Concat(falseCoups); });

                }

                return ordersByProdId;
                
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }



    }
}
