using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ViviGest.Dtos;
using ViviGest.Utilities;
using log4net;

namespace ViviGest.Repositories
{
    public class UsuarioRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UsuarioRepository));

        public IEnumerable<usuariosDto> GetAllUsuarios()
        {
            var list = new List<usuariosDto>();
            const string SQL = @"
                SELECT id_usuario, numero_documento, tipo_documento, nombres, apellidos, telefono, correo_electronico, contrasena, id_rol
                FROM vivigest.dbo.usuarios;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new usuariosDto
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
                        });
                    }
                }
            }
            return list;
        }

        public usuariosDto GetUsuarioById(int id)
        {
            usuariosDto usuario = null;
            const string SQL = @"
                SELECT id_usuario, numero_documento, tipo_documento, nombres, apellidos, telefono, correo_electronico, contrasena, id_rol
                FROM vivigest.dbo.usuarios
                WHERE id_usuario = @idUsuario;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", id);
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
            }
            return usuario;
        }

        public int CreateUser(usuariosDto user)
        {
            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                var sql = @"
                    INSERT INTO vivigest.dbo.usuarios
                    (numero_documento, tipo_documento, nombres, apellidos, telefono, correo_electronico, contrasena, id_rol)
                    VALUES
                    (@numero_documento, @tipo_documento, @nombres, @apellidos, @telefono, @correo_electronico, @contrasena, @id_rol);
                ";

                using (var cmd = new SqlCommand(sql, conn.CONN()))
                {
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
                    catch (Exception ex)
                    {
                        log.Error($"Error CreateUser: {ex.Message}");
                        return 0;
                    }
                }
            }
        }

        public bool UpdateUsuario(usuariosDto user)
        {
            const string SQL = @"
                UPDATE vivigest.dbo.usuarios
                SET numero_documento = @numero_documento,
                    tipo_documento = @tipo_documento,
                    nombres = @nombres,
                    apellidos = @apellidos,
                    telefono = @telefono,
                    correo_electronico = @correo_electronico,
                    contrasena = @contrasena,
                    id_rol = @id_rol
                WHERE id_usuario = @id_usuario;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", user.id_usuario);
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
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error UpdateUsuario: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public bool DeleteUsuario(int id)
        {
            const string SQL = @"
                DELETE FROM vivigest.dbo.usuarios
                WHERE id_usuario = @id_usuario;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error DeleteUsuario: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}
