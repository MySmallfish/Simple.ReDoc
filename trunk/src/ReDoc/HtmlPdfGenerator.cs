using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.util;
using System.Net;
using System.Xml;

namespace ReDoc
{
    public class HtmlPdfGenerator
    {
        public void ConvertToPdf(string sourceFile, string targetFile)
        {
            //HttpContext context = HttpContext.Current;

            using (var reader = new StreamReader(sourceFile, Encoding.UTF8))
            {
                //Create PDF document 
                var document = new Document(PageSize.A4);
                var parser = new HTMLWorker(document);
                using (var pdfStream = new FileStream(targetFile, FileMode.Create))
                {
                    PdfWriter.GetInstance(document, pdfStream);

                    document.Open();
                    
                    try
                    {
                        parser.Parse(reader);
                    }
                    catch (Exception ex)
                    {
                        //Display parser errors in PDF. 
                        var paragraph = new Paragraph("Error!" + ex.Message);
                        var text = paragraph.Chunks[0] as Chunk;
                        if (text != null)
                        {
                            text.Font.Color = BaseColor.RED;
                        }
                        document.Add(paragraph);
                    }
                    finally
                    {
                        document.Close();
                    }

                }
            }





        }
    }
}