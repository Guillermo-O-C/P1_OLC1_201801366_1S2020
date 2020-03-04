using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class AFN
    {
        private NodoAFN Primero;
        private NodoAFN Ultimo;

        public AFN(NodoAFN Primero, NodoAFN Ultimo)
        {
            this.Primero = Primero;
            this.Ultimo = Ultimo;
        }

        public NodoAFN getPrimero()
        {
            return Primero;
        }

        public void setPrimero(NodoAFN Primero)
        {
            this.Primero = Primero;
        }

        public NodoAFN getUltimo()
        {
            return Ultimo;
        }

        public void setUltimo(NodoAFN Ultimo)
        {
            this.Ultimo = Ultimo;
        }


    }
}
