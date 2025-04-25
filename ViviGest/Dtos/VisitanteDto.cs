using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ViviGest.Dtos
{
    public class VisitanteDto
    {
        public int id_visitante { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string nombre_completo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de documento.")]
        public string tipo_documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        public string numero_documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        public string destino { get; set; } = string.Empty;

       

        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
