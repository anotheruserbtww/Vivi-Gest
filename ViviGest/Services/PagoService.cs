using System;
using System.Collections.Generic;
using System.Linq;
using ViviGest.Dtos;
using ViviGest.Repositories;
namespace ViviGest.Services
{
    public class PagoService
    {
        private readonly PagoRepository _pagoRepository;
        private readonly ResidenteRepository _residenteRepository;

        public PagoService(PagoRepository pagoRepository, ResidenteRepository residenteRepository)
        {
            _pagoRepository = pagoRepository;
            _residenteRepository = residenteRepository;
        }

        public IEnumerable<PagoDto> ObtenerPagosPorFecha(DateTime inicio, DateTime fin)
        {
            return _pagoRepository.GetAllPagos()  // Esto usa el método GetAllPagos que ya tienes en el repo
                        .Where(p => p.fecha_pago.Date >= inicio.Date && p.fecha_pago.Date <= fin.Date);
        }
        public int CrearPago(PagoDto pago)
            => _pagoRepository.CreatePago(pago);
        public IEnumerable<PagoDto> ObtenerPagosPendientes()
           => _pagoRepository.ObtenerPagosPendientes();

        public IEnumerable<PagoDto> ObtenerTodosLosPagos()
            => _pagoRepository.GetAllPagos();


        public bool ConfirmarPago(int idPago)
            => _pagoRepository.ConfirmarPago(idPago) > 0;

        public PagoDto ObtenerPagoPorId(int idPago)
            => _pagoRepository.GetAllPagos()
                              .FirstOrDefault(p => p.id_pago == idPago);

        public usuariosDto ObtenerResidentePorId(int idResidente)
            => _residenteRepository.GetResidenteById(idResidente);
    }
}
