using AsignacionesEstudiantiles.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace AsignacionesEstudiantiles.Data
{
    public class GetData
    {
        private readonly string _connectionString;
        public GetData(string connectionString)
        {
            _connectionString = connectionString;   
        }

        public List<AsignacionModel> GetAsignaciones()
        {
            List<AsignacionModel> list = new();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand comm = new("SELECT * FROM ASIGNACION", conn);

                conn.Open();

                SqlDataReader reader = comm.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        AsignacionModel model = new()
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                        list.Add(model);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return list;
        }

        public List<EstudianteModel> GetEstudiantes()
        {
            List<EstudianteModel> list = new();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand comm = new("SELECT * FROM ESTUDIANTE", conn);

                conn.Open();

                SqlDataReader reader = comm.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        EstudianteModel model = new()
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Telefono = reader["TELEFONO"] == DBNull.Value ? string.Empty : reader.GetString(2)
                        };
                        list.Add(model);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return list;
        }

        public List<HojitaModel> GetHojita()
        {
            List<HojitaModel> list = new();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand comm = new("SELECT * FROM PROGRAMA", conn);

                conn.Open();

                SqlDataReader reader = comm.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        HojitaModel model = new()
                        {
                            Id  = reader.GetString(0),
                            Fecha  = reader.GetDateTime(1),
                            Asignacion = reader.GetString(2),
                            Nombre = reader.GetString(3),
                            Ayudante = reader["AYUDANTE"] == DBNull.Value ? null : reader.GetString(4),
                            Archivo = reader.GetString(5)
                        };
                        list.Add(model);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return list;
        }

        public int InsertPrograma(ProgramaModel model, string path)
        {
            List<EstudianteModel> list = new();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string insert = $"INSERT INTO PROGRAMA VALUES ('{model.id}','{(DateTime.ParseExact(model.fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"))}'," +
                    $"'{model.asignacion}','{model.nombre}','{model.ayudante}','{path}')";
                SqlCommand comm = new(insert, conn);

                conn.Open();

                try
                {
                    return comm.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
