using System;

namespace ViviGest.Dtos
{
    public class CorrespondenciaDto
    {
        public int id_correspondencia { get; set; }
        public int destinatario { get; set; } // id_usuario
        public string tipo_correspondencia { get; set; }
        public string numero_apartamento { get; set; }
        public string remitente { get; set; }
        public DateTime fecha_recepcion { get; set; }
        public string estado { get; set; } // Pendiente, Entregado, etc.
        public int? registrado_por { get; set; }
    }
}
