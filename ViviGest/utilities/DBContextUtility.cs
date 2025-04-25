using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace  ViviGest.Utilities
{
    public class DBContextUtility
    {
        static string SERVER = "LAPTOP-R455N33E";
        static string DB_NAME = "vivigest";
        static string DB_USER = "vivigest1";
        static string DB_PASSWORD = "vivigest" +
            "" +
            "" +
            "";

        static string Conn = "server=" + SERVER + ";database=" + DB_NAME + ";user id=" + DB_USER + ";password=" + DB_PASSWORD + ";MultipleActiveResultSets=true";
        //mi conexion:
        SqlConnection Con = new SqlConnection(Conn);

        //procedimiento que abre la conexion sqlsever
        public bool Connect()
        {
            try
            {
                Con.Open();
                return true;  // Retorna true si la conexión fue exitosa
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;  // Retorna false si hubo algún error
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }
        //procedimiento que cierra la conexion sqlserver
        public void Disconnect()
        {
            Con.Close();
        }

        //funcion que devuelve la conexion sqlserver
        public SqlConnection CONN()
        {
            return Con;
        }
    }
}