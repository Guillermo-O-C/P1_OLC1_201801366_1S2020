﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Proyecto1OLC
{
    class AnalizadorLexico
    {
        private LinkedList<Errores> Error;
        private LinkedList<Token> Salida;
        private LinkedList<Conjunto> Conjuntos;
        private LinkedList<ExpresionRegular> ExpresionesRegulares;
        private LinkedList<Lexema> LexemaList;
        private int estado;
        private int fila;
        private int columna;
        private int columnaToken;
        private String auxlex;
        private Boolean Lexemas;
        private int conjunto;
        private int expr;
        private int lex;
        private String conjuntoID;
        private String conjuntoCONTENT;
        private String expresion;
        private String expID;
        private String lexID;
        private String lexContent;

        public ListasAnalisis escanear(String entrada)
        {
            entrada = entrada + "#";
            Salida = new LinkedList<Token>();
            Error = new LinkedList<Errores>();
            Conjuntos = new LinkedList<Conjunto>();
            ExpresionesRegulares = new LinkedList<ExpresionRegular>();
            LexemaList = new LinkedList<Lexema>();
            fila = 1;
            columna = 0;
            estado = 0;
            auxlex = "";
            Lexemas = false;
            conjunto = 0;
            expr = 0;
            conjuntoID = "";
            conjuntoCONTENT = "";
            expresion = "";
            expID = "";
            lex = 0;
            lexID = "";
            lexContent = "";
            Char c;
            for (int i = 0; i < entrada.Length - 1; i++)
            {
                c = entrada.ElementAt(i);
                columna++;
                /*
                if (conjunto == 4 && !Salida.Last().GetTipo().Equals("Identificador"))
                {
                    if(conjunto == 4 && !Salida.Last().GetTipo().Equals("Numero"))
                    {
                        conjuntoCONTENT += c;
                    }
                }
                */
                if (conjunto == 4)
                {
                    estado = 12;
                }
                switch (estado)
                {
                    case 0:
                        if (Char.IsDigit(c))
                        {
                            estado = 1;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (Char.IsLetter(c))
                        {
                            estado = 2;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (c.CompareTo('"') == 0)
                        {
                            estado = 3;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (c.CompareTo(':') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.DosPuntos);
                            if (conjunto == 1) conjunto = 2;
                            if (lex == 1) lex = 2;
                        }
                        else if (c.CompareTo(';') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            if (conjunto == 5)
                            {
                                Conjuntos.AddLast(new Conjunto(conjuntoID, conjuntoCONTENT));
                                conjuntoID = "";
                                conjuntoCONTENT = "";
                                conjunto = 0;
                            }
                            else if (expr == 2 && conjunto == 0 && Lexemas == false)
                            {
                                ExpresionesRegulares.AddLast(new ExpresionRegular(expID, expresion));
                                Console.WriteLine("-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-" + expresion);
                                expr = 0;
                                expresion = "";
                                expID = "";
                            }
                            if (lex==2)
                            {
                                LexemaList.AddLast(new Lexema(lexID, lexContent, fila));
                                lexID = "";
                                lex = 0;
                                lexContent = "";
                            }
                            agregarToken(Token.Tipo.PuntoYComa);
                        }
                        else if (c.CompareTo('{') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.AbreLlaves);
                        }
                        else if (c.CompareTo('}') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.CierraLlaves);
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Punto);
                        }
                        else if (c.CompareTo('/') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 5;
                        }
                        else if (c.CompareTo('*') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Kleene);
                        }
                        else if (c.CompareTo('-') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 6;
                        }
                        else if (c.CompareTo('|') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Absoluto);
                        }
                        else if (c.CompareTo('<') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 7;
                        }
                        else if (c.CompareTo('!') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 9;
                        }
                        else if (c.CompareTo('+') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Mas);
                        }
                        else if (c.CompareTo('~') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.enie);
                        }
                        else if (c.CompareTo(',') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.coma);
                        }
                        else if (c.CompareTo('?') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Interrogacion);
                        }
                        else if (c.CompareTo('%') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 10;
                        }
                        else if (c.CompareTo('\\') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 13;
                        }
                        else if (c.CompareTo('[') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 14;
                        }
                        else if (c.CompareTo('\n') == 0)
                        {
                            fila++;
                            columna = 0;
                            columnaToken = columna;
                        }
                        else if (c.CompareTo(' ') == 0)
                        {
                        }
                        else if (c.CompareTo('\t') == 0)
                        {
                        }
                        else
                        {
                            if (c.CompareTo('#') == 0 && i == entrada.Length - 1)
                            {
                                Console.WriteLine("hemos concluido el análisis con éxito " + auxlex);
                                agregarError(fila, columna, auxlex, "Desconocido");

                            }
                            else
                            {
                                Console.WriteLine("Error Léxico con " + c);
                                agregarError(fila, columna, auxlex, "Desconocido");
                                agregarError(fila, columna, c.ToString(), "Desconocido");
                                estado = 0;
                            }
                        }
                        break;
                    case 1:
                        if (Char.IsDigit(c))
                        {
                            estado = 1;
                            auxlex += c;
                        }
                        else
                        {
                            agregarToken(Token.Tipo.Numero);
                            i -= 1;
                        }
                        break;
                    case 2:
                        if (Char.IsLetterOrDigit(c) | c.CompareTo('_') == 0)
                        {
                            estado = 2;
                            auxlex += c;
                        }
                        else
                        {
                            VerificarResevada();
                            i -= 1;
                        }
                        break;
                    case 3:
                        if(c.CompareTo('"') == 0)
                        {
                             if (lex == 2)
                            {
                                if (c.CompareTo('"') == 0 && entrada.ElementAt(i + 1).CompareTo(';') == 0 && entrada.ElementAt(i + 2).CompareTo('\n') == 0)
                                {
                                    auxlex += c;
                                    lexContent = auxlex;
                                    agregarToken(Token.Tipo.cadena);
                                }
                                else
                                {
                                    estado = 3;
                                    auxlex += c;
                                }
                            }
                            else
                            {
                                auxlex += c;
                                agregarToken(Token.Tipo.cadena);
                            }
                        }
                        else
                        {
                            estado = 3;
                            auxlex += c;
                        }
                        break;
                    case 5:
                        if (c.CompareTo('/') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.DobleBarra);
                            estado = 11;
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 6:
                        if (c.CompareTo('>') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.Flecha);
                            if (conjunto == 3) conjunto = 4;
                            if (expr == 1) expr = 2;
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 7:
                        if (c.CompareTo('!') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.BeginCM);
                            estado = 8;
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 8:
                        if (c.CompareTo('!') == 0)
                        {
                            agregarToken(Token.Tipo.cadena);
                            i -= 1;
                        }
                        else
                        {
                            estado = 8;
                            auxlex += c;
                        }
                        break;
                    case 9:
                        if (c.CompareTo('>') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.EndCM);
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 10:
                        if (c.CompareTo('%') == 0)
                        {
                            auxlex += c;
                            Lexemas = true;
                            agregarToken(Token.Tipo.DoblePorcentaje);
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 11:
                        if (c.CompareTo('\n') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.cadena);
                        }
                        else
                        {
                            estado = 11;
                            auxlex += c;
                        }
                        break;
                    case 12:
                        if (c.CompareTo(';') == 0)
                        {
                            //auxlex += c;
                            conjunto = 5;
                            estado = 0;
                            i -= 1;
                        }
                        else
                        {
                            estado = 12;
                            if(c.CompareTo(' ') == 0)
                            {
                                
                            }
                            else
                            {
                                conjuntoCONTENT += c;
                            }
                        }
                        break;
                    case 13://Caracteres Especiales
                        if (c.CompareTo('n') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.Salto);
                        }
                        else if (c.CompareTo('\'') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.ComillaSimple);
                        }
                        else if (c.CompareTo('"') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.ComillaDoble);
                        }
                        else if (c.CompareTo('t') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.Tabulacion);
                        }
                        else
                        {
                            i -= 1;
                            Console.WriteLine("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.ToString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 14://Todo //1
                        if (c.CompareTo(':')==0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.TodoBegin);
                            estado = 15;
                        }
                        break;
                    case 15://2
                        if(c.CompareTo('\\') == 0 && entrada.ElementAt(i + 1).CompareTo('t') == 0)
                        {
                            if (auxlex != "")
                            {
                                agregarToken(Token.Tipo.cadena);
                                estado = 17;
                            }
                            auxlex = "\\t";
                            agregarToken(Token.Tipo.Tabulacion);
                            i+= 1;
                            estado = 15;
                        }else if (c.CompareTo('\\') == 0 && entrada.ElementAt(i + 1).CompareTo('\'') == 0)
                        {
                            if (auxlex != "")
                            {
                                agregarToken(Token.Tipo.cadena);
                            }
                            auxlex = "\\\'";
                            agregarToken(Token.Tipo.ComillaSimple);
                            i += 1;
                            estado = 15;
                        }
                        else if (c.CompareTo('\\') == 0 && entrada.ElementAt(i + 1).CompareTo('"') == 0)
                        {
                            if (auxlex != "")
                            {
                                agregarToken(Token.Tipo.cadena);
                            }
                            auxlex = "\\\"";
                            agregarToken(Token.Tipo.ComillaDoble);
                            i += 1;
                            estado = 15;
                        }
                        else if (c.CompareTo(':') == 0 && entrada.ElementAt(i + 1).CompareTo(']') == 0)
                        {
                            if (auxlex != "")
                            {
                                agregarToken(Token.Tipo.cadena);
                            }
                            auxlex = ":]";
                            agregarToken(Token.Tipo.TodoEnd);
                            i += 1;
                        }
                        else
                        {
                            auxlex += c;
                        }
                        break;
                }
            }

            return new ListasAnalisis(Error, Salida, Conjuntos, ExpresionesRegulares, LexemaList);
        }


        public void agregarToken(Token.Tipo tipo)
        {
            if (expr == 2)
            {
                expresion += auxlex;
            }

            Salida.AddLast(new Token(tipo, auxlex, fila, columnaToken));
            auxlex = "";
            estado = 0;
        }
        public void agregarError(int fila, int columna, String val, String descripcion)
        {
            if (!"".Equals(val))
            {
                if (!" ".Equals(val))
                {
                    Error.AddLast(new Errores(fila, columna, val, descripcion));
                    auxlex = "";
                    estado = 0;
                }

            }

        }

        public void imprimiListaToken(LinkedList<Token> lista)
        {
            try
            {
                int i = 0;
                StreamWriter File = new StreamWriter("D:\\Tokens.html");
                File.Write("<html>\n");
                File.Write("<head>\n");
                File.Write("<meta charset = \"utf -8\">\n");
                File.Write("<title> Tabla de Tokens</title>\n");
                File.Write("</head>\n");
                File.Write("<body bgcolor=#000>\n");
                File.Write("<table align = \"center\" style = \"width:80% ; font-family: consolas; \">\n");
                File.Write("<tr bgcolor = \"F5C82E\" ><td colspan = 6 align = \"center\" ><h3> Tabla de Tokens</h3></td></tr>\n");
                File.Write("<tr bgcolor = \"FBDD76\" style = \"width: 10%; text-align:center; \"><td style = \"width: 10%; text-align:center; \">#</td><td>ID</td><td style = \"width: 30% \">Lexema</td><td style =\"width: 30% \">Tipo</td><td style =\"width: 10% \">Fila</td><td style =\"width: 10% \">Columna</td></tr>\n");


                Console.WriteLine("-------------------------Tabla de Tokens--------------------------");
                foreach (Token item in lista)
                {
                    i++;
                    Console.WriteLine(i + " " + item.GetTipo() + " <--> " + item.GetVal());
                    File.Write("<tr bgcolor = \"FBF3D6\"><td style = \"text-align:center; \">" + i + "</td><td style = \"text-align:center; \">" + item.GetID() + "</td><td>" + item.GetVal() + "</td><td>" + item.GetTipo() + "</td><td>" + item.GetFila() + "</td><td>" + item.GetColumna() + "</td></tr>\n");
                }
                Console.WriteLine("-------------------------Fin Tabla de Tokens--------------------------");
                File.Write("</table>\n");
                File.Write("</body>\n");
                File.Write("</html>\n");
                File.Close();

            }
            catch (Exception e)
            {
            }

            try
            {
                Document ReporteTokens = new Document();
                PdfWriter.GetInstance(ReporteTokens, new FileStream("D:\\Tokens.pdf", FileMode.Create));
                ReporteTokens.Open();
                ReporteTokens.Add(new Paragraph("Reporte de Tokens", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 15, BaseColor.BLACK)));
                ReporteTokens.Add(new Paragraph("\n\n"));
                PdfPTable Tabla = new PdfPTable(6);
                Tabla.WidthPercentage = 100;
                PdfPCell title = new PdfPCell(new Paragraph("TOKENS"));
                title.Colspan = 6;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                title.BackgroundColor = BaseColor.WHITE;
                Tabla.AddCell(title);
                Paragraph column1 = new Paragraph("#", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph column2 = new Paragraph("ID", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph column3 = new Paragraph("Lexema", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph column4 = new Paragraph("Tipo", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph column5 = new Paragraph("Fila", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph column6 = new Paragraph("Columna", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Tabla.AddCell(column1);
                Tabla.AddCell(column2);
                Tabla.AddCell(column3);
                Tabla.AddCell(column4);
                Tabla.AddCell(column5);
                Tabla.AddCell(column6);
                int i = 0;
                foreach (Token item in lista)
                {
                    i++;
                    Tabla.AddCell(i.ToString());
                    Tabla.AddCell(item.GetID().ToString());
                    Tabla.AddCell(item.GetVal());
                    Tabla.AddCell(item.GetTipo());
                    Tabla.AddCell(item.GetFila().ToString());
                    Tabla.AddCell(item.GetColumna().ToString());
                }
                ReporteTokens.Add(Tabla);
                ReporteTokens.Close();
            }catch(Exception e)
            {

            }
        }
        public void imprimiListaErrores(LinkedList<Errores> laser)
        {
            if (laser != null)
            {
                try
                {
                    int i = 0;

                    StreamWriter File = new StreamWriter("D:\\Errores.html");
                    File.Write("<html>\n");
                    File.Write("<head>\n");
                    File.Write("<meta charset = \"utf -8\">\n");
                    File.Write("<title> Tabla de Erroes</title>\n");
                    File.Write("</head>\n");
                    File.Write("<body bgcolor=#000>\n");
                    File.Write("<table align = \"center\" style = \"width:80% ; font-family: consolas; \">\n");
                    File.Write("<tr bgcolor = \"4C868A\" ><td colspan = 5 align = \"center\" ><h3> Tabla de Errores</h3></td></tr>\n");
                    File.Write("<tr bgcolor = \"4DD0C2\" style = \"width: 10%; text-align:center; \"><td style = \"width: 10%; text-align:center; \">#</td><td style = \"width: 10%; text-align:center; \">Fila</td><td style = \"width: 10%; text-align:center; \">Columna</td><td style = \"width: 30% \">Caracter</td><td style =\"width: 40% \">Descripción</td></tr>\n");
                    Console.WriteLine("-------------------------Tabla de Errores--------------------------");
                    foreach (Errores item in laser)
                    {
                        i++;
                        File.Write("<tr bgcolor = \"C0E8E0\"><td style = \"width: 10%; text-align:center; \">" + i + "</td><td style = \"width: 10%; text-align:center; \">" + item.GetFila() + "</td><td style = \"width: 10%; text-align:center; \">" + item.GetColumna() + "</td><td style = \"width: 30% \">" + item.GetCaracter() + "</td><td style =\"width: 40% \">" + item.GetDescripcion() + "</td></tr>\n");

                        Console.WriteLine(i + " Fila no." + item.GetFila() + " Columna no." + item.GetColumna() + " caracter desconocido -->" + item.GetCaracter());
                    }
                    Console.WriteLine("------------------------- Fin Tabla de Errores--------------------------");

                    File.Write("</table>\n");
                    File.Write("</body>\n");
                    File.Write("</html>\n");
                    File.Close();
                }
                catch (Exception e)
                {
                }
                try
                {
                    Document ReporteErrores = new Document();
                    PdfWriter.GetInstance(ReporteErrores, new FileStream("D:\\Errores.pdf", FileMode.Create));
                    ReporteErrores.Open();
                    ReporteErrores.Add(new Paragraph("Reporte de Errores", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 15, BaseColor.BLACK)));
                    ReporteErrores.Add(new Paragraph("\n\n"));
                    PdfPTable Tabla = new PdfPTable(5);
                    Tabla.WidthPercentage = 100;
                    PdfPCell title = new PdfPCell(new Paragraph("ERRORES"));
                    title.Colspan = 6;
                    title.HorizontalAlignment = Element.ALIGN_CENTER;
                    title.BackgroundColor = BaseColor.WHITE;
                    Tabla.AddCell(title);
                    Paragraph column1 = new Paragraph("#", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                    Paragraph column2 = new Paragraph("Fila", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                    Paragraph column3 = new Paragraph("Columna", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                    Paragraph column4 = new Paragraph("Caracter", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                    Paragraph column5 = new Paragraph("Descripción", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                    Tabla.AddCell(column1);
                    Tabla.AddCell(column2);
                    Tabla.AddCell(column3);
                    Tabla.AddCell(column4);
                    Tabla.AddCell(column5);
                    int i = 0;
                    foreach (Errores item in laser)
                    {
                        i++;
                        Tabla.AddCell(i.ToString());
                        Tabla.AddCell(item.GetFila().ToString());
                        Tabla.AddCell(item.GetColumna().ToString());
                        Tabla.AddCell(item.GetCaracter());
                        Tabla.AddCell(item.GetDescripcion());
                    }
                    ReporteErrores.Add(Tabla);
                    ReporteErrores.Close();
                }
                catch (Exception e)
                {
                   
                }
            }
        }
        public void VerificarResevada()
        {
            switch (auxlex)
            {
                case "CONJ":
                    agregarToken(Token.Tipo.PR_CONJ);
                    conjunto = 1;
                    break;
                default:
                    if (conjunto == 2)
                    {
                        conjunto = 3;
                        conjuntoID = auxlex;
                    }
                    if (expr == 0 && conjunto == 0)
                    {
                        expr = 1;
                        expID = auxlex;
                    }
                    if (lex == 0 && expr !=0)
                    {
                        lex = 1;
                        lexID = auxlex;
                    }
                    agregarToken(Token.Tipo.Identificador);
                    break;
            }
        }

    }
}
