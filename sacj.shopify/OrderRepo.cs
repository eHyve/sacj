using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace sacj.shopify
{


    public class OrderRepo //Connexion Shopify
    {
        private readonly HttpClient client = new HttpClient();

        public OrderRepo()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            
            //var byteArray = Encoding.ASCII.GetBytes("d593f2cf4945c39b1f986054af5dbaf5:d2dedb94c81752215bc0b81ff91e3eac");
            var byteArray = Encoding.ASCII.GetBytes("f64cdd7e68d9af1ebdd9a40ace45b3b8:aa36be43ce22ec2af5ce6cd252c58d82");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<Orders> GetOrders()
        {
            Orders orders = new Orders();
            try
            {
                var queryUri = "https://soutien-aux-commercants-jurassiens-ch.myshopify.com/admin/api/2020-01/orders.json?limit=250&status=closed";
                var doQuery = true;
                while(doQuery)
                {
                    var response = await this.client.GetAsync(queryUri);
                    
                    var orderResult = await response.Content.ReadAsStreamAsync();
                    var ordersChunk = await JsonSerializer.DeserializeAsync<Orders>(orderResult);
                    orders.orders.AddRange(ordersChunk.orders);

                    if(response.Headers.Contains("Link"))
                    {
                        var linkParam = response.Headers.GetValues("Link").First(); //.Split(";");
                        if(linkParam.Contains(" rel=\"next\""))
                        {
                            var link = linkParam.Contains(" rel=\"previous\"") ? linkParam.Split(",")[1].Split(";") : linkParam.Split(";");
                            if (link.Length == 2 && link[1].Equals(" rel=\"next\""))
                            {
                                var pageInfo = HttpUtility.ParseQueryString(link[0]).Get("page_info").Replace(">", "");
                                queryUri = "https://soutien-aux-commercants-jurassiens-ch.myshopify.com/admin/api/2020-01/orders.json?limit=250&page_info=" + pageInfo;
                            }
                            else
                            {
                                doQuery = false;
                            }
                        }
                        else
                        {
                            doQuery = false;
                        }
                        
                    }
                    else
                    {
                        doQuery = false;
                    }
                    
                }
                
                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

    }

}
