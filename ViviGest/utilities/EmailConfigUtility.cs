using System.Net;
using System.Net.Mail;
using ViviGest.Dtos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;

namespace ViviGest.Utilities
{
    public class EmailConfigUtility
    {
        private readonly SmtpClient cliente;
        private const string Host = "smtp.gmail.com";
        private const int Port = 587;
        private const string User = "gestvivi@gmail.com";
        private const string Password = "dlzmlvzmaqvrlyld   ";

        public EmailConfigUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(User, Password),
                EnableSsl = true
            };
        }

        public SmtpClient Cliente => cliente;

        public void EnviarCorreo(string destinatario, string asunto, string cuerpoHtml, byte[] adjunto = null, string adjNombre = null)
        {
            var mail = new MailMessage(User, destinatario, asunto, cuerpoHtml)
            {
                IsBodyHtml = true
            };
            if (adjunto != null && !string.IsNullOrEmpty(adjNombre))
                mail.Attachments.Add(new Attachment(new MemoryStream(adjunto), adjNombre));
            cliente.Send(mail);
        }



        public byte[] GenerarPdfComprobante(PagoDto pago)
        {
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A5, 40, 40, 50, 50); 
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Logo centrado arriba
                var logoPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/images/"), "logo.png");
                if (File.Exists(logoPath))
                {
                    var logo = Image.GetInstance(logoPath);
                    logo.ScaleToFit(120f, 120f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }

                doc.Add(new Paragraph("\n"));

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                var title = new Paragraph("Comprobante de Pago Aprobado", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 25f
                };
                doc.Add(title);

                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.DARK_GRAY);
                var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(2)
                {
                    WidthPercentage = 90,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    SpacingBefore = 15f,
                    SpacingAfter = 30f
                };
                table.SetWidths(new float[] { 1f, 2f });

                void AddRow(string label, string value)
                {
                    var labelCell = new PdfPCell(new Phrase(label, labelFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        PaddingBottom = 6,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    var valueCell = new PdfPCell(new Phrase(value, valueFont))
                    {
                        Border = Rectangle.NO_BORDER,
                        PaddingBottom = 6,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        BackgroundColor = BaseColor.WHITE
                    };
                    table.AddCell(labelCell);
                    table.AddCell(valueCell);
                }

                AddRow("ID Pago:", pago.id_pago.ToString());
                AddRow("ID Residente:", pago.id_residente.ToString());
                AddRow("Monto:", pago.monto.ToString("C2"));
                AddRow("Método de Pago:", pago.metodo_pago);
                AddRow("Fecha de Pago:", pago.fecha_pago.ToString("dd/MM/yyyy HH:mm"));

                doc.Add(table);

                
                var noteFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10, BaseColor.DARK_GRAY);
                var nota = new Paragraph("Nota: Este comprobante es válido como constancia de pago. " +
                    "Conserve este documento para cualquier trámite futuro relacionado con su administración.", noteFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 40f
                };
                doc.Add(nota);

               
                var cb = writer.DirectContent;
                cb.SetLineWidth(0.5f);
                cb.SetColorStroke(BaseColor.GRAY);
                cb.MoveTo(doc.LeftMargin, doc.BottomMargin - 10);
                cb.LineTo(doc.PageSize.Width - doc.RightMargin, doc.BottomMargin - 10);
                cb.Stroke();

                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                    new Phrase("ViviGest - Administración de Condominios | www.vivigest.com", footerFont),
                    doc.PageSize.Width / 2, doc.BottomMargin - 25, 0);

                doc.Close();
                return ms.ToArray();
            }
        }

    }
}

