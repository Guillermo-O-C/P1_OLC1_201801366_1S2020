using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class TerminalesTH
    {
        
        private string TerminalID;
        private Tipo TipoTerminal;

        public TerminalesTH(string terminalID, Tipo tipoTerminal)
        {
            TerminalID = terminalID;
            TipoTerminal = tipoTerminal;
        }

        public string TerminalID1 { get => TerminalID; set => TerminalID = value; }
        internal Tipo TipoTerminal1 { get => TipoTerminal; set => TipoTerminal = value; }

        public enum Tipo { cadena, conjunto, especial};
        
    }
}
