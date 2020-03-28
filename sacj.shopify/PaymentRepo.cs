using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sacj.shopify
{
    public class PaymentRepo
    {
        public PaymentRepo()
        {

        }

        public async Task<List<Payment>> GetPayments()
        {
            var payments = new List<Payment>();
            try
            {
                using (var reader = new StreamReader("./Payments.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<PaymentMap>();
                    payments = csv.GetRecords<Payment>().ToList();
                    return payments;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }

    public sealed class PaymentMap : ClassMap<Payment>
    {
        public PaymentMap()
        {
            Map(p => p.Id).Name("Id");
            Map(p => p.Date).Name("Date");
            Map(p => p.IBAN).Name("IBAN");
            Map(p => p.Nom).Name("Nom");
            Map(p => p.Total).Name("Total");
        }
    }
}
