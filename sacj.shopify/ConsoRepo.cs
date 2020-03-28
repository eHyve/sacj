using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sacj.shopify
{
    public class ConsoRepo
    {
        public async Task<IEnumerable<IGrouping<long, OrderItemPair>>> GetAllConso()
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
                                                                                                   gc.customer_email.Equals(orderItem.Order.contact_email)).ToList();
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

        public async Task<IEnumerable<IGrouping<long, OrderItemPair>>> GetConsoByProductId(long productId)
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
                                         group.Select(orderItem => {
                                             orderItem.GiftCards = giftCards.gift_cards.Where(gc => gc.initial_value.Equals(orderItem.Item.price) &&
                                                                                                    gc.order_name.Equals(orderItem.Order.name) &&
                                                                                                    //gc.customer_name.Equals(orderItem.Order.billing_address.name) &&
                                                                                                    gc.customer_email.Equals(orderItem.Order.contact_email)).Take(orderItem.Item.quantity).ToList();
                                             return orderItem;
                                         }).ToList();
                                         return group; 
                                     }).ToList();

                return ordersByProdId;
                
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }



    }
}
