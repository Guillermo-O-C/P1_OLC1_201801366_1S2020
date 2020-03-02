using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class ExpresionRegular
    {

        private LinkedList<Token> Nodos;
        private String expID;
        private String expresion;

        public ExpresionRegular(String expID, String expresion)
        {
            this.expID = expID;
            this.expresion = expresion;
        }

        public LinkedList<Token> getNodos()
        {
            return Nodos;
        }

        public void setNodos(LinkedList<Token> Nodos)
        {
            this.Nodos = Nodos;
        }

        public String getExpID()
        {
            return expID;
        }

        public void setExpID(String expID)
        {
            this.expID = expID;
        }

        public String getExpresion()
        {
            return expresion;
        }

        public void setExpresion(String expresion)
        {
            this.expresion = expresion;
        }

    }
}
