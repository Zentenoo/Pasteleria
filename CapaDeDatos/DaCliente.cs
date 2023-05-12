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
    public class DaCliente
    {
        private Cliente LoadCliente(IDataReader reader)
        {
            Cliente cliente = new Cliente();
            cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
            cliente.nit = Convert.ToInt32(reader["nit"]);
            cliente.nombre = Convert.ToString(reader["nombre"]);
            cliente.telefono = Convert.ToString(reader["telf"]);

            return cliente;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> listaDeClientes = new List<Cliente>();
            using(SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT * FROM tblClientes ORDER BY nombre";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listaDeClientes.Add(LoadCliente(reader));
                    }
                    return listaDeClientes;
                }
                catch
                {
                    throw;
                }
            }
        } // end get all

        public Cliente GetById(int id)
        {
            Cliente cliente = null;
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT * FROM tblClientes WHERE idCliente = @idcli";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idcli", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) cliente = LoadCliente(reader);
                    return cliente;
                }
                catch
                {
                    throw;
                }
            }
        } // end get by id

        public bool Exist(int id)
        {
            int nrorecrd = 0;
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT Count(*)  FROM tblClientes     WHERE idCliente = @idcli";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idcli", id);
                    nrorecrd = Convert.ToInt32(cmd.ExecuteScalar());

                    return nrorecrd > 0;

                }
                catch
                {
                    throw;
                }
            }
        } // end exist

        public Cliente Create(Cliente cliente)
        {
            try
            { 
                using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
                {
                    string sql = @"INSERT INTO tblClientes (nit, nombre , telf) values (@nit, @nom, @telf  )  SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@nit", cliente.nit);
                    cmd.Parameters.AddWithValue("@nom", cliente.nombre);
                    cmd.Parameters.AddWithValue("@telf", cliente.telefono);

                    cliente.idCliente = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return cliente;
            }
            catch
            {
                throw;
            }
        } // end create
        public Cliente Update(Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"UPDATE tblClientes SET  
                                            nombre = @nombre, 
                                            nit=@nit,
                                            telf=@telf
                                    WHERE idCliente = @idcli";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nit", cliente.nit);
                cmd.Parameters.AddWithValue("@nombre", cliente.nombre);
                cmd.Parameters.AddWithValue("@telf", cliente.telefono);
                cmd.Parameters.AddWithValue("@idcli", cliente.idCliente);
                cmd.ExecuteNonQuery();

            }
            return cliente;
        }

    } //end class
}// end namespace
