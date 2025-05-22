using System;

namespace ViviGest.Dtos
{
    public class PagoDto
    {
        public int id_pago { get; set; }
        public int id_residente { get; set; }
        public decimal monto { get; set; }
        public string metodo_pago { get; set; } = string.Empty;
        public DateTime fecha_pago { get; set; }
        public string estado { get; set; } = string.Empty;

        // Opcionales para respuesta de servicio
        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
