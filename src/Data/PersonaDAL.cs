using Microsoft.Data.SqlClient;
using UsersManagmentApp.Models;

public class PersonaDAL
{
    private readonly string _connectionString;

    public PersonaDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public List<Persona> ObtenerPersonas()
    {
        List<Persona> personas = new List<Persona>();
        Dictionary<int, Persona> personaMap = new Dictionary<int, Persona>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"
            SELECT 
                p.id, p.documento_identidad AS documento, p.Nombres, p.Apellidos, p.fecha_nacimiento,
                t.Id AS telefonoId, t.Numero AS telefono, 
                c.Id AS correoId, c.email, 
                d.Id AS direccionId, d.Direccion AS direccionFisica
            FROM persona p
            LEFT JOIN telefono t ON p.Id = t.persona_id
            LEFT JOIN correo_electronico c ON p.Id = c.persona_id
            LEFT JOIN direccion d ON p.Id = d.persona_id
            ORDER BY p.Id;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int personaId = reader.GetInt32(reader.GetOrdinal("id"));

                        if (!personaMap.ContainsKey(personaId))
                        {
                            personaMap[personaId] = new Persona
                            {
                                Id = personaId,
                                Documento = reader.GetString(reader.GetOrdinal("documento")),
                                Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                                Telefonos = new List<Telefono>(),
                                Correos = new List<CorreoElectronico>(),
                                Direcciones = new List<Direccion>()
                            };
                        }

                        Persona persona = personaMap[personaId];

                        if (!reader.IsDBNull(reader.GetOrdinal("telefonoId")))
                        {
                            int telefonoId = reader.GetInt32(reader.GetOrdinal("telefonoId"));
                            if (!persona.Telefonos.Any(t => t.Id == telefonoId))
                            {
                                persona.Telefonos.Add(new Telefono
                                {
                                    Id = telefonoId,
                                    PersonaId = personaId,
                                    Numero = reader.GetString(reader.GetOrdinal("telefono"))
                                });
                            }
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("correoId")))
                        {
                            int correoId = reader.GetInt32(reader.GetOrdinal("correoId"));
                            if (!persona.Correos.Any(c => c.Id == correoId))
                            {
                                persona.Correos.Add(new CorreoElectronico
                                {
                                    Id = correoId,
                                    PersonaId = personaId,
                                    email = reader.GetString(reader.GetOrdinal("email"))
                                });
                            }
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("direccionId")))
                        {
                            int direccionId = reader.GetInt32(reader.GetOrdinal("direccionId"));
                            if (!persona.Direcciones.Any(d => d.Id == direccionId))
                            {
                                persona.Direcciones.Add(new Direccion
                                {
                                    Id = direccionId,
                                    PersonaId = personaId,
                                    DireccionFisica = reader.GetString(reader.GetOrdinal("direccionFisica"))
                                });
                            }
                        }
                    }
                }
            }
        }

        return personaMap.Values.ToList();
    }


    public void RegistrarPersona(Persona nuevaPersona)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string verificarDocumentoQuery = "SELECT COUNT(*) FROM persona WHERE documento_identidad = @documento";
                    using (SqlCommand cmd = new SqlCommand(verificarDocumentoQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@documento", nuevaPersona.Documento);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            throw new Exception("El documento de identidad ya está registrado.");
                        }
                    }

                    if (nuevaPersona.Correos.Count == 0 && nuevaPersona.Direcciones.Count == 0)
                    {
                        throw new Exception("Debe registrar al menos un correo electrónico o una dirección.");
                    }

                    if (nuevaPersona.Telefonos.Count > 2 || nuevaPersona.Correos.Count > 2 || nuevaPersona.Direcciones.Count > 2)
                    {
                        throw new Exception("Solo se permiten 2 teléfonos, 2 correos electrónicos y 2 direcciones por persona.");
                    }

                    string insertPersonaQuery = @"
                INSERT INTO persona (documento_identidad, nombres, apellidos, fecha_nacimiento) 
                VALUES (@documento, @nombres, @apellidos, @fechaNacimiento);
                SELECT SCOPE_IDENTITY();";

                    int personaId;
                    using (SqlCommand cmd = new SqlCommand(insertPersonaQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@documento", nuevaPersona.Documento);
                        cmd.Parameters.AddWithValue("@nombres", nuevaPersona.Nombres);
                        cmd.Parameters.AddWithValue("@apellidos", nuevaPersona.Apellidos);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", nuevaPersona.FechaNacimiento);

                        personaId = Convert.ToInt32(cmd.ExecuteScalar());
                    }


                    string insertTelefonoQuery = "INSERT INTO telefono (persona_id, Numero) VALUES (@personaId, @numero);";
                    foreach (var telefono in nuevaPersona.Telefonos)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertTelefonoQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@personaId", personaId);
                            cmd.Parameters.AddWithValue("@numero", telefono.Numero);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    string insertCorreoQuery = "INSERT INTO correo_electronico (persona_id, email) VALUES (@personaId, @email);";
                    foreach (var correo in nuevaPersona.Correos)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertCorreoQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@personaId", personaId);
                            cmd.Parameters.AddWithValue("@email", correo.email);
                            cmd.ExecuteNonQuery();
                        }
                    }


                    string insertDireccionQuery = "INSERT INTO direccion (persona_id, Direccion) VALUES (@personaId, @direccion);";
                    foreach (var direccion in nuevaPersona.Direcciones)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertDireccionQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@personaId", personaId);
                            cmd.Parameters.AddWithValue("@direccion", direccion.DireccionFisica);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al registrar persona: " + ex.Message);
                }
            }
        }
    }

}
