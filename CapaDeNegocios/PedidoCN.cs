using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDeDatos;
using System.Transactions;
using System.Data;

namespace CapaDeNegocios
{
    public class PedidoCN
    {

        private DaPedido daPedido = new DaPedido();

        public void Create(Pedido pedido)
        {
            // Inicializo la transacción
            pedido.fechaInicio = DateTime.Now;
            pedido.numPedido = (CountNumPed() + 1);
            if (pedido.status == 0) pedido.status = 1;

            if (pedido.fechaEntrega < DateTime.Now) pedido.fechaEntrega = DateTime.Now;

            if (pedido.direccionEntrega == null) pedido.direccionEntrega = "";

            using (TransactionScope scope = new TransactionScope())
            {
                // Creo la factura en la BD
                if (pedido.direccionEntrega != null && pedido.lat != null && pedido.lng != null)
                    daPedido.CreateWithMap(pedido);
                else
                    daPedido.Create(pedido);

                scope.Complete();
            }
        } //end create 

        public int CountNumPed()
        {
            return daPedido.CountNumPed();
        }

        public Pedido AsignarPedido(Pedido pedido)
        {
            return daPedido.AsignarPedido(pedido);
        }

        public Pedido CerrarPedido(Pedido pedido)
        {
            return daPedido.CerrarPedido(pedido);
        }

        public List<Pedido> GetAll()
        {
            return daPedido.GetAll();
        }

        public List<Pedido> GetAllByCliente(int id)
        {
            return daPedido.GetAllByClient(id);
        }

        public DataTable GetAllDataTable()
        {
            List<Pedido> list = GetAll();
            DataTable dt = new DataTable();

            dt.Columns.Add("idPedido", typeof(int));
            dt.Columns.Add("status", typeof(int));
            dt.Columns.Add("fechaInicio", typeof(DateTime));
            dt.Columns.Add("fechaEntrega", typeof(DateTime));
            dt.Columns.Add("costo", typeof(double));
            dt.Columns.Add("direccionEntrega", typeof(string));
            dt.Columns.Add("numPedido", typeof(int));

            foreach (Pedido ped in list)
            {
                dt.Rows.Add(ped.idPedido, ped.status, ped.fechaInicio, ped.fechaEntrega, ped.costo, ped.direccionEntrega, ped.numPedido);
            }

            return dt;
        }
    } //end class
}// end namespace
