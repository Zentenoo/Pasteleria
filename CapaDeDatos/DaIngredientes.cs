using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace CapaDeDatos
{
    public class DaIngredientes
    {
        private Ingredientes LoadIngredientes(IDataReader reader)
        {
            Ingredientes ingrediente = new Ingredientes();
            ingrediente.idIngrediente = Convert.ToInt32(reader["idIngrediente"]);
            ingrediente.nombre = Convert.ToString(reader["nombre"]);
            ingrediente.precio = Convert.ToDouble(reader["precio"]);
            ingrediente.stock = Convert.ToInt32(reader["stock"]);

            return ingrediente;
        }// end loadIngredientes

        public List<Ingredientes> GetAll()
        {
            List<Ingredientes> ListaDeIngrediente = new List<Ingredientes>();
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"SELECT * FROM tblIngredientes";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListaDeIngrediente.Add(LoadIngredientes(reader));
                }
                return ListaDeIngrediente;
            }
        } //end getall



    } //end class
}// end namespace
