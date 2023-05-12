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
    public class DaTrabajador
    {
        private Trabajador LoadTrabajador(IDataReader reader)
        {
            Trabajador trabajador = new Trabajador();
            trabajador.idTrabajador = Convert.ToInt32(reader["idTrabajador"]);
            trabajador.ciTrabajador = Convert.ToInt32(reader["ciTrabajador"]);
            trabajador.nombre = Convert.ToString(reader["nombre"]);
            trabajador.telefono = Convert.ToString(reader["telf"]);
            trabajador.nick = Convert.ToString(reader["nick"]);
            trabajador.dateIn = Convert.ToDateTime(reader["dateIn"]);
            trabajador.estado = Convert.ToInt32(reader["estado"]);
            if (reader["dateMod"] != DBNull.Value)
               trabajador.dateMod = Convert.ToDateTime(reader["dateMod"]);

            return trabajador;
        } //end trabajador 

        public List<Trabajador> GetAll()
        {
            List<Trabajador> listaDeTrabajadores = new List<Trabajador>();
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT * FROM tblTrabajador WHERE estado = 1 ORDER BY nombre";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listaDeTrabajadores.Add(LoadTrabajador(reader));
                    }
                    return listaDeTrabajadores;
                }
                catch
                {
                    throw;
                }
            }
        } // end get all

        public Trabajador GetById(int id)
        {
            Trabajador trabajador = null;
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT * FROM tblTrabajador WHERE idTrabajador = @idTra";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idTra", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) trabajador = LoadTrabajador(reader);
                    return trabajador;
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
                    string sql = @"SELECT Count(*)  FROM tblTrabajador     WHERE idTrabajador = @idtra";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idtra", id);
                    nrorecrd = Convert.ToInt32(cmd.ExecuteScalar());

                    return nrorecrd > 0;

                }
                catch
                {
                    throw;
                }
            }
        } // end exist

        public void Delete(Trabajador trabajador)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                string sql = @"update tblTrabajador set estado = 2 WHERE idTrabajador = @idTra";
                //SCOPE_IDENTITY() , devuelve el ultimo registro creado

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@idTra", trabajador.idTrabajador);

                trabajador.idTrabajador = Convert.ToInt32(cmd.ExecuteScalar());
            }

        } // end delete

        public Trabajador Create(Trabajador trabajador)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"INSERT INTO tblTrabajador(ciTrabajador, nombre,telf, nick, estado, dateIn) 
                               values (@ci , @nombre ,  @telf , @nick , 1 , @date);
                               SELECT SCOPE_IDENTITY()";
                //SCOPE_IDENTITY() , devuelve el ultimo registro creado

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ci", trabajador.ciTrabajador);
                cmd.Parameters.AddWithValue("@nombre", trabajador.nombre);
                cmd.Parameters.AddWithValue("@telf", trabajador.telefono);
                cmd.Parameters.AddWithValue("@nick", trabajador.nick);
                cmd.Parameters.AddWithValue("@date", trabajador.dateIn);

                trabajador.idTrabajador = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return trabajador;
        }

        public Trabajador Update(Trabajador trabajador)
        {
            using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
            {
                conn.Open();
                string sql = @"update tblTrabajador set ciTrabajador = @ci, nombre = @nombre, telf = @telf, nick = @nick
                                WHERE idTrabajador = @idTra";
                //SCOPE_IDENTITY() , devuelve el ultimo registro creado

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ci", trabajador.ciTrabajador);
                cmd.Parameters.AddWithValue("@nombre", trabajador.nombre);
                cmd.Parameters.AddWithValue("@telf", trabajador.telefono);
                cmd.Parameters.AddWithValue("@nick", trabajador.nick);
                cmd.Parameters.AddWithValue("@idTra", trabajador.idTrabajador);

                trabajador.idTrabajador = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return trabajador;
        }//Listas por nick de trabajadores

        public List<Trabajador> GetNick()
        {
            try
            {
                List<Trabajador> listaDeTrabajadores = new List<Trabajador>();
                using (SqlConnection conn = new SqlConnection(ConexionSQL.ObtenerCadenaConexion()))
                {
                    conn.Open();
                    string sql = @"SELECT nick,idTrabajador  FROM tblTrabajador";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Trabajador trabajador = new Trabajador();
                        trabajador.nick = Convert.ToString(reader["nick"]);
                        trabajador.idTrabajador= Convert.ToInt32(reader["idTrabajador"]);
                    }
                    return listaDeTrabajadores;

                }
            }


            catch
            {
                throw;
            }

        }


    } //end class
} // end namesapce
