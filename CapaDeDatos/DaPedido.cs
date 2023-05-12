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
    public class DaPedido
    {
        /// <summary>
        /// Metodo para armar un pedido por medio de un reader
        /// </summary>
        /// 
        // Dejar para fase 2 del proyecto 
        private Pedido LoadPedido(IDataReader reader)
        {
            Pedido ped = new Pedido();
            ped.idPedido = Convert.ToInt32(reader["idPedido"]);
            ped.numPedido = Convert.ToInt32(reader["numPedido"]);
            ped.fechaInicio = Convert.ToDateTime(reader["fechaInicio"]);
            ped.fechaEntrega = Convert.ToDateTime(reader["fechaEntrega"]);
            ped.costo = Convert.ToInt32(reader["costo"]);
            ped.direccionEntrega = Convert.ToString(reader["direccion"]);
            ped.status = Convert.ToInt32(reader["estado"]);
            ped.idTrabajador = Convert.ToInt32(reader["idTrabajador"]);
            ped.idCliente = Convert.ToInt32(reader["idCliente"]);
            if (reader["descripcionMap"] != DBNull.Value)
               ped.descripcionMap = Convert.ToString(reader["descripcionMap"]);
            if (reader["lat"] != DBNull.Value)
                ped.lat = Convert.ToDouble(reader["lat"]);
            if (reader["lng"] != DBNull.Value)
                ped.lng = Convert.ToDouble(reader["lng"]);

            return ped;
        } //end loadpedido

        public List<Pedido> GetAll()
        {
            List<Pedido> list = new List<Pedido>();

            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"SELECT * FROM tblPedido";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadPedido(reader));
                }

            }

            return list;
        }//end get all

        public List<Pedido> GetAllByClient(int id)
        {
            List<Pedido> list = new List<Pedido>();

            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"SELECT * FROM tblPedido WHERE idCliente = @idcliente AND lat is not null ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idcliente", id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadPedido(reader));
                }

            }

            return list;
        }//end get all

        public int CountNumPed()
        {
            int nrorecrd = 0;
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT Count(*)  FROM tblPedido  ";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    nrorecrd = Convert.ToInt32(cmd.ExecuteScalar());

                    return nrorecrd;

                }
                catch
                {
                    throw;
                }
            }
        } // end exist

        public Pedido AsignarPedido(Pedido pedido)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"UPDATE tblPedido SET  
                                            idTrabajador = @idtra,
                                            estado = 2
                                    WHERE idPedido = @idPed";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@idtra", pedido.idTrabajador);
                cmd.Parameters.AddWithValue("@idPed", pedido.idPedido);
                cmd.ExecuteNonQuery();

            }
            return pedido;
        }
        public Pedido CerrarPedido(Pedido pedido)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"UPDATE tblPedido SET  
                                            estado = 3
                                    WHERE idPedido = @idPed";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@idPed", pedido.idPedido);
                cmd.ExecuteNonQuery();

            }
            return pedido;
        }

        public Pedido GetById(int id)
        {
            Pedido pedido = null;
            DaProducto DAproducto = new DaProducto();
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT * FROM tblPedido WHERE idPedido = @idped";
                    using(SqlCommand cmd = new SqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@idped", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            pedido = LoadPedido(reader);
                           // pedido.listaDeProductos = DAproducto.GetAllProductosInPedidoById(id);
                           // *********************************** DANGER ***********************************
                        }
                    }                   

                    return pedido;
                }
                catch
                {
                    throw;
                }
            }
        } // end get by id
        public void CreateWithMap(Pedido ped)
        {
            using(SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"INSERT into tblPedido (numPedido, fechaInicio, fechaEntrega, costo, direccion, estado, idCliente, idTrabajador,  lat, lng,descripcionMap)
                values ( @numped , @fechaIni , @fechaFin , @costo , @direccion , @estado , @idcliente , @idTrabajador,   @lat, @lng, @descripcion) SELECT SCOPE_IDENTITY()";

                using(SqlCommand cmd = new SqlCommand(sql, conn))
                    //linea para pedido
                {
                    cmd.Parameters.AddWithValue("@numped", ped.numPedido);
                    cmd.Parameters.AddWithValue("@fechaIni", ped.fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", ped.fechaEntrega);
                    cmd.Parameters.AddWithValue("@costo", ped.costo);
                    cmd.Parameters.AddWithValue("@direccion", ped.direccionEntrega);
                    cmd.Parameters.AddWithValue("@estado", ped.status);
                    cmd.Parameters.AddWithValue("@idcliente", ped.idCliente);
                    cmd.Parameters.AddWithValue("@idTrabajador", ped.idTrabajador);
                    cmd.Parameters.AddWithValue("@lat", ped.lat);
                    cmd.Parameters.AddWithValue("@lng", ped.lng);
                    cmd.Parameters.AddWithValue("@descripcion", ped.descripcionMap);


                    ped.idPedido = Convert.ToInt32(cmd.ExecuteScalar());// nos retorna el Id de la factura creada
                }
                //linea para el detalle del pedido
                string sqlDetalle = @"INSERT INTO tblDetallePedido(idProducto, idPedido, cantidad) values (@idpro, @idped, @cant)";
                using (SqlCommand cmd = new SqlCommand(sqlDetalle, conn))
                //linea para pedido
                {
                    foreach(DetallePedido producto in ped.listaDeProductos)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@idpro", producto.idProducto);
                        cmd.Parameters.AddWithValue("@idped", ped.idPedido);
                        cmd.Parameters.AddWithValue("@cant", producto.cantidad);

                        producto.idProducto = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }

            }
        }// end create with map

        public void Create(Pedido ped)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"INSERT into tblPedido (numPedido, fechaInicio, fechaEntrega, costo,  estado, idCliente, idTrabajador)
                values ( @numped , @fechaIni , @fechaFin , @costo ,  @estado , @idcliente , @idTrabajador ) SELECT SCOPE_IDENTITY()";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                //linea para pedido
                {
                    cmd.Parameters.AddWithValue("@numped", ped.numPedido);
                    cmd.Parameters.AddWithValue("@fechaIni", ped.fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", ped.fechaEntrega);
                    cmd.Parameters.AddWithValue("@costo", ped.costo);
                    cmd.Parameters.AddWithValue("@estado", ped.status);
                    cmd.Parameters.AddWithValue("@idcliente", ped.idCliente);
                    cmd.Parameters.AddWithValue("@idTrabajador", ped.idTrabajador);


                    ped.idPedido = Convert.ToInt32(cmd.ExecuteScalar());// nos retorna el Id de la factura creada
                }
                //linea para el detalle del pedido
                string sqlDetalle = @"INSERT INTO tblDetallePedido(idProducto, idPedido, cantidad) values (@idpro, @idped, @cant)";
                using (SqlCommand cmd = new SqlCommand(sqlDetalle, conn))
                //linea para pedido
                {
                    foreach (DetallePedido producto in ped.listaDeProductos)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@idpro", producto.idProducto);
                        cmd.Parameters.AddWithValue("@idped", ped.idPedido);
                        cmd.Parameters.AddWithValue("@cant", producto.cantidad);

                        producto.idProducto = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }

            }
        }// end create

        public decimal GetPrecioByIdPedido(int id)
        {
            decimal precio = 0;

            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();

                string sql = @"SELECT precio FROM tblProducto WHERE idProducto = @idProducto";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idProducto", Convert.ToInt32(id));
                precio = Convert.ToDecimal(cmd.ExecuteScalar());

            }

            return precio;

        } // end precio by id



    }// end class
} //end namesapce
