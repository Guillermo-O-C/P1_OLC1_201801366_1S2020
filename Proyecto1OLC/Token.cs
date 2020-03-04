using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class Token
    {
        public enum Tipo
        {
            Numero,
            Identificador,
            PR_CONJ,
            DobleBarra,
            BeginCM,
            EndCM,
            Flecha,
            AbreLlaves,
            CierraLlaves,
            DoblePorcentaje,
            DosPuntos,
            PuntoYComa,
            Punto,
            Absoluto,
            Kleene,
            Interrogacion,
            Mas,
            enie,
            cadena,
            coma,
            aceptacion,
            Epsilon
        }

        private Tipo tipoToken;
        private String valor;
        private int ID;
        private int columna;
        private int fila;

        public Token(Tipo tipoDelToken, String val, int fila, int columna)
        {
            this.tipoToken = tipoDelToken;
            this.valor = val;
            this.fila = fila;
            this.columna = columna;
        }

        public String GetVal()
        {
            return valor;
        }
        public int GetFila()
        {
            return fila;
        }
        public int GetColumna()
        {
            return columna;
        }

        public void setTipoToken(Tipo tipoToken)
        {
            this.tipoToken = tipoToken;
        }

        public void setValor(String valor)
        {
            this.valor = valor;
        }

        public String GetTipo()
        {
            switch (tipoToken)
            {
                case Tipo.Numero:
                    return "Numero";
                case Tipo.Identificador:
                    return "Identificador";
                case Tipo.PR_CONJ:
                    return "Palabra Reservada CONJ";
                case Tipo.DobleBarra:
                    return "Doble Barra";
                case Tipo.BeginCM:
                    return "Inicio Comentario";
                case Tipo.EndCM:
                    return "Fin Comentario";
                case Tipo.AbreLlaves:
                    return "Símbolo Abrir Llaves";
                case Tipo.CierraLlaves:
                    return "Símbolo Cerrar Llaves";
                case Tipo.DoblePorcentaje:
                    return "Doble Porcentaje";
                case Tipo.DosPuntos:
                    return "Dos Puntos";
                case Tipo.PuntoYComa:
                    return "Símbolo Punto y Coma";
                case Tipo.Punto:
                    return "Concatenacion";
                case Tipo.Absoluto:
                    return "Disyunción";
                case Tipo.Kleene:
                    return "Cerradura de Kleene";
                case Tipo.Interrogacion:
                    return "Signo de Interrogacion";
                case Tipo.Mas:
                    return "Simbolo Suma";
                case Tipo.enie:
                    return "Simbolo de la ñ";
                case Tipo.cadena:
                    return "Cadena";
                case Tipo.coma:
                    return "Símbolo coma";
                case Tipo.Flecha:
                    return "Flecha";
                case Tipo.aceptacion:
                    return "Simbolo Aceptación";
                case Tipo.Epsilon:
                    return "Epsilon";
                default:
                    return "desconocido";
            }
        }

        public int GetID()
        {
            switch (tipoToken)
            {
                case Tipo.Numero:
                    return 1;
                case Tipo.Identificador:
                    return 2;
                case Tipo.PR_CONJ:
                    return 3;
                case Tipo.DobleBarra:
                    return 4;
                case Tipo.BeginCM:
                    return 5;
                case Tipo.EndCM:
                    return 6;
                case Tipo.AbreLlaves:
                    return 7;
                case Tipo.CierraLlaves:
                    return 8;
                case Tipo.DoblePorcentaje:
                    return 9;
                case Tipo.DosPuntos:
                    return 10;
                case Tipo.PuntoYComa:
                    return 11;
                case Tipo.Punto:
                    return 12;
                case Tipo.Absoluto:
                    return 13;
                case Tipo.Kleene:
                    return 14;
                case Tipo.Interrogacion:
                    return 15;
                case Tipo.Mas:
                    return 16;
                case Tipo.enie:
                    return 17;
                case Tipo.cadena:
                    return 18;
                case Tipo.coma:
                    return 19;
                case Tipo.Flecha:
                    return 20;
                case Tipo.aceptacion:
                    return 21;
                case Tipo.Epsilon:
                    return 22;
                default:
                    return 23;
            }
        }

    }
}
