using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ViviGest.Dtos;
using ViviGest.Utilities;
using log4net;
using System.Data;


namespace ViviGest.Repositories
{
    public class PagoRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PagoRepository));


        public int CreatePago(PagoDto pago)


        {
            using (var db = new DBContextUtility())
            {
                db.Connect();

                using (var cmd = new SqlCommand("dbo.sp_CrearPago", db.CONN()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@id_residente", pago.id_residente);
                    cmd.Parameters.AddWithValue("@monto", pago.monto);
                    cmd.Parameters.AddWithValue("@metodo_pago", pago.metodo_pago);
                    cmd.Parameters.AddWithValue("@fecha_pago", pago.fecha_pago);
                    cmd.Parameters.AddWithValue("@estado", pago.estado);

                    // Parámetro de salida
                    var outParam = new SqlParameter("@NuevoPagoID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    log.Info($"Creando pago: id_residente={pago.id_residente}, monto={pago.monto}, metodo_pago={pago.metodo_pago}, fecha_pago={pago.fecha_pago}, estado={pago.estado}");
                    Console.WriteLine($"Creando pago: id_residente={pago.id_residente}, monto={pago.monto}, metodo_pago={pago.metodo_pago}, fecha_pago={pago.fecha_pago}, estado={pago.estado}");


                    try
                    {
                        cmd.ExecuteNonQuery();
                        // Recupera el ID generado
                        return (int)outParam.Value;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error CreatePago SP: {ex.Message}");
                        throw; // Lanza la excepción para que puedas verla mejor o debugearla
                    }
                }
            }
        }

        public IEnumerable<PagoDto> GetAllPagos()
        {
            var list = new List<PagoDto>();
            const string SQL = @"
                SELECT id_pago, id_residente, monto, metodo_pago, fecha_pago, estado
                  FROM vivigest.dbo.pagos;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        list.Add(new PagoDto
                        {
                            id_pago = (int)rdr["id_pago"],
                            id_residente = (int)rdr["id_residente"],
                            monto = (decimal)rdr["monto"],
                            metodo_pago = rdr["metodo_pago"].ToString(),
                            fecha_pago = (DateTime)rdr["fecha_pago"],
                            estado = rdr["estado"].ToString()
                        });
                }
            }
            return list;
        }

        public IEnumerable<PagoDto> ObtenerPagosPendientes()
        {
            var list = new List<PagoDto>();
            const string SQL = @"
                SELECT id_pago, id_residente, monto, metodo_pago, fecha_pago, estado
                  FROM vivigest.dbo.pagos
                 WHERE estado = 'Pendiente';
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        list.Add(new PagoDto
                        {
                            id_pago = (int)rdr["id_pago"],
                            id_residente = (int)rdr["id_residente"],
                            monto = (decimal)rdr["monto"],
                            metodo_pago = rdr["metodo_pago"].ToString(),
                            fecha_pago = (DateTime)rdr["fecha_pago"],
                            estado = rdr["estado"].ToString()
                        });
                }
            }
            return list;
        }

        public int ConfirmarPago(int idPago)
        {
            const string SQL = @"
                UPDATE vivigest.dbo.pagos
                   SET estado = 'Pagado',
                       fecha_pago = GETDATE()
                 WHERE id_pago = @idPago;
            ";

            using (var conn = new DBContextUtility())
            {
                conn.Connect();
                using (var cmd = new SqlCommand(SQL, conn.CONN()))
                {
                    cmd.Parameters.AddWithValue("@idPago", idPago);
                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error ConfirmarPago: {ex.Message}");
                        return 0;
                    }
                }
            }
        }
    }
}
