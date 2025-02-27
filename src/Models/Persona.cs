using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersManagmentApp.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El documento solo puede contener caracteres alfanuméricos.")]
        public required string Documento { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El nombre solo puede contener letras.")]
        public required string Nombres { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El apellido solo puede contener letras.")]
        public required string Apellidos { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        public List<Telefono> Telefonos { get; set; } = new List<Telefono>();
        public List<CorreoElectronico> Correos { get; set; } = new List<CorreoElectronico>();
        public List<Direccion> Direcciones { get; set; } = new List<Direccion>();
    }
}
