using System.ComponentModel.DataAnnotations;

namespace UsersManagmentApp.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }

        [Required]
        public required string DireccionFisica  { get; set; }
    }
}