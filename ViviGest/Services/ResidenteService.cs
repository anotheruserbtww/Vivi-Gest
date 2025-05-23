using System.Collections.Generic;
using ViviGest.Dtos;
using ViviGest.Repositories;

namespace ViviGest.Services
{
    public class ResidenteService
    {
        private readonly ResidenteRepository _residenteRepo;

        public ResidenteService()
        {
            _residenteRepo = new ResidenteRepository();
        }

        public int CrearResidente(usuariosDto residente)
        {
            // Aquí puedes agregar validaciones antes de crear, por ejemplo:
            if (string.IsNullOrEmpty(residente.numero_documento))
                return 0;

            return _residenteRepo.CreateResidente(residente);
        }

        public IEnumerable<usuariosDto> ObtenerTodos()
        {
            return _residenteRepo.GetAllResidentes();
        }

        public usuariosDto ObtenerPorId(int id_usuario)
        {
            return _residenteRepo.GetResidenteById(id_usuario);
        }

        public bool ActualizarResidente(usuariosDto residente)
        {
            return _residenteRepo.UpdateResidente(residente);
        }

        public bool EliminarResidente(int id)
        {
            return _residenteRepo.DeleteResidente(id);
        }
    }
}
