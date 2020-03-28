using HandlebarsDotNet;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace sacj.shopify
{
    public class EmailRepo
    {
        string[] Scopes = { GmailService.Scope.GmailReadonly };
        private UserCredential credential;
        private GmailService gmailService;

        public EmailRepo()
        {
            using (var stream = new FileStream("credentialsGMail.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Gmail API service.
            gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SACJ 1.0",
            });
        }

        public async Task<int> GenerateAllEmails() //Generation rapports 
        {
            var nbGenerated = 0;
            try
            {
                var consoRepo = new ConsoRepo();
                var orderConso = await consoRepo.GetAllConso();

                var googleRepo = new GoogleRepo();
                var merchants = await googleRepo.GetAllMerchants();

                foreach (var group in orderConso)
                {
                    var merchant = merchants.Where(m => m.Id == group.Key).FirstOrDefault();
                    nbGenerated += await GenerateEmail(group, merchant) ? 1 : 0;
                }

                return nbGenerated;
            }
            catch (Exception ex)
            {
                return nbGenerated;
            }
        }

        public async Task<int> GenerateEmailsById(long id)
        {
            try
            {
                var consoRepo = new ConsoRepo();
                var orderConso = await consoRepo.GetConsoByProductId(id);

                var googleRepo = new GoogleRepo();
                var merchants = await googleRepo.GetAllMerchants();
                var merchant = merchants.Where(m => m.Id == orderConso.First().Key).FirstOrDefault();

                await GenerateEmail(orderConso.First(), merchant);

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private async Task<bool> GenerateEmail(IGrouping<long, OrderItemPair> group, Merchant merchant) //Generate PDF
        {
            try
            {
                //Noémie pour tests, delete this line
                merchant.Email = "noemie.petignat@gmail.com";

                // Define parameters of request.
                UsersResource.LabelsResource.ListRequest request = gmailService.Users.Labels.List("me");

                // List labels.
                IList<Label> labels = request.Execute().Labels;
                Console.WriteLine("Labels:");
                if (labels != null && labels.Count > 0)
                {
                    foreach (var labelItem in labels)
                    {
                        Debug.WriteLine("{0}", labelItem.Name);
                    }
                }
                else
                {
                    Debug.WriteLine("No labels found.");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
