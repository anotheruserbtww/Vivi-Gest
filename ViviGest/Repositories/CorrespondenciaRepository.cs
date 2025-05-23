using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ViviGest.Dtos;
using ViviGest.Utilities;
using System.Data;
using log4net;

namespace ViviGest.Repositories
{
    public class CorrespondenciaRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CorrespondenciaRepository));

        public IEnumerable<CorrespondenciaDto> GetCorrespondenciaPorDestinatario(int idUsuario)
        {
            var list = new List<CorrespondenciaDto>();
            const string SQL = @"
                SELECT id_correspondencia, destinatario, tipo_correspondencia, numero_apartamento, remitente, fecha_recepcion, estado, registrado_por
                  FROM vivigest.dbo.correspondencia
                 WHERE destinatario = @idUsuario
                 ORDER BY fecha_recepcion DESC;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new CorrespondenciaDto
                            {
                                id_correspondencia = (int)rdr["id_correspondencia"],
                                destinatario = (int)rdr["destinatario"],
                                tipo_correspondencia = rdr["tipo_correspondencia"].ToString(),
                                numero_apartamento = rdr["numero_apartamento"].ToString(),
                                remitente = rdr["remitente"].ToString(),
                                fecha_recepcion = (DateTime)rdr["fecha_recepcion"],
                                estado = rdr["estado"].ToString(),
                                registrado_por = rdr["registrado_por"] as int?
                            });
                        }
                    }
                }
            }

            return list;
        }

        public int MarcarComoEntregado(int idCorrespondencia)
        {
            const string SQL = @"
                UPDATE vivigest.dbo.correspondencia
                   SET estado = 'Entregado'
                 WHERE id_correspondencia = @idCorrespondencia;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idCorrespondencia", idCorrespondencia);

                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error MarcarComoEntregado: {ex.Message}");
                        return 0;
                    }
                }
            }
        }

        public IEnumerable<CorrespondenciaDto> GetTodasCorrespondencias()
        {
            var list = new List<CorrespondenciaDto>();
            const string SQL = @"
        SELECT id_correspondencia, destinatario, tipo_correspondencia, numero_apartamento, remitente, fecha_recepcion, estado, registrado_por
        FROM vivigest.dbo.correspondencia
        ORDER BY fecha_recepcion DESC;
    ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new CorrespondenciaDto
                            {
                                id_correspondencia = (int)rdr["id_correspondencia"],
                                destinatario = (int)rdr["destinatario"],
                                tipo_correspondencia = rdr["tipo_correspondencia"].ToString(),
                                numero_apartamento = rdr["numero_apartamento"].ToString(),
                                remitente = rdr["remitente"].ToString(),
                                fecha_recepcion = (DateTime)rdr["fecha_recepcion"],
                                estado = rdr["estado"].ToString(),
                                registrado_por = rdr["registrado_por"] as int?
                            });
                        }
                    }
                }
            }

            return list;
        }


        public CorrespondenciaDto GetCorrespondenciaPorId(int idCorrespondencia)
        {
            const string SQL = @"
        SELECT id_correspondencia, destinatario, tipo_correspondencia, numero_apartamento, remitente, fecha_recepcion, estado, registrado_por
        FROM vivigest.dbo.correspondencia
        WHERE id_correspondencia = @idCorrespondencia;
    ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idCorrespondencia", idCorrespondencia);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            return new CorrespondenciaDto
                            {
                                id_correspondencia = (int)rdr["id_correspondencia"],
                                destinatario = (int)rdr["destinatario"],
                                tipo_correspondencia = rdr["tipo_correspondencia"].ToString(),
                                numero_apartamento = rdr["numero_apartamento"].ToString(),
                                remitente = rdr["remitente"].ToString(),
                                fecha_recepcion = (DateTime)rdr["fecha_recepcion"],
                                estado = rdr["estado"].ToString(),
                                registrado_por = rdr["registrado_por"] as int?
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int UpdateCorrespondencia(CorrespondenciaDto dto)
        {
            const string SQL = @"
        UPDATE vivigest.dbo.correspondencia
        SET destinatario = @destinatario,
            tipo_correspondencia = @tipo_correspondencia,
            numero_apartamento = @numero_apartamento,
            remitente = @remitente,
            estado = @estado,
            registrado_por = @registrado_por
        WHERE id_correspondencia = @id_correspondencia;
    ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@id_correspondencia", dto.id_correspondencia);
                    cmd.Parameters.AddWithValue("@destinatario", dto.destinatario);
                    cmd.Parameters.AddWithValue("@tipo_correspondencia", dto.tipo_correspondencia);
                    cmd.Parameters.AddWithValue("@numero_apartamento", dto.numero_apartamento);
                    cmd.Parameters.AddWithValue("@remitente", dto.remitente ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", dto.estado);
                    cmd.Parameters.AddWithValue("@registrado_por", dto.registrado_por ?? (object)DBNull.Value);

                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log error aquí si quieres
                        return 0;
                    }
                }
            }
        }
        public int DeleteCorrespondencia(int idCorrespondencia)
        {
            const string SQL = @"
        DELETE FROM vivigest.dbo.correspondencia
        WHERE id_correspondencia = @idCorrespondencia;
    ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idCorrespondencia", idCorrespondencia);

                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error DeleteCorrespondencia: {ex.ToString()}");
                        return 0;
                    }
                }
            }
        }


        public int InsertCorrespondencia(CorrespondenciaDto dto)
        {
            const string SQL = @"
                INSERT INTO vivigest.dbo.correspondencia
                (destinatario, tipo_correspondencia, numero_apartamento, remitente, fecha_recepcion, estado, registrado_por)
                VALUES
                (@destinatario, @tipo_correspondencia, @numero_apartamento, @remitente, GETDATE(), 'Pendiente', @registrado_por);
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@destinatario", dto.destinatario);
                    cmd.Parameters.AddWithValue("@tipo_correspondencia", dto.tipo_correspondencia);
                    cmd.Parameters.AddWithValue("@numero_apartamento", dto.numero_apartamento);
                    cmd.Parameters.AddWithValue("@remitente", dto.remitente ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@registrado_por", dto.registrado_por ?? (object)DBNull.Value);

                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error InsertCorrespondencia: {ex.Message}");
                        return 0;
                    }
                }
            }
        }
    }
}
