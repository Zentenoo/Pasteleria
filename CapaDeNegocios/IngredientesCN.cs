using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDeDatos;
using Entidades;

namespace CapaDeNegocios
{
    public class IngredientesCN
    {
        private DaIngredientes daIngredientes = new DaIngredientes();

        public List<Ingredientes> GetAll()
        {
            return daIngredientes.GetAll();
        }
    }
}
