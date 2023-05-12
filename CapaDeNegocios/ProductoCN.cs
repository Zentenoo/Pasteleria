using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CapaDeDatos;
using Entidades;

namespace CapaDeNegocios
{
    public class ProductoCN
    {
        private DaProducto daProducto = new DaProducto();       

        public List<Producto> GetAll()
        {
            return daProducto.GetAll();
        } // end GetAll

        public Producto GetProductoByid(int id)
        {
            try
            {
                if (id >= 0)
                {
                    return daProducto.GetProductoById(id);
                }
                else
                {
                    throw new Exception("El id debe ser mayor o igual a 0....");
                }
            }
            catch (Exception)
            {
                throw;
            }
        } //end GetProductoByid

        public decimal GetPrecioByIdProducto(int id)
        {
            try
            {
                if (id >= 0)
                {
                    return daProducto.GetPrecioById(id);
                }
                else
                {
                    //colocar una excepcion;
                    throw new Exception("El id debe ser mayor o igual a 0....");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }// end GetPrecioByIdProducto

        public void Delete(Producto producto)
        {
            if (daProducto.Exist(producto.idProducto))
                daProducto.Delete(producto);
        }// end delete

        public Producto Create(Producto producto)
        {
            if (daProducto.Exist(producto.idProducto)) 
            {
                if (producto.foto == null)
                    daProducto.Update(producto);
                else
                    daProducto.UpdateWithFoto(producto);
            }
            else
            {
                if (producto.foto == null)
                    daProducto.Create(producto);
                else
                    daProducto.CreateWithFoto(producto);
            }
            return producto;
            
        }

    } //end class
}// end namespace
