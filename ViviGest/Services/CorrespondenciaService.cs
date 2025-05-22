using System.Collections.Generic;
using ViviGest.Dtos;
using ViviGest.Repositories;

namespace ViviGest.Services
{
    public class CorrespondenciaService
    {
        private readonly CorrespondenciaRepository _repo;

        public CorrespondenciaService()
        {
            _repo = new CorrespondenciaRepository();
        }

        public IEnumerable<CorrespondenciaDto> ObtenerCorrespondenciaPorUsuario(int idUsuario)
            => _repo.GetCorrespondenciaPorDestinatario(idUsuario);

        public bool MarcarEntregado(int idCorrespondencia)
            => _repo.MarcarComoEntregado(idCorrespondencia) > 0;

        public bool RegistrarCorrespondencia(CorrespondenciaDto dto)
            => _repo.InsertCorrespondencia(dto) > 0;
    }
}
