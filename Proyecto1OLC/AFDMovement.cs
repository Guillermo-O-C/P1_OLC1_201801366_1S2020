using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class AFDMovement
    {
        private LinkedList<NodoAFN> transiciones;
        private string transicion;

        public AFDMovement(LinkedList<NodoAFN> transiciones, string transicion)
        {
            this.Transiciones = transiciones;
            this.Transicion = transicion;
        }

        public string Transicion { get => transicion; set => transicion = value; }
        internal LinkedList<NodoAFN> Transiciones { get => transiciones; set => transiciones = value; }
    }
}
