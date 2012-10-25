using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace ReDoc.Controllers
{
    public class HomeController : Controller
    {


        private IEnumerable<ReportParameter> CreateReportParameters(AgentInfo agentInfo)
        {
            var parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("LogoUrl", agentInfo.LogoUrl));
            parameters.Add(new ReportParameter("LogoName", agentInfo.LogoName));
            parameters.Add(new ReportParameter("AgentName", agentInfo.Name));
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
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerIdNumber { get; set; }
            public double PercentsRate { get; set; }
        }

        public class AgentInfo
        {
            public string LogoUrl { get; set; }
            public string LogoName { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string IdNumber { get; set; }
            public string CertificateNumber { get; set; }
        }
        [HttpPost]
        public ActionResult SendPropertyAgreement(Agreement agreement)
        {
            //var agreement = new Agreement()
            //{
            //    CustomerName = "דוד אזולאי",
            //    CustomerAddress = "השקמה 10, פתח תקווה",
            //    CustomerPhone = "052-5233366",
            //    CustomerIdNumber = "040022534",
            //    PercentsRate = 1.5
            //};
            var destFile = RenderReport(agreement);
            var agentInfo = GetAgentInfo();
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("mysmallfish@gmail.com", agentInfo.Name);
            mailMessage.Sender = new MailAddress("mysmallfish@gmail.com", "Simple. ReDoc");
            //mailMessage.To.Add(new MailAddress(agentInfo.Email, agentInfo.Name));
            mailMessage.To.Add(new MailAddress("mysmallfish@gmail.com", "Simple. ReDoc"));
            mailMessage.Subject = "הסכם לשירותי תיווך מאת - " + agentInfo.Name;
            mailMessage.Body = "מצורף בזאת הסכם לשירותי תיווך";
            var attachment = new Attachment(destFile, new ContentType("application/pdf"))
                                 {
                                     Name="הסכם שירותי תיווך - " +DateTime.Now.ToShortDateString() + ".pdf"
                                 };
            mailMessage.Attachments.Add(attachment);

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("mysmallfish", "Smallfish00");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);

            return new HttpStatusCodeResult(200);
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
            var destFile = RenderReport(agreement);

            return File(destFile, "application/pdf");
        }

        private string RenderReport(Agreement agreement)
        {
            var destFile = Path.GetTempFileName() + ".pdf";
            var designFile = Server.MapPath("~/Static/Reports/PropertyAgreement.rdlc");

            var agentInfo = GetAgentInfo();
            var parameters = CreateReportParameters(agentInfo);

            Render(designFile, new ReportDataSource[] {new ReportDataSource("Agreement", new[] {agreement})}, destFile,
                   parameters);
            return destFile;
        }

        private static AgentInfo GetAgentInfo()
        {
            var agentInfo = new AgentInfo()
                                {
                                    LogoUrl = "http://localhost:61196/ReDoc/Images/Guy.png",
                                    LogoName = "ג.י.א שיווק נדל\"ן",
                                    Name = "גיא אברהמי",
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
