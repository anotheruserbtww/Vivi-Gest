using System.Linq;
using System.Web.Mvc;
using ViviGest.Dtos;
using ViviGest.Repositories;
using ViviGest.Services;
using ViviGest.Utilities;

namespace ViviGest.Controllers
{
    [AuthorizeRole(1,2)]

    public class CorrespondenciaController : Controller
    {
        private readonly CorrespondenciaService _service = new CorrespondenciaService();
        private readonly ResidenteRepository residente; // campo privado

        public CorrespondenciaController()
        {
            residente = new ResidenteRepository(); // instancia aquí
        }

        // Listar correspondencia de un usuario
        public ActionResult Index()
        {
            var lista = _service.ObtenerTodasCorrespondencias();
            return View(lista);
        }

        // Mostrar formulario para crear correspondencia
        public ActionResult Create()
        {
            var residentes = residente.GetAllResidentes(); // usa el campo privado

            ViewBag.Residentes = residentes.Select(r => new SelectListItem
            {
                Value = r.id_usuario.ToString(),
                Text = $"{r.nombres} {r.apellidos} - Numero de Identificacion: {r.numero_documento}"
            }).ToList();

            return View(new CorrespondenciaDto());
        }



        // POST: Crear correspondencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CorrespondenciaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = _service.RegistrarCorrespondencia(dto);
            if (success)
                return RedirectToAction("Index", new { idUsuario = dto.destinatario });

            ModelState.AddModelError("", "No se pudo registrar la correspondencia.");
            return View(dto);
        }

        // Mostrar formulario para editar correspondencia
        public ActionResult Edit(int id)
        {
            var correspondencia = _service.ObtenerCorrespondenciaPorId(id);
            if (correspondencia == null)
                return HttpNotFound();

            return View(correspondencia);
        }


        // POST: Editar correspondencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CorrespondenciaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = _service.ActualizarCorrespondencia(dto);
            if (success)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "No se pudo actualizar la correspondencia.");
            return View(dto);
        }

        // GET: Confirmar eliminación
        public ActionResult Delete(int id)
        {
            var correspondencia = _service.ObtenerCorrespondenciaPorId(id);
            if (correspondencia == null)
                return HttpNotFound();

            return View(correspondencia);
        }

        // POST: Eliminar correspondencia
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var success = _service.EliminarCorrespondencia(id);
            if (!success)
            {
                ModelState.AddModelError("", "No se pudo eliminar la correspondencia.");
                var correspondencia = _service.ObtenerCorrespondenciaPorId(id);
                return View(correspondencia);
            }

            return RedirectToAction("Index");
        }


    }
}
