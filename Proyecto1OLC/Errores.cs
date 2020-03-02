using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class Errores
    {
        private int Fila;
        private int Columna;
        private String Caracter;
        private String Descripcion;

        public Errores(int fila, int columna, String car, String descripcion)
        {
            this.Fila = fila;
            this.Columna = columna;
            this.Caracter = car;
            this.Descripcion = descripcion;
        }

        public int GetFila()
        {
            return Fila;
        }
        public int GetColumna()
        {
            return Columna;
        }
        public String GetCaracter()
        {
            return Caracter;
        }
        public String GetDescripcion()
        {
            return Descripcion;
        }
    }
}
