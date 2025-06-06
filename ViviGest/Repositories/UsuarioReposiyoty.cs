﻿using ViviGest.Dtos;
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
        private DBContextUtility db = new DBContextUtility();

        // Configuración de log4net
        private static readonly ILog log = LogManager.GetLogger(typeof(UsuarioReposiyoty));


        public int CreateUser(usuariosDto user)
        {
            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand("dbo.sp_CrearUsuario", conn.CONN()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                   
                    cmd.Parameters.AddWithValue("@numero_documento", user.numero_documento);
                    cmd.Parameters.AddWithValue("@tipo_documento", user.tipo_documento);
                    cmd.Parameters.AddWithValue("@nombres", user.nombres);
                    cmd.Parameters.AddWithValue("@apellidos", user.apellidos);
                    cmd.Parameters.AddWithValue("@telefono", user.telefono);
                    cmd.Parameters.AddWithValue("@correo_electronico", user.correo_electronico);
                    cmd.Parameters.AddWithValue("@contrasena", user.contrasena);
                    cmd.Parameters.AddWithValue("@id_rol", user.id_rol);

                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error SQL al insertar usuario (SP): " + ex.Message, ex);
                    }
                }
            }
        }
        public usuariosDto GetById(int id)
        {
            usuariosDto usuario = null;
            const string sql = "SELECT * FROM usuarios WHERE id_usuario = @id";

            using (var conn = db.CONN())
            {
                db.Connect();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            usuario = new usuariosDto
                            {
                                id_usuario = (int)rdr["id_usuario"],
                                numero_documento = rdr["numero_documento"].ToString(),
                                tipo_documento = rdr["tipo_documento"].ToString(),
                                nombres = rdr["nombres"].ToString(),
                                apellidos = rdr["apellidos"].ToString(),
                                telefono = rdr["telefono"].ToString(),
                                correo_electronico = rdr["correo_electronico"].ToString(),
                                contrasena = rdr["contrasena"].ToString(),
                                id_rol = (int)rdr["id_rol"]
                            };
                        }
                    }
                }
                db.Disconnect();
            }
            return usuario;
        }



        public usuariosDto BuscarUsuarioPorNumeroDocumento(string numeroDocumento)
        {
            usuariosDto user = null;
            string SQL = "SELECT id_usuario, nombres, contrasena, id_rol, numero_documento, telefono, correo_electronico " +
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
                                correo_electronico = reader["correo_electronico"].ToString()
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
                                correo_electronico = reader["correo_electronico"].ToString(),
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
