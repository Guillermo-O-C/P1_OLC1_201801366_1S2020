﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class ListasAnalisis
    {

        private LinkedList<Errores> Error;
        private LinkedList<Token> Salida;
        private LinkedList<Conjunto> Conjuntos;
        private LinkedList<ExpresionRegular> ExpresionesRegulares;

        public ListasAnalisis(LinkedList<Errores> Error, LinkedList<Token> Salida, LinkedList<Conjunto> Conjuntos, LinkedList<ExpresionRegular> ExpresionesRegulares)
        {
            this.Error = Error;
            this.Salida = Salida;
            this.Conjuntos = Conjuntos;
            this.ExpresionesRegulares = ExpresionesRegulares;
        }

        public LinkedList<Errores> getError()
        {
            return Error;
        }

        public void setError(LinkedList<Errores> Error)
        {
            this.Error = Error;
        }

        public LinkedList<Token> getSalida()
        {
            return Salida;
        }

        public void setSalida(LinkedList<Token> Salida)
        {
            this.Salida = Salida;
        }

        public LinkedList<Conjunto> getConjuntos()
        {
            return Conjuntos;
        }

        public void setConjuntos(LinkedList<Conjunto> Conjuntos)
        {
            this.Conjuntos = Conjuntos;
        }

        public LinkedList<ExpresionRegular> getExpresionesRegulares()
        {
            return ExpresionesRegulares;
        }

        public void setExpresionesRegulares(LinkedList<ExpresionRegular> ExpresionesRegulares)
        {
            this.ExpresionesRegulares = ExpresionesRegulares;
        }

    }
}