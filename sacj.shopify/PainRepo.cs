using CsvHelper;
using CsvHelper.Configuration;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sacj.shopify
{
    public class PainRepo
    {

        public PainRepo()
        {

        }

        public async Task<int> GeneratePAINFile()
        {
            try
            {
                var painItems = GetPainItems();

                var paymentItems = painItems.Select(p => { return new { 
                    guid = Guid.NewGuid().ToString().Replace("-",""),
                    executionDate = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd"),
                    IBAN = p.IBAN.ToUpper().Replace(" ",""),
                    BIC = p.BIC,
                    name = p.Name,
                    address = p.Address,
                    amount = p.Amount,
                    transfertId = Guid.NewGuid().ToString().Replace("-", ""),
                    e2eId = Guid.NewGuid().ToString().Replace("-", "")
                }; }).ToList();

                var paymentData = new {
                    uniqueId = Guid.NewGuid().ToString().Replace("-", ""),
                    creationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"),
                    itemNb = painItems.Count(),
                    ctrlSum = painItems.Sum(p => p.Amount),
                    paymentItems = paymentItems
                };

                string source = File.ReadAllText(@"./PainTemplate.xml", Encoding.UTF8);
                var template = Handlebars.Compile(source);
                var content = template(paymentData);

                using (StreamWriter sw = new StreamWriter(new FileStream($"./Pain/{DateTime.Now.ToString("yyyyMMddhhmmss")}.xml", FileMode.CreateNew, FileAccess.ReadWrite), Encoding.UTF8))
                {
                    sw.Write(content);
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<PainItem> GetPainItems()
        {
            var painItems = new List<PainItem>();
            try
            {
                using (var reader = new StreamReader("./Pain.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<PaymentMap>();
                    csv.Configuration.Encoding = Encoding.UTF8;
                    painItems = csv.GetRecords<PainItem>().ToList();
                    return painItems;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

    public sealed class PainItemMap : ClassMap<PainItem>
    {
        public PainItemMap()
        {
            Map(p => p.Id).Name("Id");
            Map(p => p.IBAN).Name("IBAN");
            Map(p => p.BIC).Name("BIC");
            Map(p => p.Name).Name("Name");
            Map(p => p.Address).Name("Address");
            Map(p => p.Amount).Name("Amount");
        }
    }
}
