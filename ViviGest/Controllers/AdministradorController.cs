using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ViviGest.Dtos;
using ViviGest.Services;
using ViviGest.Repositories;      // para PagoRepository, ResidenteRepository
using ViviGest.Utilities;
using System.Web.Services.Description;

namespace ViviGest.Controllers
{
    [AuthorizeRole(3)]
    public class AdministradorController : Controller
    {

        private readonly UsuarioService _usuarioService;
        private readonly PagoService _pagoService;
        private readonly EmailConfigUtility _emailUtil;

        public AdministradorController()
        {
            var pagoRepo = new PagoRepository();
            var residenteRepo = new ResidenteRepository();
            _pagoService = new PagoService(pagoRepo, residenteRepo);

            _usuarioService = new UsuarioService();
            _emailUtil = new EmailConfigUtility();

        }



        // ─── PANEL PRINCIPAL ─────────────────────────────────────────────────
        public ActionResult Index()
        {
            var adminDto = new ViviGest.Dtos.usuariosDto
            {
                id_usuario = (int)Session["UserID"],
                nombres = Session["UserName"]?.ToString(),
                numero_documento = Session["UserDocumento"]?.ToString(),
                correo_electronico = Session["UserCorreo"]?.ToString(),
                // Puedes agregar más propiedades si las tienes
            };

            return View(adminDto);
        }
            
        

        // ─── USUARIOS ─────────────────────────────────────────────────────────

        // GET: /Administrador/IndexUsuario
        public ActionResult IndexUsuario()
        {
            var users = _usuarioService.GetAllUsuario();
            return View(users);
        }

        // GET: /Administrador/CreateUsuario
        public ActionResult CreateUsuario()
        {
            // Inicializamos con rol cliente por defecto, o quita id_rol si no aplica
            var dto = new usuariosDto { id_rol = 1 };
            return View(dto);
        }

        // POST: /Administrador/CreateUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsuario(usuariosDto newUser)
        {
            if (!ModelState.IsValid)
                return View(newUser);

            var resp = _usuarioService.CreateUser(newUser);
            if (resp.Response == 1)
                return RedirectToAction("IndexUsuario");

            ModelState.AddModelError("", resp.Message ?? "Error al crear el usuario.");
            return View(newUser);
        }

        


        // ─── PAGOS PENDIENTES ────────────────────────────────────────────────

        [HttpGet]
        public ActionResult ReportePagosPendientes()
        {
            var pagos = _pagoService.ObtenerPagosPendientes().ToList();
            return View(pagos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarPago(int idPago)
        {
            if (!_pagoService.ConfirmarPago(idPago))
            {
                TempData["Error"] = "No se pudo confirmar el pago.";
                return RedirectToAction("ReportePagosPendientes");
            }

            var pago = _pagoService.ObtenerTodosLosPagos()
                                   .First(p => p.id_pago == idPago);

            var pdfBytes = _emailUtil.GenerarPdfComprobante(pago);
            var residente = _pagoService.ObtenerResidentePorId(pago.id_residente);

            if (residente == null || string.IsNullOrEmpty(residente.correo_electronico))
            {
                TempData["Error"] = "No se encontró el correo del residente.";
                return RedirectToAction("ReportePagosPendientes");
            }

            _emailUtil.EnviarCorreo(
                residente.correo_electronico,
                "Su pago ha sido aprobado",
                $"Hola {residente.nombres}, tu pago #{idPago} ha sido aprobado.",
                pdfBytes,
                $"Comprobante_{idPago}.pdf"
            );

            TempData["Success"] = "Pago confirmado y comprobante enviado.";
            return RedirectToAction("ReportePagosPendientes");
        }


        // ─── REPORTE PDF POR FECHAS ─────────────────────────────────────────

        [HttpGet]
        public ActionResult ReportePagos()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarReportePagos(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                TempData["Error"] = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                return RedirectToAction("ReportePagos");
            }

            var pagos = _pagoService.ObtenerPagosPorFecha(fechaInicio, fechaFin).ToList();
            if (!pagos.Any())
            {
                TempData["Error"] = "No hay pagos en el rango seleccionado.";
                return RedirectToAction("ReportePagos");
            }

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Logo
                var logo = Image.GetInstance(Server.MapPath("~/Content/images/logo.png"));
                logo.ScaleAbsolute(100, 100);
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);

                var tf = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                doc.Add(new Paragraph("Reporte de Pagos - ViviGest", tf) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph($"Desde: {fechaInicio:dd/MM/yyyy} Hasta: {fechaFin:dd/MM/yyyy}\n\n"));

                var table = new PdfPTable(5) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 10f, 15f, 20f, 25f, 20f });

                var hf = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                foreach (var col in new[] { "ID Pago", "Residente", "Monto", "Método", "Fecha" })
                    table.AddCell(new PdfPCell(new Phrase(col, hf)) { HorizontalAlignment = Element.ALIGN_CENTER });

                var cf = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                bool alt = false;
                foreach (var p in pagos)
                {
                    var bg = alt ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    table.AddCell(new PdfPCell(new Phrase(p.id_pago.ToString(), cf)) { BackgroundColor = bg });
                    table.AddCell(new PdfPCell(new Phrase(p.id_residente.ToString(), cf)) { BackgroundColor = bg });
                    table.AddCell(new PdfPCell(new Phrase(p.monto.ToString("C2"), cf)) { BackgroundColor = bg });
                    table.AddCell(new PdfPCell(new Phrase(p.metodo_pago, cf)) { BackgroundColor = bg });
                    table.AddCell(new PdfPCell(new Phrase(p.fecha_pago.ToString("dd/MM/yyyy"), cf)) { BackgroundColor = bg });
                    alt = !alt;
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "Reporte_Pagos.pdf");
            }
        }
    }
}
