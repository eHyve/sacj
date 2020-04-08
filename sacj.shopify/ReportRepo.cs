using HandlebarsDotNet;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sacj.shopify
{
    public class ReportRepo
    {

        public async Task<int> GenerateAllReports() //Generation rapports 
        {
			var nbGenerated = 0;
			try
			{
				var consoRepo = new ConsoRepo();
				var orderConso = await consoRepo.GetAllConso();

                var googleRepo = new GoogleRepo();
                var merchants = await googleRepo.GetAllMerchants();

                var paymentsRepo = new PaymentRepo();
                var payments = await paymentsRepo.GetPayments();

                foreach(var group in orderConso)
                {
                    var merchant = merchants.Where(m => m.Id == group.Key).FirstOrDefault();
                    var paymentsList = payments.Where(p => p.Id == group.Key).ToList();
                    nbGenerated += await GenerateStatement(group, merchant, paymentsList) ? 1 : 0;
                }

                return nbGenerated;
			}
			catch (Exception ex)
			{
				return nbGenerated;
			}
        }

        public async Task<int> GenerateReportsById(long id)
        {
            try
            {
                var consoRepo = new ConsoRepo();
                var orderConso = await consoRepo.GetConsoByProductId(id);

                var googleRepo = new GoogleRepo();
                var merchants = await googleRepo.GetAllMerchants();
                var merchant = merchants.Where(m => m.Id == orderConso.First().Key).FirstOrDefault();

                var paymentsRepo = new PaymentRepo();
                var payments = await paymentsRepo.GetPayments();
                var paymentsList = payments.Where(p => p.Id == orderConso.First().Key).ToList();

                await GenerateStatement(orderConso.First(), merchant, paymentsList);
                
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private async Task<bool> GenerateStatement(IGrouping<long, OrderItemPair> group, Merchant merchant, List<Payment> payments) //Generate PDF
        {
            try
            {
                //TODO: FIX HERE for gift total, calculate on gift cards and not items
                var giftTotal = group.Select(g => g).ToList().Sum(o => decimal.Parse(o.Item.price) * o.Item.quantity);
                var paymentsTotal = payments.Sum(p => p.Total);
                
                var reportData = new {
                    merchant = merchant,
                    date = DateTime.Now.ToString("dd.MM.yyyy"),
                    merchandId = group.Key,
                    items = group.Select(g => g).ToList(),
                    payments = payments,
                    giftTotal = giftTotal,
                    displayPayments = payments.Count() > 0,
                    paymentsTotal = paymentsTotal,
                    balance = giftTotal - paymentsTotal
                };

                var renderer = new HtmlToPdf();

                // Basic settings
                renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
                renderer.PrintOptions.MarginTop = 10;  //millimeters
                renderer.PrintOptions.MarginBottom = 24;
                renderer.PrintOptions.MarginLeft = 17;
                renderer.PrintOptions.MarginRight = 17;
                renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

                //Header
                renderer.PrintOptions.Header = new HtmlHeaderFooter()
                {
                    DrawDividerLine = true,
                    HtmlFragment = "<img src='logo.jpg' height='40' ><div style='float:right;padding-top:15px;'><b>Action de soutien aux commerçants jurassiens</b></div>",
                    Height = 20
                };

                // Footer
                renderer.PrintOptions.Footer = new SimpleHeaderFooter()
                {
                    LeftText = "Imprimé {date} - {time}",
                    RightText = "Page {page}/{total-pages}",
                    DrawDividerLine = true,
                    FontSize = 10
                };

                Handlebars.RegisterHelper("formatDate", (writer, options, context, parameters) =>
                {
                    if (parameters.Length != 2)
                    {
                        writer.Write("formatDate:Wrong number of arguments");
                        return;
                    }
                    var dateFormatted = DateTime.Parse(parameters[0].ToString()).ToString(parameters[1].ToString());
                    writer.WriteSafeString(dateFormatted);
                });

                string source = File.ReadAllText(@"./Statement.html");

                var template = Handlebars.Compile(source);
                var content = template(reportData);
                var document = await renderer.RenderHtmlAsPdfAsync(content);

                document.SaveAs("./Statements/" + group.First().Item.title.Replace('/','-').Replace(':', '-').Replace("\"","") + "_" + group.Key + ".pdf");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
