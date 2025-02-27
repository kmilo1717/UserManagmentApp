using Microsoft.AspNetCore.Mvc;
using UsersManagmentApp.Services;
using UsersManagmentApp.Models;

namespace UsersManagmentApp.Controllers
{
    [Route("api/personas")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly PersonaService _personaService;

        public PersonaController(PersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet]
        public IActionResult ObtenerPersonas()
        {
            var personas = _personaService.ObtenerPersonas();
            return Ok(personas);
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] Persona persona)
        {
            try
            {
                _personaService.RegistrarPersona(persona);
                return Ok(new { message = "Persona registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



    }
}
