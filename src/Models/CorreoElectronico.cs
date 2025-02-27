using System.ComponentModel.DataAnnotations;

namespace UsersManagmentApp.Models
{
    public class CorreoElectronico
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public required string email { get; set; }
    }
}