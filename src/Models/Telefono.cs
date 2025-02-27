using System.ComponentModel.DataAnnotations;

namespace UsersManagmentApp.Models
{
    public class Telefono
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }

        [Required]
        [RegularExpression(@"^\d{7,10}$", ErrorMessage = "El número debe contener entre 7 y 10 dígitos.")]
        public required string Numero { get; set; }
    }
}
