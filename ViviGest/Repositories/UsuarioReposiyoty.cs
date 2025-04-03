
using ViviGest.Dtos;
using ViviGest.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ViviGest.Repositories
{
    public class UsuarioReposiyoty

    {
        public int CreateUser(UsuarioDto user)
        {
            int resultado = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand("sp_CrearUsuario", Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Agregar los parámetros
                command.Parameters.AddWithValue("@id_rol", user.id_rol);
                command.Parameters.AddWithValue("@id_estado", user.id_estado);
                command.Parameters.AddWithValue("@nombres", user.nombres);
                command.Parameters.AddWithValue("@contraseña", user.contrasena);
                command.Parameters.AddWithValue("@telefono", user.telefono);
                command.Parameters.AddWithValue("@apellidos", user.apellidos);
                command.Parameters.AddWithValue("@tipo_documento", user.tipo_documento);
                command.Parameters.AddWithValue("@numero_documento", user.numero_documento);
                command.Parameters.AddWithValue("@correo", user.correo);

                try
                {
                    // Ejecutar el procedimiento almacenado
                    resultado = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear usuario: {ex.Message}");
                    resultado = -1; // Indicar que hubo un error
                }
                finally
                {
                    Connection.Disconnect();
                }
            }

            return resultado;
        }

        public UsuarioDto BuscarUsuarioPorNumeroDocumento(string numeroDocumento)
        {
            UsuarioDto user = null;
            string SQL = "SELECT id_usuario, nombres, contraseña, id_rol, id_estado, numero_documento, telefono, correo" +
                        "FROM vivigest.dbo.[Usuario] WHERE numero_documento = @numero_documento";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@numero_documento", numeroDocumento);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UsuarioDto
                        {
                            id_usuario = (int)reader["id_usuario"],
                            nombres = reader["nombre"].ToString(),
                            contrasena = reader["contraseña"].ToString(),
                            id_rol = (int)reader["id_rol"],
                            id_estado = (int)reader["id_estado"],
                            numero_documento = reader["numero_documento"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            correo = reader["correo"].ToString()
                        };
                    }
                }
            }
            Connection.Disconnect();
            return user;
        }



        public bool BuscarUsuario(string username)
        {
            bool result = false;
            string SQL = "SELECT id_usuario,id_estado,nombres,contraseña " +
                "FROM vivigest.dbo.[Usuario] " +
                "WHERE nombre = '" + username + "';";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = true;
                    }
                }
            }
            Connection.Disconnect();

            return result;
        }


        public IEnumerable<UsuarioDto> GetAllUsuarios()
        {
            List<UsuarioDto> user = new List<UsuarioDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM vivigest.dbo.[Usuario]";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Add(new UsuarioDto
                        {
                            id_usuario = (int)reader["id_usuario"],
                            nombres = reader["nombres"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            numero_documento = reader["numero_documento"].ToString(),
                            correo = reader["correo"].ToString(),
                            id_rol = (int)reader["id_rol"],
                        });
                    }
                }
            }

            connection.Disconnect();
            return user;
        }





    }
}