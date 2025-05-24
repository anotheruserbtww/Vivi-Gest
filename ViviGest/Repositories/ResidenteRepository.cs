using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ViviGest.Dtos;
using ViviGest.Utilities;
using log4net;

namespace ViviGest.Repositories
{
    public class ResidenteRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResidenteRepository));


        public int CreateResidente(usuariosDto residente)
        {
            using (var db = new DBContextUtility())
            {
                db.Connect();

                using (var cmd = new SqlCommand("dbo.sp_CrearResidente", db.CONN()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombres", residente.nombres);
                    cmd.Parameters.AddWithValue("@apellidos", residente.apellidos);
                    cmd.Parameters.AddWithValue("@numero_documento", residente.numero_documento);
                    cmd.Parameters.AddWithValue("@correo_electronico", residente.correo_electronico);
                    cmd.Parameters.AddWithValue("@telefono", residente.telefono);
                    cmd.Parameters.AddWithValue("@contrasena", residente.contrasena);
                    cmd.Parameters.AddWithValue("@id_rol", residente.id_rol); // Por ejemplo, rol Residente

                    var outParam = new SqlParameter("@NuevoResidenteID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return (int)outParam.Value;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error CreateResidente SP: {ex.Message}");
                        return 0;
                    }
                }
            }
        }

        public IEnumerable<usuariosDto> GetAllResidentes()
        {
            var list = new List<usuariosDto>();
            const string SQL = @"
                SELECT id_usuario, nombres, apellidos, numero_documento, correo_electronico, telefono, id_rol
                FROM vivigest.dbo.usuarios
                WHERE id_rol = 1; -- Asumiendo que 1 es el id del rol Residente
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
                            nombres = rdr["nombres"].ToString(),
                            apellidos = rdr["apellidos"].ToString(),
                            numero_documento = rdr["numero_documento"].ToString(),
                            correo_electronico = rdr["correo_electronico"].ToString(),
                            telefono = rdr["telefono"].ToString(),
                            id_rol = (int)rdr["id_rol"]
                        });
                    }
                }
            }
            return list;
        }

        public usuariosDto GetResidenteById(int idResidente)
        {
            usuariosDto residente = null;
            const string SQL = @"
                SELECT id_usuario, nombres, apellidos, numero_documento, correo_electronico, telefono, id_rol
                FROM vivigest.dbo.usuarios
                WHERE id_usuario = @idResidente AND id_rol = 1;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idResidente", idResidente);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            residente = new usuariosDto
                            {
                                id_usuario = (int)rdr["id_usuario"],
                                nombres = rdr["nombres"].ToString(),
                                apellidos = rdr["apellidos"].ToString(),
                                numero_documento = rdr["numero_documento"].ToString(),
                                correo_electronico = rdr["correo_electronico"].ToString(),
                                telefono = rdr["telefono"].ToString(),
                                id_rol = (int)rdr["id_rol"]
                            };
                        }
                    }
                }
            }
            return residente;
        }

        public bool UpdateResidente(usuariosDto residente)
        {
            const string SQL = @"
                UPDATE vivigest.dbo.usuarios
                SET nombres = @nombres,
                    apellidos = @apellidos,
                    numero_documento = @numero_documento,
                    correo_electronico = @correo_electronico,
                    telefono = @telefono,
                    contrasena = @contrasena,
                    id_rol = @id_rol
                WHERE id_usuario = @id_usuario AND id_rol = 1;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@nombres", residente.nombres);
                    cmd.Parameters.AddWithValue("@apellidos", residente.apellidos);
                    cmd.Parameters.AddWithValue("@numero_documento", residente.numero_documento);
                    cmd.Parameters.AddWithValue("@correo_electronico", residente.correo_electronico);
                    cmd.Parameters.AddWithValue("@telefono", residente.telefono);
                    cmd.Parameters.AddWithValue("@contrasena", residente.contrasena);
                    cmd.Parameters.AddWithValue("@id_rol", residente.id_rol);
                    cmd.Parameters.AddWithValue("@id_usuario", residente.id_usuario);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error UpdateResidente: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public bool DeleteResidente(int idResidente)
        {
            const string SQL = @"
                DELETE FROM vivigest.dbo.usuarios
                WHERE id_usuario = @idResidente AND id_rol = 1;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idResidente", idResidente);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error DeleteResidente: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}
