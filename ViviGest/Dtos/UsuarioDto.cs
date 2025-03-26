﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GastroByte.Dtos
{
    public class UsuarioDto
    {
        public int id_usuario { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        public string tipo_documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        public string numero_documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        public string correo_electronico { get; set; } = string.Empty;


        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string contrasena { get; set; } = string.Empty;

        public int id_estado { get; set; }
        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
        public int id_rol { get; set; }


    }




    }
