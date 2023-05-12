using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data;
using System.Data.SqlClient;

namespace CapaDeDatos
{
    public class DaProducto
    {
        public List<Producto> GetAll()
        {
            List<Producto> list = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"SELECT * FROM tblProducto Where estatus = 1";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadProducto(reader));
                }

            }

            return list;
        }//end get all

        public void Delete(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"update tblProducto set estatus = 0 where idProducto = @idPro;";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@idPro", producto.idProducto);
                cmd.ExecuteNonQuery();

            }
        }//end delete

        public bool Exist(int id)
        {
            int nrorecrd = 0;
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT Count(*)  FROM tblProducto     WHERE idProducto = @idproducto";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idproducto", id);
                    nrorecrd = Convert.ToInt32(cmd.ExecuteScalar());

                    return nrorecrd > 0;

                }
                catch
                {
                    throw;
                }
            }
        } // end exist

        public Producto CreateWithFoto(Producto producto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
                {
                    conn.Open();
                    string sql = @"INSERT INTO tblProducto(nombre, precio, stock, categoria, tamaño, produccion, estatus, foto)
	                                	Values(@nombre, @precio, 0, @categoria, @tama, @produccion, 1, @foto)                             
                                    SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.precio);
                    cmd.Parameters.AddWithValue("@categoria", producto.categoria);
                    cmd.Parameters.AddWithValue("@tama", producto.tamaño);
                    cmd.Parameters.AddWithValue("@produccion", producto.produccion);
                    cmd.Parameters.AddWithValue("@foto", producto.produccion);

                    producto.idProducto = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return producto;
            }
            catch
            {
                throw;
            }
        } // end create with foto

        public Producto Create(Producto producto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
                {
                    conn.Open();
                    string sql = @"INSERT INTO tblProducto(nombre, precio, stock, categoria, tamaño, produccion, estatus)
	                                	Values(@nombre, @precio, 0, @categoria, @tama, @produccion, 1)                             
                                    SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.precio);
                    cmd.Parameters.AddWithValue("@categoria", producto.categoria);
                    cmd.Parameters.AddWithValue("@tama", producto.tamaño);
                    cmd.Parameters.AddWithValue("@produccion", producto.produccion);

                    producto.idProducto = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return producto;
            }
            catch
            {
                throw;
            }
        } // end create with foto

        public Producto Update(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"UPDATE tblProducto SET  
                                            nombre = @nombre, 
                                            precio=@precio,
                                            categoria=@categoria,
                                            tamaño=@tama
                                    WHERE idProducto = @idPro";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@categoria", producto.categoria);
                cmd.Parameters.AddWithValue("@tama", producto.tamaño);
                cmd.Parameters.AddWithValue("@idPro", producto.idProducto);
                cmd.ExecuteNonQuery();

            }
            return producto;
        } //end update

        public Producto UpdateWithFoto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"UPDATE tblProducto SET  
                                            nombre = @nombre, 
                                            precio=@precio,
                                            categoria=@categoria,
                                            tamaño=@tama,
                                            foto = @foto
                                    WHERE idProducto = @idPro";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@categoria", producto.categoria);
                cmd.Parameters.AddWithValue("@tama", producto.tamaño);
                cmd.Parameters.AddWithValue("@idPro", producto.idProducto);
                cmd.Parameters.AddWithValue("@foto", producto.foto);
                cmd.ExecuteNonQuery();

            }
            return producto;
        } //end UpdateWithFoto

        public List<Producto> GetAllProductosInPedidoById(int id)
        {
            List<Producto> listaDeProductos = new List<Producto>();
            using(SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {

                conn.Open();
                string sqlDetalle = @"Select P.idProducto, p.precio, p.stock, p.categoria, p.tamaño, p.nombre, p.produccion
                                        FROM tblProducto p, tblDetallePedido DP
                                       WHERE p.idProducto = DP.idProducto and DP.idPedido = @idped";
                SqlCommand command = new SqlCommand(sqlDetalle, conn);
                command.Parameters.AddWithValue("@idped", id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listaDeProductos.Add(LoadProducto(reader));
                }
                return listaDeProductos;
            }
        } // end GetAllPedidoById

        public Producto GetProductoById(int id)
        {
            Producto producto = null;
            using(SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                string sql = @"SELECT * FROM tblproducto WHERE idProducto = @idpro";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idpro", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) producto = LoadProducto(reader);
                return producto;

            }
        }

        public decimal GetPrecioById(int id)
        {
            decimal precio = 0;
            using(SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"SELECT precio FROM tblProducto WHERE idProducto = @idpro";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idpro", id);
                precio = Convert.ToDecimal(cmd.ExecuteScalar());
            }
            return precio;
        }



        private Producto LoadProducto(IDataReader reader)
        {
            Producto pro = new Producto();
            pro.idProducto = Convert.ToInt32(reader["idProducto"]);
            pro.nombre = Convert.ToString(reader["nombre"]);
            pro.precio = Convert.ToDouble(reader["precio"]);
            pro.stock = Convert.ToInt32(reader["stock"]);
            pro.categoria = Convert.ToString(reader["categoria"]);
            pro.tamaño = Convert.ToString(reader["tamaño"]);
            pro.produccion = Convert.ToInt32(reader["produccion"]);
            if (reader["foto"] != DBNull.Value)
            {
                pro.foto = (byte[])reader["foto"];
            }

            return pro;
        }// end load
    } //end class
}//end namespace
