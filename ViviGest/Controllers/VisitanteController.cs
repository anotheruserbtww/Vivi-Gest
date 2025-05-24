using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViviGest.Dtos;
using ViviGest.Services;
using ViviGest.Utilities;

namespace ViviGest.Controllers
{
    [AuthorizeRole(2, 3)]
    public class VisitanteController : Controller
    {
        private readonly VisitanteService _service = new VisitanteService();

        public ActionResult Index()
        {
            var lista = _service.GetAllVisitantes();
            return View(lista);
        }

        public ActionResult Create()
        {
            return View(new VisitanteDto());
        }

        [HttpPost]
        public ActionResult Create(VisitanteDto visitante)
        {
            if (!ModelState.IsValid)
                return View(visitante);

            var result = _service.CreateVisitante(visitante);
            if (result.Response == 1)
                return RedirectToAction("Index");

            ModelState.AddModelError("", result.Message);
            return View(visitante);
        }

        public ActionResult Edit(int id_visitante)
        {
            var visitante = _service.GetVisitante(id_visitante);
            if (visitante == null)
                return HttpNotFound();

            return View(visitante);
        }

        [HttpPost]
        public ActionResult Edit(VisitanteDto visitante)
        {
            if (!ModelState.IsValid)
                return View(visitante);

            var result = _service.UpdateVisitante(visitante);
            if (result.Response == 1)
                return RedirectToAction("Index");

            ModelState.AddModelError("", result.Message);
            return View(visitante);
        }

        public ActionResult Delete(int id_visitante)
        {
            var visitante = _service.GetVisitante(id_visitante);
            if (visitante == null)
                return HttpNotFound();

            return View(visitante);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id_visitante)
        {
            _service.DeleteVisitante(id_visitante);
            return RedirectToAction("Index");
        }
    }
}