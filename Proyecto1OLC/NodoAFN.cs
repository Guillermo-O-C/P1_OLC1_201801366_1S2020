using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class NodoAFN
    {
        private NodoAFN Left;
        private NodoAFN Right;
        private String TransicionLeft;
        private String TransicionRight;
        private int ID;

        public NodoAFN(int ID)
        {
            this.Left = null;
            this.Right = null;
            this.TransicionLeft = "";
            this.TransicionRight = "";
            this.ID = ID;
        }

        public NodoAFN getLeft()
        {
            return Left;
        }

        public void setLeft(NodoAFN Left)
        {
            this.Left = Left;
        }

        public NodoAFN getRight()
        {
            return Right;
        }

        public void setRight(NodoAFN Right)
        {
            this.Right = Right;
        }

        public String getTransicionLeft()
        {
            return TransicionLeft;
        }

        public void setTransicionLeft(String TransicionLeft)
        {
            this.TransicionLeft = TransicionLeft;
        }

        public String getRransicionRight()
        {
            return TransicionRight;
        }

        public void setTransicionRight(String TransicionRight)
        {
            this.TransicionRight = TransicionRight;
        }

        public int getID()
        {
            return ID;
        }

        public void setID(int ID)
        {
            this.ID = ID;
        }

        internal void setLeft(AFN hijoIzq)
        {
            throw new NotImplementedException();
        }
    }
}
