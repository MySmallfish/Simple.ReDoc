﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace ReDoc.Controllers
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
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
        
        [HttpPost]
        public ActionResult SendPropertyAgreement(AgentInfo agent, Agreement agreement)
        {
            try
            {
                var sigToImage = new SignatureToImage()
                                     {
                                         CanvasWidth = 200,
                                         CanvasHeight =120
                                     };
                var bitmap = sigToImage.SigJsonToImage(agreement.Signature);
                var signatureUrl = "~/Signatures/" + agreement.UniqueId + ".gif";
                var targetPath = Server.MapPath(signatureUrl);
                bitmap.Save(targetPath, ImageFormat.Gif);
                agreement.Signature = FullyQualifiedApplicationPath + Url.Content(signatureUrl);
                var destFile = RenderReport(agent, agreement);
                SendEmail(agent, destFile);
            }
            catch (Exception anyException)
            {
                System.IO.File.AppendAllLines(Server.MapPath("~/log.txt"),new[]{ anyException.ToString()});
                return new HttpStatusCodeResult(500);
            }
            return new HttpStatusCodeResult(200);
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



        private static void SendEmail(AgentInfo agent, string destFile)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("redoc@simplesoftware.co.il", agent.FullName);
            mailMessage.Sender = new MailAddress("redoc@simplesoftware.co.il", "Simple. ReDoc");
            mailMessage.To.Add(new MailAddress(agent.Email, agent.FullName));
            mailMessage.To.Add(new MailAddress("mysmallfish@gmail.com", "Simple. ReDoc"));
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

        private string RenderReport(AgentInfo agent, Agreement agreement)
        {
            var destFile = Path.GetTempFileName() + ".pdf";
            var designFile = Server.MapPath("~/Static/Reports/PropertyAgreement.rdlc");

            var parameters = CreateReportParameters(agent);

            Render(designFile, 
                new ReportDataSource[]
                    {
                        new ReportDataSource("Agreement", new[] {agreement})
                    }, destFile,
                   parameters);
            return destFile;
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

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
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
