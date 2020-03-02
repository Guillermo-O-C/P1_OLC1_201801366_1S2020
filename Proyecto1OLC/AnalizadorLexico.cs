using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1OLC
{
    class AnalizadorLexico
    {
        private LinkedList<Errores> Error;
        private LinkedList<Token> Salida;
        private LinkedList<Conjunto> Conjuntos;
        private LinkedList<ExpresionRegular> ExpresionesRegulares;
        private int estado;
        private int fila;
        private int columna;
        private int columnaToken;
        private String auxlex;
        private boolean Lexemas;
        private int conjunto;
        private int expr;
        private String conjuntoID;
        private String conjuntoCONTENT;
        private String expresion;
        private String expID;

        public ListasAnalisis escanear(String entrada)
        {
            entrada = entrada + "#";
            Salida = new LinkedList<>();
            Error = new LinkedList<>();
            Conjuntos = new LinkedList<>();
            ExpresionesRegulares = new LinkedList<>();
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
            Character c;
            for (int i = 0; i < entrada.length() - 1; i++)
            {
                c = entrada.charAt(i);
                columna++;
                switch (estado)
                {
                    case 0:
                        if (Character.isDigit(c))
                        {
                            estado = 1;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (Character.isAlphabetic(c))
                        {
                            estado = 2;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (c.compareTo('"') == 0)
                        {
                            estado = 3;
                            auxlex += c;
                            columnaToken = columna;
                        }
                        else if (c.compareTo(':') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.DosPuntos);
                            if (conjunto == 1) conjunto = 2;
                        }
                        else if (c.compareTo(';') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            if (conjunto == 4)
                            {
                                Conjuntos.add(new Conjunto(conjuntoID, conjuntoCONTENT));
                                conjuntoID = "";
                                conjuntoCONTENT = "";
                                conjunto = 0;
                            }
                            else if (expr == 2 && conjunto == 0 && Lexemas == false)
                            {
                                ExpresionesRegulares.add(new ExpresionRegular(expID, expresion));
                                System.out.println("-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-" + expresion);
                                expr = 0;
                                expresion = "";
                                expID = "";
                            }
                            agregarToken(Token.Tipo.PuntoYComa);
                        }
                        else if (c.compareTo('{') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.AbreLlaves);
                        }
                        else if (c.compareTo('}') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.CierraLlaves);
                        }
                        else if (c.compareTo('.') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Punto);
                        }
                        else if (c.compareTo('/') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 5;
                        }
                        else if (c.compareTo('*') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Kleene);
                        }
                        else if (c.compareTo('-') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 6;
                        }
                        else if (c.compareTo('|') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Absoluto);
                        }
                        else if (c.compareTo('<') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 7;
                        }
                        else if (c.compareTo('!') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 9;
                        }
                        else if (c.compareTo('+') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Mas);
                        }
                        else if (c.compareTo('~') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.enie);
                        }
                        else if (c.compareTo(',') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.coma);
                        }
                        else if (c.compareTo('?') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            agregarToken(Token.Tipo.Interrogacion);
                        }
                        else if (c.compareTo('%') == 0)
                        {
                            auxlex += c;
                            columnaToken = columna;
                            estado = 10;
                        }
                        else if (c.compareTo('\n') == 0)
                        {
                            fila++;
                            columna = 0;
                            columnaToken = columna;
                        }
                        else if (c.compareTo(' ') == 0)
                        {
                        }
                        else if (c.compareTo('\t') == 0)
                        {
                        }
                        else
                        {
                            if (c.compareTo('#') == 0 && i == entrada.length() - 1)
                            {
                                System.out.println("hemos concluido el análisis con éxito " + auxlex);
                                agregarError(fila, columna, auxlex, "Desconocido");

                            }
                            else
                            {
                                System.out.println("Error Léxico con " + c);
                                agregarError(fila, columna, auxlex, "Desconocido");
                                agregarError(fila, columna, c.toString(), "Desconocido");
                                estado = 0;
                            }
                        }
                        break;
                    case 1:
                        if (Character.isDigit(c))
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
                        if (Character.isLetterOrDigit(c) | c.compareTo('_') == 0)
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
                        if (c.compareTo('"') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.cadena);
                        }
                        else
                        {
                            estado = 3;
                            auxlex += c;
                        }
                        break;
                    case 5:
                        if (c.compareTo('/') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.DobleBarra);
                            estado = 11;
                        }
                        else
                        {
                            i -= 1;
                            System.out.println("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.toString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 6:
                        if (c.compareTo('>') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.Flecha);
                            if (conjunto == 3) conjunto = 4;
                            if (expr == 1) expr = 2;
                        }
                        else
                        {
                            i -= 1;
                            System.out.println("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.toString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 7:
                        if (c.compareTo('!') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.BeginCM);
                            estado = 8;
                        }
                        else
                        {
                            i -= 1;
                            System.out.println("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.toString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 8:
                        if (c.compareTo('!') == 0)
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
                        if (c.compareTo('>') == 0)
                        {
                            auxlex += c;
                            agregarToken(Token.Tipo.EndCM);
                        }
                        else
                        {
                            i -= 1;
                            System.out.println("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.toString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 10:
                        if (c.compareTo('%') == 0)
                        {
                            auxlex += c;
                            Lexemas = true;
                            agregarToken(Token.Tipo.DoblePorcentaje);
                        }
                        else
                        {
                            i -= 1;
                            System.out.println("Error Léxico con " + c);
                            agregarError(fila, columna, auxlex, "Desconocido");
                            agregarError(fila, columna, c.toString(), "Desconocido");
                            estado = 0;
                        }
                        break;
                    case 11:
                        if (c.compareTo('\n') == 0)
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
                }
            }

            return new ListasAnalisis(Error, Salida, Conjuntos, ExpresionesRegulares);
        }


        public void agregarToken(Token.Tipo tipo)
        {
            if (conjunto == 4)
            {
                conjuntoCONTENT += auxlex;
            }
            if (expr == 2)
            {
                expresion += auxlex;
            }

            Salida.addLast(new Token(tipo, auxlex, fila, columnaToken));
            auxlex = "";
            estado = 0;
        }
        public void agregarError(int fila, int columna, String val, String descripcion)
        {
            if (!"".equals(val))
            {
                if (!" ".equals(val))
                {
                    Error.addLast(new Errores(fila, columna, val, descripcion));
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
                FileWriter File = new FileWriter("D:\\Tokens.html");
                File.write("<html>\n");
                File.write("<head>\n");
                File.write("<meta charset = \"utf -8\">\n");
                File.write("<title> Tabla de Tokens</title>\n");
                File.write("</head>\n");
                File.write("<body bgcolor=#000>\n");
                File.write("<table align = \"center\" style = \"width:80% ; font-family: consolas; \">\n");
                File.write("<tr bgcolor = \"F5C82E\" ><td colspan = 6 align = \"center\" ><h3> Tabla de Tokens</h3></td></tr>\n");
                File.write("<tr bgcolor = \"FBDD76\" style = \"width: 10%; text-align:center; \"><td style = \"width: 10%; text-align:center; \">#</td><td>ID</td><td style = \"width: 30% \">Lexema</td><td style =\"width: 30% \">Tipo</td><td style =\"width: 10% \">Fila</td><td style =\"width: 10% \">Columna</td></tr>\n");


                System.out.println("-------------------------Tabla de Tokens--------------------------");
                for (Token item : lista)
                {
                    i++;
                    System.out.println(i + " " + item.GetTipo() + " <--> " + item.GetVal());
                    File.write("<tr bgcolor = \"FBF3D6\"><td style = \"text-align:center; \">" + i + "</td><td style = \"text-align:center; \">" + item.GetID() + "</td><td>" + item.GetVal() + "</td><td>" + item.GetTipo() + "</td><td>" + item.GetFila() + "</td><td>" + item.GetColumna() + "</td></tr>\n");
                }
                System.out.println("-------------------------Fin Tabla de Tokens--------------------------");
                File.write("</table>\n");
                File.write("</body>\n");
                File.write("</html>\n");
                File.close();

            }
            catch (Exception e)
            {
            }
        }
        public static void imprimiListaErrores(LinkedList<Errores> laser)
        {
            if (laser != null)
            {
                try
                {
                    int i = 0;
                    FileWriter File = new FileWriter("D:\\Errores.html");
                    File.write("<html>\n");
                    File.write("<head>\n");
                    File.write("<meta charset = \"utf -8\">\n");
                    File.write("<title> Tabla de Erroes</title>\n");
                    File.write("</head>\n");
                    File.write("<body bgcolor=#000>\n");
                    File.write("<table align = \"center\" style = \"width:80% ; font-family: consolas; \">\n");
                    File.write("<tr bgcolor = \"4C868A\" ><td colspan = 5 align = \"center\" ><h3> Tabla de Errores</h3></td></tr>\n");
                    File.write("<tr bgcolor = \"4DD0C2\" style = \"width: 10%; text-align:center; \"><td style = \"width: 10%; text-align:center; \">#</td><td style = \"width: 10%; text-align:center; \">Fila</td><td style = \"width: 10%; text-align:center; \">Columna</td><td style = \"width: 30% \">Caracter</td><td style =\"width: 40% \">Descripción</td></tr>\n");
                    System.out.println("-------------------------Tabla de Errores--------------------------");
                    for (Errores item : laser)
                    {
                        i++;
                        File.write("<tr bgcolor = \"C0E8E0\"><td style = \"width: 10%; text-align:center; \">" + i + "</td><td style = \"width: 10%; text-align:center; \">" + item.GetFila() + "</td><td style = \"width: 10%; text-align:center; \">" + item.GetColumna() + "</td><td style = \"width: 30% \">" + item.GetCaracter() + "</td><td style =\"width: 40% \">" + item.GetDescripcion() + "</td></tr>\n");

                        System.out.println(i + " Fila no." + item.GetFila() + " Columna no." + item.GetColumna() + " caracter desconocido -->" + item.GetCaracter());
                    }
                    System.out.println("------------------------- Fin Tabla de Errores--------------------------");

                    File.write("</table>\n");
                    File.write("</body>\n");
                    File.write("</html>\n");
                    File.close();
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
                    agregarToken(Token.Tipo.Identificador);
                    break;
            }
        }

    }
