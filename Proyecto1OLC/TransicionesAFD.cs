using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class TransicionesAFD
    {
        private int conjunto;
        private string transicion;
        private int llegada;

        public TransicionesAFD(int conjunto, string transicion, int llegada)
        {
            this.conjunto = conjunto;
            this.Transicion = transicion;
            this.Llegada = llegada;
        }

        public int Conjunto { get => conjunto; set => conjunto = value; }
        public string Transicion { get => transicion; set => transicion = value; }
        public int Llegada { get => llegada; set => llegada = value; }
    }

}
