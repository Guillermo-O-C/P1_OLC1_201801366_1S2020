using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class Conjunto
    {

        private String ID;
        private String conjunto;
        private Tipo listType;
        private LinkedList<Char> DefConjunto;
        private LinkedList<int> DefIntConjunto;

        public enum Tipo {ConjuntoChar, ConjuntoInt};
        public LinkedList<char> DefConjunto1 { get => DefConjunto; set => DefConjunto = value; }
        public LinkedList<int> DefIntConjunto1 { get => DefIntConjunto; set => DefIntConjunto = value; }
        internal Tipo ListType { get => listType; set => listType = value; }

        public Conjunto(String ID, String conjunto)
        {
            this.ID = ID;
            this.conjunto = conjunto;
            this.DefConjunto1 = new LinkedList<char>();
            this.DefIntConjunto1 = new LinkedList<int>();
        }

        public String getID()
        {
            return ID;
        }

        public void setID(String ID)
        {
            this.ID = ID;
        }

        public String getConjunto()
        {
            return conjunto;
        }

        public void setConjunto(String conjunto)
        {
            this.conjunto = conjunto;
        }
    }
}
