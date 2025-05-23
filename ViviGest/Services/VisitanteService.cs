using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViviGest.Dtos;
using ViviGest.Repositories;

namespace ViviGest.Services
{
    public class VisitanteService
    {
        private readonly VisitanteRepository repo = new VisitanteRepository();

        public List<VisitanteDto> GetAllVisitantes() => repo.GetAll();

        public VisitanteDto GetVisitante(int id) => repo.GetById(id);

        public VisitanteDto CreateVisitante(VisitanteDto v)
        {
            var result = repo.Create(v);
            v.Response = result > 0 ? 1 : 0;
            v.Message = result > 0 ? "Creado correctamente" : "Error al crear";
            return v;
        }

        public VisitanteDto UpdateVisitante(VisitanteDto v)
        {
            var result = repo.Update(v);
            v.Response = result > 0 ? 1 : 0;
            v.Message = result > 0 ? "Actualizado correctamente" : "Error al actualizar";
            return v;
        }

        public bool DeleteVisitante(int id)
        {
            return repo.Delete(id) > 0;
        }
    }
}