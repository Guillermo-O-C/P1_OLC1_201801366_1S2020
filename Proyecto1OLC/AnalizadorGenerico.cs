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
        private LinkedList<TerminalesTH> TerminalesTH;
        private LinkedList<string> Termianles;
        private LinkedList<int> Aceptacion;

        public AnalizadorGenerico(string exprID, LinkedList<TransicionesAFD> tablaTransiciones, LinkedList<TerminalesTH> terminalesTH, LinkedList<string> termianles, LinkedList<int> aceptacion)
        {
            ExprID = exprID;
            TablaTransiciones = tablaTransiciones;
            TerminalesTH = terminalesTH;
            Termianles = termianles;
            Aceptacion = aceptacion;
        }

        public string ExprID1 { get => ExprID; set => ExprID = value; }
        public LinkedList<int> Aceptacion1 { get => Aceptacion; set => Aceptacion = value; }
        public LinkedList<string> Termianles1 { get => Termianles; set => Termianles = value; }
        internal LinkedList<TransicionesAFD> TablaTransiciones1 { get => TablaTransiciones; set => TablaTransiciones = value; }
        internal LinkedList<TerminalesTH> TerminalesTH1 { get => TerminalesTH; set => TerminalesTH = value; }
    }
}
