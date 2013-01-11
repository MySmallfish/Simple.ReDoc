using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Simple;

namespace ReDoc.Controllers
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            SystemMonitor.Info("HERE!");
            filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            string rqstMethod = HttpContext.Current.Request.Headers["Access-Control-Request-Method"];
            if (rqstMethod == "OPTIONS" || rqstMethod == "POST")
            {
                filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With, Accept, Access-Control-Allow-Origin, Content-Type");
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class HomeController : Controller
    {


        private IEnumerable<ReportParameter> CreateReportParameters(AgentInfo agentInfo)
        {
            var parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("LogoUrl", agentInfo.ImageUrl));
            parameters.Add(new ReportParameter("LogoName", agentInfo.LogoName));
            parameters.Add(new ReportParameter("AgentName", agentInfo.FullName));
            parameters.Add(new ReportParameter("AgentIdNumber", agentInfo.IdNumber));
            parameters.Add(new ReportParameter("AgentPhone", agentInfo.Phone));
            parameters.Add(new ReportParameter("AgentAddress", agentInfo.Address));
            parameters.Add(new ReportParameter("AgentEmail", agentInfo.Email));
            parameters.Add(new ReportParameter("AgentCertificateNumber", agentInfo.CertificateNumber));



            return parameters.AsReadOnly();
        }

        public void Render(string reportDesign, ReportDataSource[] dataSources, string destFile, IEnumerable<ReportParameter> parameters = null)
        {
            var localReport = new LocalReport();

            using (var reportDesignStream = System.IO.File.OpenRead(reportDesign))
            {
                localReport.LoadReportDefinition(reportDesignStream);
            }
            localReport.EnableExternalImages = true;
            localReport.EnableHyperlinks = true;

            if (parameters != null)
            {
                localReport.SetParameters(parameters);
            }
            foreach (var reportDataSource in dataSources)
            {
                localReport.DataSources.Add(reportDataSource);
            }

            //Export to PDF
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;
            Warning[] warnings;
            var content = localReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            System.IO.File.WriteAllBytes(destFile, content);
        }

        public class Agreement
        {
            public string UniqueId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public string CustomerCity { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerIdNumber { get; set; }
            public string DealType { get; set; }
            public double? PercentsRate { get; set; }
            public double? AmountRate { get; set; }
            public string Signature { get; set; }
            public bool IsExclusive { get; set; }
            public string PropertyType { get; set; }
            public string PropertyCity { get; set; }
            public string PropertyAddress { get; set; }
            public string AgreementDate { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        public class AgentInfo
        {
            public string ImageUrl { get; set; }
            public string LogoName { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Phone { get; set; }
            public string IdNumber { get; set; }
            public string CertificateNumber { get; set; }
        }


        private static AgentInfo GetAgentInfo()
        {
            var agentInfo = new AgentInfo()
            {
                ImageUrl = "http://me.5115.us/Safety/ReDoc/Images/Guy.png",
                LogoName = "ג.י.א שיווק נדל\"ן",
                FullName = "גיא אברהמי",
                IdNumber = "25156175",
                CertificateNumber = "15980",
                Address = "חבקוק 42, פתח תקווה",
                Phone = "050-7116850",
                Email = "guyal4@gmail.com"
            };
            return agentInfo;
        }
        public ActionResult PropertyAgreement(int id)
        {
            var agreement = new Agreement()
            {
                CustomerName = "דוד אזולאי",
                CustomerAddress = "השקמה 10, פתח תקווה",
                CustomerPhone = "052-5233366",
                CustomerIdNumber = "040022534",
                PercentsRate = 1.5
            };
            var destFile = RenderReport(GetAgentInfo(), agreement);

            return File(destFile, "application/pdf");
        }

        private const string PropertyAgreementUrl = "~/Output/PropertyAgreements";
        private string GetPropertyAgreementsOutputPath()
        {
            return Server.MapPath(PropertyAgreementUrl);
        }

        [HttpPost]
        [AllowCrossSiteJsonAttribute]
        public ActionResult PropertyAgreement(AgentInfo agent, Agreement agreement)
        {
            try
            {
                SystemMonitor.Info("Sending property agreement from agent '{0}' to customer '{1}'.", agent.FullName, agreement.CustomerName);
                
                agreement.Signature = CreateSignature(agreement);
                
                SystemMonitor.Debug("Signature created, url: {0}", agreement.Signature);

                var destFile = GenerateReport(agent, agreement);

                SystemMonitor.Debug("Report generated, file name: {0}", System.IO.Path.GetFileName(destFile));

                SendByMail(agent, agreement, destFile);
            }
            catch (Exception anyException)
            {
                SystemMonitor.Error(anyException, "Error Creating agreement report");
                return new HttpStatusCodeResult(500);
            }
            return new HttpStatusCodeResult(200);
        }

        private string GenerateReport(AgentInfo agent, Agreement agreement)
        {
            var destFile = RenderReport(agent, agreement);
            var reportFilePath = string.Format((agreement.IsExclusive ? "Exclusive-" : "") + "Agreement.{0}[{1}].pdf",
                                               DateTime.Now.ToString("dd-MM-yyyy HH-mm"), agreement.UniqueId);
            var outputPath = GetPropertyAgreementsOutputPath();
            reportFilePath = Path.Combine(outputPath, reportFilePath);
            System.IO.File.Copy(destFile, reportFilePath, true);
            return destFile;
        }

        private static void SendByMail(AgentInfo agent, Agreement agreement, string destFile)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          try
                                          {
                                              SystemMonitor.Info("Sending Email to: {0}, {1}", agent.Email,
                                                                 agreement.CustomerEmail);
                                              SendEmail(agent, agreement, destFile);
                                          }
                                          catch (Exception anyException)
                                          {
                                              SystemMonitor.Error(anyException, "Failed to send agreement by mail.");
                                          }
                                      });
        }

        private string CreateSignature(Agreement agreement)
        {
            var sigToImage = new SignatureToImage()
                                 {
                                     CanvasWidth = 200,
                                     CanvasHeight = 120
                                 };
            var signatureUrl = "~/Output/Signatures/" + agreement.UniqueId + ".gif";

            CreateSignatureBitmap(agreement, sigToImage, signatureUrl);
            signatureUrl = Url.Content(signatureUrl);
            agreement.Signature = signatureUrl;
            return signatureUrl;
        }


        private void CreateSignatureBitmap(Agreement agreement, SignatureToImage sigToImage, string signatureUrl)
        {
            var bitmap = sigToImage.SigJsonToImage(agreement.Signature);
            var targetPath = Server.MapPath(signatureUrl);
            bitmap.Save(targetPath, ImageFormat.Gif);
        }

        public static string FullyQualifiedApplicationPath
        {
            get
            {
                //Return variable declaration
                var appPath = string.Empty;

                //Getting the current context of HTTP request
                var context = System.Web.HttpContext.Current;

                //Checking the current context content
                if (context != null)
                {
                    //Formatting the fully qualified website url/name
                    appPath = string.Format("{0}://{1}{2}{3}",
                      context.Request.Url.Scheme,
                      context.Request.Url.Host,
                      context.Request.Url.Port == 80
                        ? string.Empty : ":" + context.Request.Url.Port,
                      context.Request.ApplicationPath);

                    if (!appPath.EndsWith("/"))
                    {
                        appPath += "/";
                    }
                }

                return appPath;
            }
        }



        private static void SendEmail(AgentInfo agent, Agreement agreement, string destFile)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("redoc@simplesoftware.co.il", agent.FullName);
            mailMessage.Sender = new MailAddress("redoc@simplesoftware.co.il", "Simple. ReDoc");
            mailMessage.To.Add(new MailAddress(agent.Email, agent.FullName, Encoding.UTF8));
            mailMessage.To.Add(new MailAddress("mysmallfish@gmail.com", "Simple. ReDoc"));
            if (!string.IsNullOrEmpty(agreement.CustomerEmail))
            {
                try
                {
                    mailMessage.To.Add(new MailAddress(agreement.CustomerEmail, agreement.CustomerName, Encoding.UTF8));
                }
                catch (Exception anyException)
                {
                    SystemMonitor.Error(anyException, "Error creating customer mail address");
                }
            }
            mailMessage.Subject = "הסכם לשירותי תיווך מאת - " + agent.FullName;
            mailMessage.Body = "מצורף בזאת הסכם לשירותי תיווך";
            var attachment = new Attachment(destFile, new ContentType("application/pdf"))
                                 {
                                     Name = "הסכם שירותי תיווך - " + DateTime.Now.ToShortDateString() + ".pdf"
                                 };
            mailMessage.Attachments.Add(attachment);

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("mysmallfish", "Smallfish00");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }



        private string RenderReport(AgentInfo agent, Agreement agreement)
        {
            var destFile = Path.GetTempFileName() + ".pdf";
            var designFile = Server.MapPath("~/Static/Reports/" + (agreement.IsExclusive ? "Exclusive":"") +"PropertyAgreement.rdlc");

            var parameters = CreateReportParameters(agent);

            Render(designFile, 
                new ReportDataSource[]
                    {
                        new ReportDataSource("Agreement", new[] {agreement})
                    }, destFile,
                   parameters);
            return destFile;
        }


        //[Authorize]
        public ActionResult Index()
        {
            
            var path = new DirectoryInfo(GetPropertyAgreementsOutputPath());
            var rx = new Regex(@"(.*)\[.*\]\.(.*)");
            var propertyAgreements = path.GetFiles("*.pdf").OrderByDescending(file=>file.CreationTime).Select(file => new PropertyAgreementModel()
                                                                               {
                                                                                   Url = Url.Content(string.Format("{0}/{1}", PropertyAgreementUrl, file.Name)),
                                                                                   Name = rx.Replace(file.Name, "$1.$2"),
                                                                                   Date = file.CreationTime.ToString("dd/MM/yyyy HH:mm")
                                                                               });

            return View(propertyAgreements);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
