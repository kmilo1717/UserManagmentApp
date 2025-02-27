using System.Text.RegularExpressions;
using UsersManagmentApp.Models;

namespace UsersManagmentApp.Services;
public class PersonaService
{
    private readonly PersonaDAL _personaDAL;

    public PersonaService(PersonaDAL personaDAL)
    {
        _personaDAL = personaDAL;
    }

    public List<Persona> ObtenerPersonas()
    {
        return _personaDAL.ObtenerPersonas();
    }

    public void RegistrarPersona(Persona persona)
    {
        _personaDAL.RegistrarPersona(persona);
    }
}
