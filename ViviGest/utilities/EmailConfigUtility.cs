using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using GastroByte.Dtos; 

namespace GastroByte.Utilities
{
    public class EmailConfigUtility
    {
    //    private SmtpClient cliente;
    //    private MailMessage email;
    //    private string Host = "smtp.gmail.com";
    //    private int Port = 587;
    //    private string User = "gastrobytesoftware@gmail.com";
    //    private string Password = "deaolobgjvdbpsrk";
    //    private bool EnabledSSL = true;

    //    public EmailConfigUtility()
    //    {
    //        cliente = new SmtpClient(Host, Port)
    //        {
    //            EnableSsl = EnabledSSL,
    //            DeliveryMethod = SmtpDeliveryMethod.Network,
    //            UseDefaultCredentials = false,
    //            Credentials = new NetworkCredential(User, Password)
    //        };
    //    }

    //    public void EnviarCorreoConPDF(string destinatario, string asunto, string mensaje, ReservaDto reserva)
    //    {
    //        var pdfData = GenerarPDF(reserva);
    //        email = new MailMessage(User, destinatario, asunto, mensaje)
    //        {
    //            IsBodyHtml = true
    //        };
    //        email.Attachments.Add(new Attachment(new MemoryStream(pdfData), "Reserva_Gastrobyte.pdf"));
    //        cliente.Send(email);
    //    }

    //    private byte[] GenerarPDF(ReservaDto reserva)
    //    {
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            Document doc = new Document(PageSize.A5, 20, 20, 30, 30);
    //            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
    //            writer.PageEvent = new PDFHeaderFooter();
    //            doc.Open();

    //            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
    //            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

    //            // Título y subtítulo
    //            Paragraph header = new Paragraph("GastroByte - Confirmación de Reserva", titleFont)
    //            {
    //                Alignment = Element.ALIGN_CENTER
    //            };
    //            doc.Add(header);

    //            Paragraph subtitle = new Paragraph("¡Gracias por reservar en Gastrobyte!", normalFont)
    //            {
    //                Alignment = Element.ALIGN_CENTER,
    //                SpacingAfter = 10
    //            };
    //            doc.Add(subtitle);

    //            doc.Add(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1));

    //            // Tabla de detalles de la reserva
    //            PdfPTable table = new PdfPTable(2);
    //            table.WidthPercentage = 90;
    //            table.SetWidths(new float[] { 1, 2 });

    //            AgregarFila(table, "Fecha de la Reserva:", "\n"+ reserva.fecha.ToString("yyyy-MM-dd"), normalFont);
    //            AgregarFila(table, "Hora de la Reserva:", reserva.hora, normalFont);
    //            AgregarFila(table, "Número de Personas:", "\n" + reserva.numero_personas, normalFont);
    //            AgregarFila(table, "Cliente:", reserva.nombre, normalFont);
    //            AgregarFila(table, "Número de Documento:", "\n" + reserva.documento, normalFont);

    //            doc.Add(table);

    //            doc.Add(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1));

    //            // Mensaje de agradecimiento
    //            Paragraph agradecimiento = new Paragraph("Nos alegra tenerte con nosotros en Gastrobyte. Te esperamos para una experiencia gastronómica inolvidable.", normalFont)
    //            {
    //                Alignment = Element.ALIGN_CENTER,
    //                SpacingBefore = 20
    //            };

                
    //            doc.Add(agradecimiento);


    //            doc.Add(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -5));
    //            // Información adicional
    //            Paragraph info = new Paragraph("Política de Cancelación:\nSi necesitas cancelar o modificar tu reserva, por favor contáctanos con al menos 24 horas de anticipación.", normalFont)
    //            {
    //                Alignment = Element.ALIGN_LEFT,
    //                SpacingBefore = 20
    //            };
    //            doc.Add(info);

    //            doc.Add(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -5));

    //            // Información de contacto
    //            Paragraph contacto = new Paragraph("Contacto:\nTeléfono: +57 322 4080792\nEmail: contacto@gastrobyte.com\nDirección: Calle 123 #45-67, Bogotá, Colombia", normalFont)
    //            {
    //                Alignment = Element.ALIGN_LEFT,
    //                SpacingBefore = 20
    //            };
    //            doc.Add(contacto);
                

    //            doc.Close();
    //            return memoryStream.ToArray();
    //        }
    //    }

    //    private void AgregarFila(PdfPTable table, string campo, string valor, Font font)
    //    {
    //        PdfPCell cellCampo = new PdfPCell(new Phrase(campo, font))
    //        {
    //            Border = Rectangle.NO_BORDER,
    //            PaddingBottom = 5,
    //            HorizontalAlignment = Element.ALIGN_LEFT
    //        };
    //        table.AddCell(cellCampo);

    //        PdfPCell cellValor = new PdfPCell(new Phrase(valor, font))
    //        {
    //            Border = Rectangle.NO_BORDER,
    //            PaddingBottom = 5,
    //            HorizontalAlignment = Element.ALIGN_LEFT
    //        };
    //        table.AddCell(cellValor);
    //    }

    //    private class PDFHeaderFooter : PdfPageEventHelper
    //    {
    //        public override void OnEndPage(PdfWriter writer, Document document)
    //        {
    //            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);
    //            PdfPTable footerTable = new PdfPTable(1)
    //            {
    //                TotalWidth = document.PageSize.Width - 40
    //            };
    //            footerTable.DefaultCell.Border = Rectangle.NO_BORDER;
    //            footerTable.AddCell(new PdfPCell(new Phrase("Gracias por elegir Gastrobyte. ¡Nos vemos pronto!", footerFont))
    //            {
    //                Border = Rectangle.NO_BORDER,
    //                HorizontalAlignment = Element.ALIGN_CENTER
    //            });
    //            footerTable.WriteSelectedRows(0, -1, 20, document.Bottom - 10, writer.DirectContent);
    //        }
    //    }
    }
}
