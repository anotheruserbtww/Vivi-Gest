using System.Linq;
using System.Web.Mvc;
using ViviGest.Services;
using ViviGest.Repositories;
using ViviGest.Dtos;
using ViviGest.Utilities;
using System;

namespace ViviGest.Controllers
{
    public class PagoController : Controller
    {
        private readonly PagoService _PagoService;

        public PagoController()
        {
            // Instanciación manual de repositorios y servicio
            var pagoRepo = new PagoRepository();
            var residenteRepo = new ResidenteRepository();
            _PagoService = new PagoService(pagoRepo, residenteRepo);
        }

        // GET: /Pago/Pendientes
        [AuthorizeRole(1, 3)]
        public ActionResult Pendientes()
        {
            var pendientes = _PagoService.ObtenerPagosPendientes();
            return View(pendientes);
        }

        // GET: /Pago/Detalle/5
        [AuthorizeRole(1, 3)]
        public ActionResult Detalle(int id)
        {
            var pago = _PagoService.ObtenerTodosLosPagos().FirstOrDefault(p => p.id_pago == id);
            if (pago == null) return HttpNotFound();

            var residente = _PagoService.ObtenerResidentePorId(pago.id_residente);
            ViewBag.Residente = residente;
            return View(pago);
        }

        // POST: /Pago/Confirmar/5
        [AuthorizeRole(3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmar(int id)
        {
            if (!_PagoService.ConfirmarPago(id))
            {
                TempData["Error"] = "No se pudo confirmar el pago.";
            }
            else
            {
                TempData["Success"] = "Pago confirmado correctamente.";
                // aquí podrías enviar el correo si quieres, usando lo que ya tienes
            }
            return RedirectToAction("Pendientes");
        }

        // GET: /Pago/Create
        [AuthorizeRole(1, 3)]
        public ActionResult Create()
        {
            // si tienes un ViewModel para crear pagos, lo cargas aquí
            return View();
        }

        // POST: /Pago/Create
        [AuthorizeRole(1, 3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagoDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            // Validar monto mínimo y máximo
            decimal montoMinimo = 125000m;
            decimal montoMaximo = 1000000m; // ajusta según tu negocio

            if (dto.monto < montoMinimo || dto.monto > montoMaximo)
            {
                ModelState.AddModelError("monto", $"El monto debe estar entre {montoMinimo:N0} y {montoMaximo:N0}.");
                return View(dto);
            }


            // Asignar la fecha actual antes de crear el pago
            dto.fecha_pago = DateTime.Now;

            var nuevoId = _PagoService.CrearPago(dto);
            if (nuevoId > 0)
            {
                TempData["Success"] = $"Pago creado con ID {nuevoId}";
                return RedirectToAction("Pendientes");
            }

            ModelState.AddModelError("", "Error al crear el pago.");
            return View(dto);
        }
    }
}
