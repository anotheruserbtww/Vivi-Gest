using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViviGest.Utilities;
using System.Data;
using System.Data.SqlClient;
using ViviGest.Dtos;
using ViviGest.Utilities;

namespace ViviGest.Repositories
{
    public class VisitanteRepository
    {
        private readonly DBContextUtility db = new DBContextUtility();

        public List<VisitanteDto> GetAll()
        {
            List<VisitanteDto> visitantes = new List<VisitanteDto>();
            SqlConnection conn = db.CONN();
            SqlCommand cmd = new SqlCommand("SELECT * FROM visitantes", conn);

            try
            {
                db.Connect();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    visitantes.Add(new VisitanteDto
                    {
                        id_visitante = Convert.ToInt32(reader["id_visitante"]),
                        nombre_completo = reader["nombre_completo"].ToString(),
                        tipo_documento = reader["tipo_documento"].ToString(),
                        numero_documento = reader["numero_documento"].ToString(),
                        destino = reader["destino"].ToString(),
                        
                    });
                }
                reader.Close();
            }
            finally
            {
                db.Disconnect();
            }

            return visitantes;
        }

        public VisitanteDto GetById(int id)
        {
            SqlConnection conn = db.CONN();
            SqlCommand cmd = new SqlCommand("SELECT * FROM visitantes WHERE id_visitante = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                db.Connect();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new VisitanteDto
                    {
                        id_visitante = Convert.ToInt32(reader["id_visitante"]),
                        nombre_completo = reader["nombre_completo"].ToString(),
                        tipo_documento = reader["tipo_documento"].ToString(),
                        numero_documento = reader["numero_documento"].ToString(),
                        destino = reader["destino"].ToString(),

                    };
                }
                reader.Close();
            }
            finally
            {
                db.Disconnect();
            }

            return null;
        }

        public int Create(VisitanteDto v)
        {
            SqlConnection conn = db.CONN();
            SqlCommand cmd = new SqlCommand("INSERT INTO visitantes (nombre_completo, tipo_documento, numero_documento, destino) VALUES (@nombre_completo, @tipo_documento, @numero_documento, @destino)", conn);

            cmd.Parameters.AddWithValue("@nombre_completo", v.nombre_completo);
            cmd.Parameters.AddWithValue("@tipo_documento", v.tipo_documento);
            cmd.Parameters.AddWithValue("@numero_documento", v.numero_documento);
            cmd.Parameters.AddWithValue("@destino", v.destino);

            try
            {
                db.Connect();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                db.Disconnect();
            }
        }

        public int Update(VisitanteDto v)
        {
            SqlConnection conn = db.CONN();
            SqlCommand cmd = new SqlCommand("UPDATE visitantes SET nombre_completo=@nombre_completo, tipo_documento=@tipo_documento, numero_documento=@numero_documento, destino=@destino WHERE id_visitante=@id", conn);

            cmd.Parameters.AddWithValue("@id", v.id_visitante);
            cmd.Parameters.AddWithValue("@nombre_completo", v.nombre_completo);
            cmd.Parameters.AddWithValue("@tipo_documento", v.tipo_documento);
            cmd.Parameters.AddWithValue("@numero_documento", v.numero_documento);
            cmd.Parameters.AddWithValue("@destino", v.destino);

            try
            {
                db.Connect();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                db.Disconnect();
            }
        }

        public int Delete(int id)
        {
            SqlConnection conn = db.CONN();
            SqlCommand cmd = new SqlCommand("DELETE FROM visitantes WHERE id_visitante = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                db.Connect();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                db.Disconnect();
            }
        }
    }
}
