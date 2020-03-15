using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class Lexema
    {
        private string lexID;
        private string lexContent;
        private int fila;

        public Lexema(string lexID, string lexContent, int fila)
        {
            this.lexID = lexID;
            this.lexContent = lexContent;
            this.Fila = fila;
        }

        public string LexID { get => lexID; set => lexID = value; }
        public string LexContent { get => lexContent; set => lexContent = value; }
        public int Fila { get => fila; set => fila = value; }
    }
}
