using System;
using System.Linq;
using System.Web.Mvc;
using ViviGest.Dtos;
using ViviGest.Repositories;
using ViviGest.Services;

namespace ViviGest.Controllers
{
    public class ResidenteController : Controller
    {
        private readonly ResidenteService _residenteService;
        private readonly PagoService _pagoService;
        private readonly CorrespondenciaService _correspondenciaService;


        public ResidenteController()
        {
            _correspondenciaService = new CorrespondenciaService();
            var pagoRepository = new PagoRepository();
            var residenteRepository = new ResidenteRepository();
            _pagoService = new PagoService(pagoRepository, residenteRepository);

        }



        public ActionResult Correspondencia()
        {
            int idUsuario = (int)Session["UserID"];
            var correspondencia = _correspondenciaService.ObtenerCorrespondenciaPorUsuario(idUsuario);
            return View(correspondencia);
        }

        [HttpPost]
        public ActionResult MarcarEntregado(int idCorrespondencia)
        {
            bool exito = _correspondenciaService.MarcarEntregado(idCorrespondencia);
            if (exito)
                TempData["Success"] = "Correspondencia marcada como entregada.";
            else
                TempData["Error"] = "No se pudo actualizar el estado.";

            return RedirectToAction("Correspondencia");
        }

        public ActionResult CrearPago()
        {
            return View(new PagoDto());
        }

        // POST: crear pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPago(PagoDto pago)
        {
            pago.id_residente = (int)Session["UserID"];
            pago.fecha_pago = DateTime.Now;
            pago.estado = "Pendiente";
            var idNuevoPago = _pagoService.CrearPago(pago);
            if (idNuevoPago > 0)
            {
                TempData["Success"] = "Pago registrado correctamente";
                return RedirectToAction("MisPagos");
            }

            {
                TempData["Error"] = "Error al registrar pago";
                return View(pago);
            }

            // Obtener id residente (simulado o de sesión)
            // Aquí lo debes adaptar a tu lógica de autenticación
            pago.id_residente = (int)Session["UserID"];

            if (Session["UserID"] == null)
            {
                // No hay usuario logueado, redirige a login o muestra error
                return RedirectToAction("Login", "Usuario");
            }

            pago.id_residente = (int)Session["UserID"];
            pago.fecha_pago = DateTime.Now;
            pago.estado = "Pendiente";

            int nuevoId = _pagoService.CrearPago(pago);

            if (nuevoId > 0)
            {
                TempData["Success"] = "Pago creado correctamente.";
                return RedirectToAction("MisPagos");
            }
            else
            {
                ModelState.AddModelError("", "Error al crear el pago.");
                return View(pago);
            }
        }

        // Listar pagos del residente
        public ActionResult MisPagos()
        {
            if (Session["UserID"] == null)
            {
                // Usuario no autenticado, redirigir al login
                return RedirectToAction("Login", "Usuario");
            }

            int idResidente = (int)Session["UserID"];
            var todosPagos = _pagoService.ObtenerTodosLosPagos()
                               .Where(p => p.id_residente == idResidente)
                               .ToList();

            return View(todosPagos);
        }



        // GET: Residente
        public ActionResult Index()
        {
                var residentes = _residenteService.ObtenerTodos();
            return View(residentes);
        }

        // GET: Residente/Details/5
        public ActionResult Details(int id)
        {
            var residente = _residenteService.ObtenerPorId(id);
            if (residente == null)
                return HttpNotFound();

            return View(residente);
        }

        // GET: Residente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Residente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(usuariosDto residente)
        {
            if (ModelState.IsValid)
            {
                var id = _residenteService.CrearResidente(residente);
                if (id > 0)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "No se pudo crear el residente.");
            }
            return View(residente);
        }

        // GET: Residente/Edit/5
        public ActionResult Edit(int id)
        {
            var residente = _residenteService.ObtenerPorId(id);
            if (residente == null)
                return HttpNotFound();

            return View(residente);
        }

        // POST: Residente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(usuariosDto residente)
        {
            if (ModelState.IsValid)
            {
                var result = _residenteService.ActualizarResidente(residente);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "No se pudo actualizar el residente.");
            }
            return View(residente);
        }

        // GET: Residente/Delete/5
        public ActionResult Delete(int id)
        {
            var residente = _residenteService.ObtenerPorId(id);
            if (residente == null)
                return HttpNotFound();

            return View(residente);
        }

        // POST: Residente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _residenteService.EliminarResidente(id);
            if (result)
                return RedirectToAction("Index");

            TempData["Error"] = "No se pudo eliminar el residente.";
            return RedirectToAction("Delete", new { id });
        }
    }
}
