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

        public Conjunto(String ID, String conjunto)
        {
            this.ID = ID;
            this.conjunto = conjunto;
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
