
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.Diagnostics;
using System.Linq;

namespace sacj.shopify
{
    public class GoogleRepo
    {
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly,
                            GmailService.Scope.GmailReadonly,   //TestListGmailLabels
                            GmailService.Scope.GmailSend};
        private UserCredential credential;
        private SheetsService sheetService;
        private GmailService gmailService;
        private const string applicationName = "SACJ 1.0";

        public GoogleRepo()
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Docs API service.
            sheetService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            // Create Gmail API service.
            gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        public async Task<List<Merchant>> GetAllMerchants() //Lire les commerçants en direct
        {
            try
            {
                string spreadsheetId = "1c17X68KLE1Vh6-tYHTX6XM7THKkpayU2c8Dk5d1_xrE";
                string range = "'Réponses au formulaire 1'!A:M";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        sheetService.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;

                var merchantList = new List<Merchant>();
                foreach(var value in values)
                {
                    if (value.Count > 12)
                    {
                        long nil;
                        var merchant = new Merchant()
                        {
                            Id = long.TryParse(value[0].ToString().Replace(" ", ""), out nil) ? long.Parse(value[0].ToString().Replace(" ", "")) : 0,
                            Name = value[3].ToString(),
                            FirstName = value[4].ToString(),
                            CompanyName = value[5].ToString(),
                            Email = value[7].ToString(),
                            Phone = value[8].ToString(),
                            Location = value[10].ToString(),
                            IBAN = value[12].ToString().ToUpper()
                        };
                        merchantList.Add(merchant);
                    }
                }

                return merchantList;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }


        public async Task<int> GenerateAllEmails() //Generation rapports 
        {
            var nbGenerated = 0;
            try
            {
                var consoRepo = new ConsoRepo();
                var orderConso = await consoRepo.GetAllConso();

                var merchants = await GetAllMerchants();

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

                var merchants = await GetAllMerchants();
                var merchant = merchants.Where(m => m.Id == orderConso.First().Key).FirstOrDefault();

                await GenerateEmail(orderConso.First(), merchant);
                await TestListGmailLabels();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        private async Task<bool> GenerateEmail(IGrouping<long, OrderItemPair> group, Merchant merchant)
        {
            // Define parameters of request.           
            string plainText = "To:noemie.petignat@gmail.com\r\n" +
                               "Subject: Gmail Send API Test\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                               "<h1>TestGmail API Testing for sending <h1>";

            var newMsg = new Google.Apis.Gmail.v1.Data.Message();
            newMsg.Raw = Base64UrlEncode(plainText.ToString());
            gmailService.Users.Messages.Send(newMsg, "me").Execute();

            return true;
        }

        private async Task<bool> TestListGmailLabels()
        {
            try
            {
                //Add "GmailService.Scope.GmailReadonly" in scope

                // Define parameters of request.
                UsersResource.LabelsResource.ListRequest GMailrequest = gmailService.Users.Labels.List("me");

                // List labels.
                IList<Label> labels = GMailrequest.Execute().Labels;
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
