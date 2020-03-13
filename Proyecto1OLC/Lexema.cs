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

        public Lexema(string lexID, string lexContent)
        {
            this.LexID = lexID;
            this.LexContent = lexContent;
        }

        public string LexID { get => lexID; set => lexID = value; }
        public string LexContent { get => lexContent; set => lexContent = value; }
    }
}
