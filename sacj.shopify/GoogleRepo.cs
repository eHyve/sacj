
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

namespace sacj.shopify
{
    public class GoogleRepo
    {
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private UserCredential credential;
        private SheetsService sheetService;

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
                ApplicationName = "SACJ 1.0",
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
    }
}
