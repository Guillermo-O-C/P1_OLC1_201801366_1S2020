using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class AnalizadorGenerico
    {
        private string ExprID;
        private LinkedList<TransicionesAFD> TablaTransiciones;
        private LinkedList<string> Terminales;

        public AnalizadorGenerico(string exprID, LinkedList<TransicionesAFD> tablaTransiciones, LinkedList<string> terminales)
        {
            ExprID = exprID;
            TablaTransiciones1 = tablaTransiciones;
            Terminales1 = terminales;
        }

        public string ExprID1 { get => ExprID; set => ExprID = value; }
        public LinkedList<string> Terminales1 { get => Terminales; set => Terminales = value; }
        internal LinkedList<TransicionesAFD> TablaTransiciones1 { get => TablaTransiciones; set => TablaTransiciones = value; }
    }
}
