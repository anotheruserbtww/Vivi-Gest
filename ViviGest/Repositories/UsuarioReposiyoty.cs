using ViviGest.Dtos;
using ViviGest.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;

namespace ViviGest.Repositories
{
    public class UsuarioReposiyoty
    {
        // Configuración de log4net
        private static readonly ILog log = LogManager.GetLogger(typeof(UsuarioReposiyoty));

        public int CreateUser(usuariosDto user)
        {
            int comando = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

          


            string SQL = "INSERT INTO vivigest.dbo.[usuarios] (numero_documento, tipo_documento, nombres, apellidos, telefono, correo, contrasena, id_rol) " +
                         "VALUES (@numero_documento, @tipo_documento, @nombres, @apellidos, @telefono, @correo, @contrasena, @id_rol);";



            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@numero_documento", user.numero_documento);
                command.Parameters.AddWithValue("@tipo_documento", user.tipo_documento);
                command.Parameters.AddWithValue("@nombres", user.nombres);
                command.Parameters.AddWithValue("@apellidos", user.apellidos);
                command.Parameters.AddWithValue("@telefono", user.telefono);
                command.Parameters.AddWithValue("@correo", user.correo);
                command.Parameters.AddWithValue("@contrasena", user.contrasena);
                command.Parameters.AddWithValue("@id_rol", user.id_rol);

                try
                {
                    comando = command.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    log.Error($"Error SQL: {sqlEx.Message}");
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    log.Error($"Error de operación: {invalidOpEx.Message}");
                }
                catch (Exception ex)
                {
                    log.Error($"Error general: {ex.Message}");
                }
            }

            connection.Disconnect();
            return comando;
        }

        public usuariosDto BuscarUsuarioPorNumeroDocumento(string numeroDocumento)
        {
            usuariosDto user = null;

            string SQL = "SELECT id_usuario, nombres, contrasena, id_rol, numero_documento, telefono, correo " +
                         "FROM vivigest.dbo.[usuarios] WHERE numero_documento = @numero_documento";
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            try
            {
                using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
                {
                    command.Parameters.AddWithValue("@numero_documento", numeroDocumento);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new usuariosDto
                            {
                                id_usuario = (int)reader["id_usuario"],
                                nombres = reader["nombres"].ToString(),
                                contrasena = reader["contrasena"].ToString(),
                                id_rol = (int)reader["id_rol"],
                                numero_documento = reader["numero_documento"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                correo = reader["correo"].ToString()
                            };
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error($"Error SQL: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                log.Error($"Error general: {ex.Message}");
            }

            connection.Disconnect();
            return user;
        }

        public bool BuscarUsuario(string username)
        {
            bool result = false;
            string SQL = "SELECT id_usuario, nombres, contrasena " +
                         "FROM vivigest.dbo.[usuarios] WHERE nombres = @username";
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            try
            {
                using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error($"Error SQL: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                log.Error($"Error general: {ex.Message}");
            }

            connection.Disconnect();
            return result;
        }

        public IEnumerable<usuariosDto> GetAllUsuarios()
        {
            List<usuariosDto> users = new List<usuariosDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM vivigest.dbo.[usuarios]";

            try
            {
                using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new usuariosDto
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
            }
            catch (SqlException sqlEx)
            {
                log.Error($"Error SQL: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                log.Error($"Error general: {ex.Message}");
            }

            connection.Disconnect();
            return users;
        }
    }
}
