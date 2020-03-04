using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class Nodo
    {

        public enum Tipo
        {
            Terminal,
            Operador_Binario,
            Operador_Unario, 
            Epsilon
        }

        private Nodo Previous;
        private Nodo Left;
        private Nodo Right;
        private Tipo tipo;
        private Token Value;
        private Boolean anulable;
        private int ID;
        private LinkedList<Nodo> Primeros;
        private LinkedList<Nodo> Ultimos;
        private LinkedList<Nodo> Siguientes;

        public Nodo(Tipo tipo, Token Value, int ID)
        {
            this.Previous = null;
            this.Left = null;
            this.Right = null;
            this.tipo = tipo;
            this.Value = Value;
            this.ID = ID;
            this.Primeros = new LinkedList<Nodo>();
            this.Ultimos = new LinkedList<Nodo>();
            this.Siguientes = new LinkedList<Nodo>();
        }

        public LinkedList<Nodo> getSiguientes()
        {
            return Siguientes;
        }

        public void setSiguientes(LinkedList<Nodo> Siguientes)
        {
            this.Siguientes = Siguientes;
        }

        public LinkedList<Nodo> getPrimeros()
        {
            return Primeros;
        }

        public void setPrimeros(LinkedList<Nodo> Primeros)
        {
            this.Primeros = Primeros;
        }

        public LinkedList<Nodo> getUltimos()
        {
            return Ultimos;
        }

        public void setUltimos(LinkedList<Nodo> Ultimos)
        {
            this.Ultimos = Ultimos;
        }

        public Nodo getPrevious()
        {
            return Previous;
        }

        public Nodo getLeft()
        {
            return Left;
        }

        public Nodo getRight()
        {
            return Right;
        }

        public Tipo getTipo()
        {
            return tipo;
        }

        public Token getValue()
        {
            return Value;
        }

        public void setPrevious(Nodo Previous)
        {
            this.Previous = Previous;
        }

        public void setLeft(Nodo Left)
        {
            this.Left = Left;
        }

        public void setRight(Nodo Right)
        {
            this.Right = Right;
        }

        public void setTipo(Tipo tipo)
        {
            this.tipo = tipo;
        }

        public void setValue(Token Value)
        {
            this.Value = Value;
        }

        public Boolean isAnulable()
        {
            return anulable;
        }

        public void setAnulable(Boolean anulable)
        {
            this.anulable = anulable;
        }

        public int getID()
        {
            return ID;
        }

        public void setID(int ID)
        {
            this.ID = ID;
        }

    }
}
