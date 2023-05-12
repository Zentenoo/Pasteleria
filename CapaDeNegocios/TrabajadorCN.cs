using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDeDatos;

namespace CapaDeNegocios
{
    public class TrabajadorCN
    {
        private DaTrabajador daTrabajador = new DaTrabajador();

        public List<Trabajador> GetAll()
        {
            return daTrabajador.GetAll();
        }
        public Trabajador GetById(int id)
        {
            return daTrabajador.GetById(id);
        }
        public void Delete(Trabajador trabajador)
        {
            if (trabajador.idTrabajador != 0)
                daTrabajador.Delete(trabajador);
        }
        public Trabajador Create(Trabajador trabajador)
        {
            trabajador.dateIn = DateTime.Now;
            if (trabajador.idTrabajador == 0)
            {
                trabajador = daTrabajador.Create(trabajador);
            }
            else
            {
                if (daTrabajador.Exist(trabajador.idTrabajador))
                    trabajador = daTrabajador.Update(trabajador);
                else
                    trabajador = daTrabajador.Create(trabajador);
            }
            return trabajador;
        }
        
    }
}
