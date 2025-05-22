using System.Linq;
using System.Web.Mvc;
using ViviGest.Services;
using ViviGest.Repositories;
using ViviGest.Dtos;

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
        public ActionResult Pendientes()
        {
            var pendientes = _PagoService.ObtenerPagosPendientes();
            return View(pendientes);
        }

        // GET: /Pago/Detalle/5
        public ActionResult Detalle(int id)
        {
            var pago = _PagoService.ObtenerTodosLosPagos().FirstOrDefault(p => p.id_pago == id);
            if (pago == null) return HttpNotFound();

            var residente = _PagoService.ObtenerResidentePorId(pago.id_residente);
            ViewBag.Residente = residente;
            return View(pago);
        }

        // POST: /Pago/Confirmar/5
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
        public ActionResult Create()
        {
            // si tienes un ViewModel para crear pagos, lo cargas aquí
            return View();
        }

        // POST: /Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

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
