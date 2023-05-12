using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDeDatos;

namespace CapaDeNegocios
{
    public class ClienteCN
    {
        private DaCliente DAcliente= new DaCliente();

        public List<Cliente> GetAll()
        {
            return DAcliente.GetAll();
        }
        public Cliente GetById(int id)
        {
            return DAcliente.GetById(id);
        }
        public Cliente SaveCustomer(Cliente cliente)
        {
            if (DAcliente.Exist(cliente.idCliente))
                return DAcliente.Update(cliente); // si existe, actualizamnos al cliente
            
            else
                return DAcliente.Create(cliente); // si no existe, lo creamos en la DB
        }
    }
}
