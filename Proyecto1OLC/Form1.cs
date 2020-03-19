using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Proyecto1OLC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private RichTextBox GetRichTextBox()
        {
            RichTextBox rtb = null;
            TabPage tp = tabControl1.SelectedTab;

            if (tp != null)
            {
                rtb = tp.Controls[0] as RichTextBox;
            }


            return rtb;
        }
        int con = 0;
        int nodos = 2;
        int nodosAFN = 0;
        int estados;
        LinkedList<AnalizadorGenerico> Tablas;
        LinkedList<string> Terminales;
        LinkedList<LinkedList<NodoAFN>> States;
        LinkedList<TransicionesAFD> TablaDeTransiciones;
        LinkedList<int> Aceptacion;
        LinkedList<TerminalesTH> TerminalesList;
        LinkedList<Conjunto> FileConjuntos;
        Document Tokens, Errors;
        PdfPTable TablaTokens, TablaErrores;
        private void NuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            con++;
            TabPage tp = new TabPage("Pestaña " + con);
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;

            tp.Controls.Add(rtb);
            tabControl1.TabPages.Add(tp);

        }
        private void AbrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Stream myStream;


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Regular Expresion (*.er)|*.er|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {

                    string strfilename = openFileDialog1.FileName;
                    string filetext = File.ReadAllText(strfilename);
                    String nombrePestania = Path.GetFileName(strfilename);
                    tabControl1.SelectedTab.Text = nombrePestania;
                    //MessageBox.Show(filename);
                    if (GetRichTextBox() == null)
                    {
                        MessageBox.Show("Debes de crear primero una pestaña", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        GetRichTextBox().Text = filetext;
                    }

                }
            }

        }
        private void GuardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog guardar = new SaveFileDialog();
            if (guardar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //s = null;
                    using (Stream s = File.Open(guardar.FileName, FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(GetRichTextBox().Text);
                    }
                }
                catch (IOException w)
                {
                    MessageBox.Show("No se pudo guardar el archivo, inténtelo de nuevo");
                    //     throw;
                }

            }
        }
        LinkedList<Nodo> SetType(LinkedList<Token> lista)
        {
            Boolean TodoFinalizado = false;
            LinkedList<Nodo> Ramas = new LinkedList<Nodo>();
            foreach (Token objeto in lista)
            {
                if (!TodoFinalizado)
                {
                    if ("Concatenacion".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Operador_Binario, objeto, nodos));
                    }
                    else if ("Disyunción".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Operador_Binario, objeto, nodos));
                    }
                    else if ("Cerradura de Kleene".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Operador_Unario, objeto, nodos));
                    }
                    else if ("Signo de Interrogacion".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Operador_Unario, objeto, nodos));
                    }
                    else if ("Simbolo Suma".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Operador_Unario, objeto, nodos));
                    }
                    else if ("Cadena".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal().Substring(1, objeto.GetVal().Length - 2)))
                        {
                            Terminales.AddLast(objeto.GetVal().Substring(1, objeto.GetVal().Length - 2));
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal().Substring(1, objeto.GetVal().Length - 2), TerminalesTH.Tipo.cadena));
                        }
                        objeto.setValor(objeto.GetVal().Substring(1, objeto.GetVal().Length - 2));
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Identificador".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal()))
                        {
                            Terminales.AddLast(objeto.GetVal());
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal(), TerminalesTH.Tipo.conjunto));
                        }
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Salto".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal()))
                        {
                            Terminales.AddLast(objeto.GetVal());
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal(), TerminalesTH.Tipo.especial));
                        }
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Tabulacion".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal()))
                        {
                            Terminales.AddLast(objeto.GetVal());
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal(), TerminalesTH.Tipo.especial));
                        }
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Comilla Simple".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal()))
                        {
                            Terminales.AddLast(objeto.GetVal());
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal(), TerminalesTH.Tipo.especial));
                        }
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Comilla Doble".Equals(objeto.GetTipo()))
                    {
                        if (!Terminales.Contains(objeto.GetVal()))
                        {
                            Terminales.AddLast(objeto.GetVal());
                            TerminalesList.AddLast(new TerminalesTH(objeto.GetVal(), TerminalesTH.Tipo.especial));
                        }
                        Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                    }
                    else if ("Todo Begin".Equals(objeto.GetTipo()))
                    {
                        Ramas.AddLast(ProcessTodo(objeto, lista));
                        TodoFinalizado = true;
                    }
                    nodos++;
                }else if("Todo End".Equals(objeto.GetTipo()))
                {
                    TodoFinalizado = false;
                }
            }
            return Ramas;
        }
        int NodoID(Token begin, LinkedList<Token> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista.ElementAt(i).Equals(begin))
                {
                    return i;
                }
            }
            return 0;
        }
        int FindTodoEnd(Token begin, LinkedList<Token> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista.ElementAt(i).GetTipo().Equals("Todo End"))
                {
                    return i;
                }
            }
            return 0;
        }
        Nodo ProcessTodo(Token begin, LinkedList<Token> lista)
        {
            int id = NodoID(begin, lista);
            int end = FindTodoEnd(begin, lista);
            LinkedList<Nodo> Hojas = new LinkedList<Nodo>();
            for (int i = id+1; i < end; i++)
            {
                if ("Cadena".Equals(lista.ElementAt(i).GetTipo()))
                {
                    if (!Terminales.Contains(lista.ElementAt(i).GetVal()))
                    {
                        Terminales.AddLast(lista.ElementAt(i).GetVal());
                        TerminalesList.AddLast(new TerminalesTH(lista.ElementAt(i).GetVal(), TerminalesTH.Tipo.cadena));
                    }
                    lista.ElementAt(i).setValor(lista.ElementAt(i).GetVal());
                    Hojas.AddLast(new Nodo(Nodo.Tipo.Terminal, lista.ElementAt(i), nodos));
                    nodos++;
                }
                else
                {
                    if (!Terminales.Contains(lista.ElementAt(i).GetVal()))
                    {
                        Terminales.AddLast(lista.ElementAt(i).GetVal());
                        TerminalesList.AddLast(new TerminalesTH(lista.ElementAt(i).GetVal(), TerminalesTH.Tipo.especial));
                    }
                    Hojas.AddLast(new Nodo(Nodo.Tipo.Terminal, lista.ElementAt(i), nodos));
                    nodos++;
                }
            }
            int hojasSize = Hojas.Count;
            for(int i =0; i < hojasSize-1; i++)
            {
                Hojas.AddFirst(new Nodo(Nodo.Tipo.Operador_Binario, new Token(Token.Tipo.Punto, ".", 0, 0), nodos));
                nodos++;
            }
            return GenerarArbol(Hojas);
        }
        Nodo GenerarArbol(LinkedList<Nodo> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                //OTT
                if (lista.ElementAt(i).getTipo() == Nodo.Tipo.Operador_Binario &&
                   lista.ElementAt(i + 1).getTipo() == Nodo.Tipo.Terminal &&
                   lista.ElementAt(i + 2).getTipo() == Nodo.Tipo.Terminal)
                {
                    lista.ElementAt(i).setLeft(lista.ElementAt(i + 1));
                    lista.ElementAt(i + 1).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i).setRight(lista.ElementAt(i + 2));
                    lista.ElementAt(i + 2).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i).setTipo(Nodo.Tipo.Terminal);
                    lista.Remove(lista.ElementAt(i + 2));
                    lista.Remove(lista.ElementAt(i + 1));
                    GenerarArbol(lista);
                    break;
                }//OUTUT
                else if (lista.ElementAt(i).getTipo() == Nodo.Tipo.Operador_Binario &&
                   lista.ElementAt(i + 1).getTipo() == Nodo.Tipo.Operador_Unario &&
                   lista.ElementAt(i + 2).getTipo() == Nodo.Tipo.Terminal &&
                   lista.ElementAt(i + 3).getTipo() == Nodo.Tipo.Operador_Unario &&
                   lista.ElementAt(i + 4).getTipo() == Nodo.Tipo.Terminal)
                {
                    lista.ElementAt(i).setLeft(lista.ElementAt(i + 1));
                    lista.ElementAt(i + 1).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i + 1).setLeft(lista.ElementAt(i + 2));
                    lista.ElementAt(i + 2).setPrevious(lista.ElementAt(i + 1));
                    lista.ElementAt(i).setRight(lista.ElementAt(i + 3));
                    lista.ElementAt(i + 3).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i + 3).setLeft(lista.ElementAt(i + 4));
                    lista.ElementAt(i + 4).setPrevious(lista.ElementAt(i + 3));
                    lista.ElementAt(i).setTipo(Nodo.Tipo.Terminal);
                    lista.Remove(lista.ElementAt(i + 4));
                    lista.Remove(lista.ElementAt(i + 3));
                    lista.Remove(lista.ElementAt(i + 2));
                    lista.Remove(lista.ElementAt(i + 1));
                    GenerarArbol(lista);
                    break;
                }//OTUT
                else if (lista.ElementAt(i).getTipo() == Nodo.Tipo.Operador_Binario &&
                   lista.ElementAt(i + 1).getTipo() == Nodo.Tipo.Terminal &&
                   lista.ElementAt(i + 2).getTipo() == Nodo.Tipo.Operador_Unario &&
                   lista.ElementAt(i + 3).getTipo() == Nodo.Tipo.Terminal)
                {
                    lista.ElementAt(i).setLeft(lista.ElementAt(i + 1));
                    lista.ElementAt(i + 1).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i).setRight(lista.ElementAt(i + 2));
                    lista.ElementAt(i + 2).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i + 2).setLeft(lista.ElementAt(i + 3));
                    lista.ElementAt(i + 3).setPrevious(lista.ElementAt(i + 2));
                    lista.ElementAt(i).setTipo(Nodo.Tipo.Terminal);
                    lista.Remove(lista.ElementAt(i + 3));
                    lista.Remove(lista.ElementAt(i + 2));
                    lista.Remove(lista.ElementAt(i + 1));
                    GenerarArbol(lista);
                    break;
                }//OUTT
                else if (lista.ElementAt(i).getTipo() == Nodo.Tipo.Operador_Binario &&
                   lista.ElementAt(i + 1).getTipo() == Nodo.Tipo.Operador_Unario &&
                   lista.ElementAt(i + 2).getTipo() == Nodo.Tipo.Terminal &&
                   lista.ElementAt(i + 3).getTipo() == Nodo.Tipo.Terminal)
                {
                    lista.ElementAt(i).setLeft(lista.ElementAt(i + 1));
                    lista.ElementAt(i + 1).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i + 1).setLeft(lista.ElementAt(i + 2));
                    lista.ElementAt(i + 2).setPrevious(lista.ElementAt(i + 1));
                    lista.ElementAt(i).setRight(lista.ElementAt(i + 3));
                    lista.ElementAt(i + 3).setPrevious(lista.ElementAt(i));
                    lista.ElementAt(i).setTipo(Nodo.Tipo.Terminal);
                    lista.Remove(lista.ElementAt(i + 3));
                    lista.Remove(lista.ElementAt(i + 2));
                    lista.Remove(lista.ElementAt(i + 1));
                    GenerarArbol(lista);
                    break;
                }
            }
            return lista.ElementAt(0);
        }
        void ThompsonTree(Nodo raiz)
        {
            //Creo que debe ir antes para que vaya cambiando todo desde un inicio
            if (raiz.getValue().GetTipo().Equals("Signo de Interrogacion")) {
                //Sustitucuon del a? por un a|E
                raiz.setTipo(Nodo.Tipo.Operador_Binario);
                raiz.setValue(new Token(Token.Tipo.Absoluto, "|", 0, 0));
                Nodo RightSon = new Nodo(Nodo.Tipo.Epsilon, new Token(Token.Tipo.Epsilon, "Epsilon", 0, 0), nodos);
                nodos++;
                raiz.setRight(RightSon);
                RightSon.setPrevious(raiz);
            }
            if (raiz.getValue().GetTipo().Equals("Simbolo Suma"))
            {
                //Convierte a+ en a.a*
                raiz.setTipo(Nodo.Tipo.Operador_Binario);
                raiz.setValue(new Token(Token.Tipo.Punto, ".", 0, 0));
                //Copiando el hijo izquierdo
                Nodo LeftSon = new Nodo(raiz.getLeft().getTipo(), raiz.getLeft().getValue(), nodos);
                nodos++;
                CloneNode(raiz.getLeft(), LeftSon);
                Nodo RightSon = new Nodo(Nodo.Tipo.Operador_Unario, new Token(Token.Tipo.Kleene, "*", 0, 0), nodos);
                nodos++;
                raiz.setRight(RightSon);
                RightSon.setPrevious(raiz);
                RightSon.setLeft(LeftSon);
                LeftSon.setPrevious(RightSon);
            }
            if (raiz.getLeft() != null)
            {
                ThompsonTree(raiz.getLeft());
            }
            //estaba en esta posicion
            if (raiz.getRight() != null)
            {
                ThompsonTree(raiz.getRight());
            }
        }
        void CloneNode(Nodo raiz, Nodo Copia)
        {
            if (raiz.getLeft() != null)
            {
                Nodo temp = new Nodo(raiz.getTipo(), raiz.getLeft().getValue(), nodos);
                nodos++;
                Copia.setLeft(temp);
                temp.setPrevious(Copia);
                CloneNode(raiz.getLeft(), temp);
            }
            if (raiz.getRight() != null)
            {
                Nodo temp = new Nodo(raiz.getTipo(), raiz.getRight().getValue(), nodos);
                nodos++;
                Copia.setRight(temp);
                temp.setPrevious(Copia);
                CloneNode(raiz.getRight(), temp);
            }
            /*
            Nodo temporal = new Nodo(raiz.getTipo(), raiz.getLeft().getValue(), nodos);
            Copia.setLeft(temporal);
            temporal.setPrevious(Copia);
            */

        }
        Boolean IsLeftSon(Nodo raiz, Nodo hijo) {
            if (raiz.getLeft() == hijo)
            {
                return true;
            }
            return false;
        }
        private void AnalizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsoleOutPut.Text = "";
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            Tablas = new LinkedList<AnalizadorGenerico>();
            FileConjuntos = new LinkedList<Conjunto>();
            AnalizadorLexico analizador = new AnalizadorLexico();
            ListasAnalisis resultado = analizador.escanear(GetRichTextBox().Text);
            DefineConjuntos(resultado.getConjuntos());
            FileConjuntos = resultado.getConjuntos();
            analizador.imprimiListaToken(resultado.getSalida());
            analizador.imprimiListaErrores(resultado.getError());
            treeView1.Nodes.Clear();
            foreach (ExpresionRegular Expr in resultado.getExpresionesRegulares())
            {
                treeView1.Nodes.Add(Expr.getExpID());
                //No reiniciaba conteo
                nodos = 2;
                nodosAFN = 0;
                estados = 0;
                Terminales = new LinkedList<string>();
                Aceptacion = new LinkedList<int>();
                TablaDeTransiciones = new LinkedList<TransicionesAFD>();
                States = new LinkedList<LinkedList<NodoAFN>>();
                TerminalesList = new LinkedList<TerminalesTH>();
                ListasAnalisis expresiones = analizador.escanear(Expr.getExpresion());
                LinkedList<Nodo> temporal = SetType(expresiones.getSalida());
                temporal.AddFirst(new Nodo(Nodo.Tipo.Operador_Binario, new Token(Token.Tipo.Punto, ".", 0, 0), 0));
                temporal.AddLast(new Nodo(Nodo.Tipo.Terminal, new Token(Token.Tipo.aceptacion, "#", 0, 0), 1));
                Nodo cabeza = GenerarArbol(temporal);
                Console.WriteLine(cabeza.getValue().GetVal());
                Graficar(cabeza, Expr.getExpID());
                ThompsonTree(cabeza);
                Graficar(cabeza, Expr.getExpID() + "_Th");
                AFN automata = Thompson(cabeza.getLeft());
                LinkedList<int> Recorridos = new LinkedList<int>();
                GraphAFN(GenGraphAFN(automata.getPrimero(), Recorridos), Expr.getExpID() + "_AFN");
                LinkedList<NodoAFN> NodesList = new LinkedList<NodoAFN>();
                AFN_Nodes(automata.getPrimero(), NodesList);
                Console.WriteLine(NodesList.Count);
                LinkedList<NodoAFN> conjunto = new LinkedList<NodoAFN>();
                conjunto.AddLast(NodesList.First());
                LinkedList<NodoAFN> EstadoInicial = new LinkedList<NodoAFN>();
                EstadoInicial.AddLast(automata.getPrimero());
                Cerradura(EstadoInicial);
                int o =States.Count;
                for (int i = 0; i < estados; i++)
                {
                    Cerradura(States.ElementAt(i));
                }
                int es = TablaDeTransiciones.Count;
                GrapfAFD(TablaDeTransiciones, Expr.getExpID());
                printTransTable(TablaDeTransiciones, Expr.getExpID());
                Tablas.AddLast(new AnalizadorGenerico(Expr.getExpID(), TablaDeTransiciones, TerminalesList,Terminales, Aceptacion));
            }
            Tokens = new Document();
            Errors = new Document();
            PdfWriter.GetInstance(Tokens, new FileStream("D:\\TokensDeLexemas.pdf", FileMode.Create));
            PdfWriter.GetInstance(Errors, new FileStream("D:\\ErroresDeLexemas.pdf", FileMode.Create));
            Tokens.Open();
            Errors.Open();
            foreach (Lexema lexema in resultado.Lexemas1)
            {

                TablaTokens = new PdfPTable(5);
                TablaErrores = new PdfPTable(3);
                TablaTokens.WidthPercentage = 100;
                PdfPCell TokenTableTitle = new PdfPCell(new Paragraph("Tokens"));
                PdfPCell ErroresTableTitle = new PdfPCell(new Paragraph("Errores"));
                TokenTableTitle.Colspan = 8;
                TokenTableTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                TokenTableTitle.BackgroundColor = BaseColor.ORANGE;
                TablaTokens.AddCell(TokenTableTitle);
                ErroresTableTitle.Colspan = 8;
                ErroresTableTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ErroresTableTitle.BackgroundColor = BaseColor.ORANGE;
                TablaErrores.AddCell(ErroresTableTitle);
                Paragraph columna1 = new Paragraph("#", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna2 = new Paragraph("Nombre", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna3 = new Paragraph("Valor", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna4 = new Paragraph("Columna", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna5 = new Paragraph("Fila", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                TablaTokens.AddCell(columna1);
                TablaTokens.AddCell(columna2);
                TablaTokens.AddCell(columna3);
                TablaTokens.AddCell(columna4);
                TablaTokens.AddCell(columna5);
                Paragraph columna22 = new Paragraph("ERROR", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna33 = new Paragraph("Columna", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                Paragraph columna44 = new Paragraph("Fila", FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12));
                TablaErrores.AddCell(columna22);
                TablaErrores.AddCell(columna33);
                TablaErrores.AddCell(columna44);
                foreach (AnalizadorGenerico analizadorGenerico in Tablas)
                {
                    if (lexema.LexID.Equals(analizadorGenerico.ExprID1)){
                        Tokens.Add(new Paragraph(analizadorGenerico.ExprID1 +" analizando "+lexema.LexContent));
                        Errors.Add(new Paragraph(analizadorGenerico.ExprID1 + " analizando " + lexema.LexContent));
                        EvaluarLexema(analizadorGenerico.ExprID1, analizadorGenerico.TablaTransiciones1, analizadorGenerico.TerminalesTH1,
                                      lexema.LexContent.Substring(1, lexema.LexContent.Length - 2), analizadorGenerico.Aceptacion1, lexema.Fila);
                    }
                }
                Tokens.Add(new Paragraph("\n \n"));
                Errors.Add(new Paragraph("\n \n"));
                Tokens.Add(TablaTokens);
                Errors.Add(TablaErrores);
            }
            Tokens.Close();
            Errors.Close();
        }
        void Graficar(Nodo raiz, string nombre)
        {
            string entrada = "digraph G {\n nodesep=0.3;\n ranksep=0.2;\n    margin=0.1;\n   node [shape=circle];\n  edge [arrowsize=0.8];";
            entrada += NextNodos(raiz) + "}";
            System.IO.File.WriteAllText(@"D:\\" + nombre + ".txt", entrada);
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Descargas\graphviz-2.38\release\bin\dot.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Programas y +\graphviz-2.38\release\bin\dot.exe");
            startInfo.Arguments = "-Tpng \"D:\\" + nombre + ".txt\" -o \"D:\\" + nombre + ".png\"";
            Process.Start(startInfo);
        }
        void GraphAFN(string contenido, string nombre)
        {
            string entrada = "digraph G {\n rankdir=LR nodesep=0.3;\n ranksep=0.2;\n    margin=0.1;\n   node [shape=circle];\n  edge [arrowsize=0.8];";
            entrada += contenido + "}";
            System.IO.File.WriteAllText(@"D:\\" + nombre + ".txt", entrada);
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Descargas\graphviz-2.38\release\bin\dot.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Programas y +\graphviz-2.38\release\bin\dot.exe");
            startInfo.Arguments = "-Tpng \"D:\\" + nombre + ".txt\" -o \"D:\\" + nombre + ".png\"";
            Process.Start(startInfo);
        }
        String NextNodos(Nodo Central)
        {
            String content = "";
            content += "\"" + Central.getID() + "_" + Central.getValue().GetVal() + "\" [label=\"" + Central.getValue().GetVal() + "\"]";
            if (Central.getLeft() != null)
            {
                content += "\"" + Central.getID() + "_" + Central.getValue().GetVal() + "\" -> \"" + Central.getLeft().getID() + "_" + Central.getLeft().getValue().GetVal() + "\";\n";
                content += NextNodos(Central.getLeft());
            }
            if (Central.getRight() != null)
            {
                content += "\"" + Central.getID() + "_" + Central.getValue().GetVal() + "\" -> \"" + Central.getRight().getID() + "_" + Central.getRight().getValue().GetVal() + "\";\n";
                content += NextNodos(Central.getRight());
            }

            return content;
        }
        AFN Thompson(Nodo Raiz)
        {
            AFN hijoIzq = null;
            AFN hijoDer = null;
            if (Raiz.getLeft() != null)
            {
                hijoIzq = Thompson(Raiz.getLeft());
                Console.WriteLine(hijoIzq.getPrimero().getID());
            }
            if (Raiz.getRight() != null)
            {
                hijoDer = Thompson(Raiz.getRight());
                Console.WriteLine(hijoDer.getPrimero().getID());
            }
            if (Raiz.getValue().GetTipo().Equals("Cadena"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Salto"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Tabulacion"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Comilla Simple"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Comilla Doble"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Epsilon"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            if (Raiz.getValue().GetTipo().Equals("Identificador"))
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(fin);
                inicio.setTransicionLeft(Raiz.getValue().GetVal());
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            //Creacion de Nodos AFN
            if (Raiz.getValue().GetTipo().Equals("Concatenacion"))//Concatenacion
            {
                hijoIzq.getUltimo().setLeft(hijoDer.getPrimero().getLeft());
                hijoIzq.getUltimo().setRight(hijoDer.getPrimero().getRight());
                hijoIzq.getUltimo().setTransicionLeft(hijoDer.getPrimero().getTransicionLeft());
                hijoIzq.getUltimo().setTransicionRight(hijoDer.getPrimero().getRransicionRight());
                AFN Union = new AFN(hijoIzq.getPrimero(), hijoDer.getUltimo());
                return Union;
            }
            else if (Raiz.getValue().GetTipo().Equals("Cerradura de Kleene"))//Asterisco
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(hijoIzq.getPrimero());
                inicio.setTransicionLeft("Epsilon");
                hijoIzq.getUltimo().setLeft(hijoIzq.getPrimero());
                hijoIzq.getUltimo().setTransicionLeft("Epsilon");
                hijoIzq.getUltimo().setRight(fin);
                hijoIzq.getUltimo().setTransicionRight("Epsilon");
                inicio.setRight(fin);
                inicio.setTransicionRight("Epsilon");
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            else if (Raiz.getValue().GetTipo().Equals("Disyunción"))//Disyunción
            {
                NodoAFN inicio = new NodoAFN(nodosAFN);
                nodosAFN++;
                NodoAFN fin = new NodoAFN(nodosAFN);
                nodosAFN++;
                inicio.setLeft(hijoIzq.getPrimero());
                inicio.setTransicionLeft("Epsilon");
                inicio.setRight(hijoDer.getPrimero());
                inicio.setTransicionRight("Epsilon");
                hijoIzq.getUltimo().setLeft(fin);
                hijoIzq.getUltimo().setTransicionLeft("Epsilon");
                hijoDer.getUltimo().setRight(fin);
                hijoDer.getUltimo().setTransicionRight("Epsilon");
                AFN Union = new AFN(inicio, fin);
                return Union;
            }
            return null;
        }
        String GenGraphAFN(NodoAFN raiz, LinkedList<int> Recorridos)
        {
            String content = "";
            if (!Recorridos.Contains(raiz.getID()))
            {
                Recorridos.AddLast(raiz.getID());
                content += "\"" + raiz.getID() + "\" [label=\""+raiz.getID()+"\"]";

                if (raiz.getLeft() != null)
                {
                    content += "\"" + raiz.getID() + "\" -> \"" + raiz.getLeft().getID() + "\"[label=\"" + raiz.getTransicionLeft() + "\"];\n";
                    content += GenGraphAFN(raiz.getLeft(), Recorridos);
                }
                if (raiz.getRight() != null)
                {
                    content += "\"" + raiz.getID() + "\" -> \"" + raiz.getRight().getID() + "\"[label=\"" + raiz.getRransicionRight() + "\"];\n";
                    content += GenGraphAFN(raiz.getRight(), Recorridos);
                }
            }
            return content;
        }
        LinkedList<NodoAFN> Mover(NodoAFN estado, string terminal)
        {
            LinkedList<NodoAFN> llegadas = new LinkedList<NodoAFN>();
            IrA(estado, terminal, llegadas);
            return llegadas;
        }
        void IrA(NodoAFN estado, String Transicion, LinkedList<NodoAFN> conjunto)
        {
            if (estado.getTransicionLeft().Equals(Transicion))
            {                
                if (!conjunto.Contains(estado.getLeft()))
                {
                    conjunto.AddLast(estado.getLeft());
                    if (Transicion.Equals("Epsilon") && (estado.getLeft().getID()!=(nodosAFN-1))) //Verifica que solo siga el camino de epsilon y sino devuelve solo la primera coincidencia de los terminales
                    {
                    IrA(estado.getLeft(), Transicion, conjunto);
                    }
                }
            }
            if (estado.getRransicionRight().Equals(Transicion))
            {                
                if (!conjunto.Contains(estado.getRight()))
                {
                    conjunto.AddLast(estado.getRight());
                    if (Transicion.Equals("Epsilon") && (estado.getRight().getID() != (nodosAFN - 1))) 
                    {
                        IrA(estado.getRight(), Transicion, conjunto);
                    }
                }
            }
        }
        void AFN_Nodes(NodoAFN raiz, LinkedList<NodoAFN> Recorridos)
        {
            if (!Recorridos.Contains(raiz))
            {
                Recorridos.AddLast(raiz);
                if (raiz.getLeft() != null)
                {
                    AFN_Nodes(raiz.getLeft(), Recorridos);
                }
                if (raiz.getRight() != null)
                {
                    AFN_Nodes(raiz.getRight(), Recorridos);
                }
            }
        }
        void Cerradura(LinkedList<NodoAFN> conjunto)
        {
            if (estados < 1)
            {
                LinkedList<NodoAFN> CerraduraResult = Mover(conjunto.ElementAt(0), "Epsilon");
                CerraduraResult.AddFirst(conjunto.ElementAt(0));
                States.AddLast(CerraduraResult);
                estados++;
            }
            else
            {
                for(int i =0; i<Terminales.Count; i++)
                {
                    LinkedList<NodoAFN> Transiciones = new LinkedList<NodoAFN>();
                    for(int e = 0; e < conjunto.Count; e++)
                    {
                        LinkedList<NodoAFN> Llegadas = Mover(conjunto.ElementAt(e), Terminales.ElementAt(i));
                        for(int o = 0; o < Llegadas.Count; o++) 
                        {
                            Transiciones.AddLast(Llegadas.ElementAt(o));
                        }                    
                    }
                    if (Transiciones.Count > 0)
                    {
                        for(int u =0; u < Transiciones.Count; u++)
                        {
                            LinkedList<NodoAFN> CerraduraResult = Mover(Transiciones.ElementAt(u), "Epsilon");
                            CerraduraResult.AddFirst(Transiciones.ElementAt(u));
                            int firstConjunto = ConjuntoID(conjunto);
                            if (!ExisteConjunto(CerraduraResult))
                            {
                                States.AddLast(CerraduraResult);
                                estados++;
                                for(int w =0; w < CerraduraResult.Count; w++)
                                {
                                    if (CerraduraResult.ElementAt(w).getID() == (nodosAFN-1))
                                    {
                                        Aceptacion.AddLast(estados);
                                    }
                                }
                                TransicionesAFD nonlisted = new TransicionesAFD(firstConjunto, Terminales.ElementAt(i), estados);
                                if (!ExistingTransicion(nonlisted))
                                {
                                TablaDeTransiciones.AddLast(nonlisted);  
                                }                              
                            }
                            else
                            {
                                TransicionesAFD nonlisted = new TransicionesAFD(firstConjunto, Terminales.ElementAt(i), ConjuntoID(CerraduraResult));
                                if (!ExistingTransicion(nonlisted))
                                {
                                    TablaDeTransiciones.AddLast(nonlisted);
                                }
                            }
                        }
                    }
                }
            }
        }
        Boolean ExisteConjunto(LinkedList<NodoAFN> conjunto)
        {
            for(int i =0; i < States.Count; i++)
            {
                if (SameList(States.ElementAt(i) , conjunto))
                {
                    return true;
                }
            }
            return false;
        }
        int ConjuntoID(LinkedList<NodoAFN> conjunto)
        {
            for (int i = 0; i < States.Count; i++)
            {
                if (SameList(States.ElementAt(i), conjunto))
                {
                    return i+1;
                }
            }
            return 0;
        }
        Boolean SameList(LinkedList<NodoAFN> First, LinkedList<NodoAFN> Second)
        {
            if (First.Count != Second.Count)
            {
                return false;
            }
            else
            {
                for(int i =0; i < First.Count; i++)
                {
                    if (!First.Contains(Second.ElementAt(i)))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        void GrapfAFD(LinkedList<TransicionesAFD> lista, string nombre)
        {
            
            string entrada = "digraph G {\n rankdir=LR nodesep=0.3;\n ranksep=0.2;\n    margin=0.1;\n   node [shape=circle];\n  edge [arrowsize=0.8];";
            string contenido="node [shape = doublecircle];";
            for (int i =0; i < Aceptacion.Count; i++)
            {
                contenido += " "+Aceptacion.ElementAt(i)+" ";
            }
            contenido += ";";
            contenido += "node[shape = circle]; ";
            foreach(TransicionesAFD transicion in lista)
            {
                contenido += "\""+transicion.Conjunto+"\" -> \""+transicion.Llegada+"\" [label=\""+transicion.Transicion+"\"];\n";
            }
            entrada += contenido + "}";
            System.IO.File.WriteAllText(@"D:\\" + nombre + "_AFD.txt", entrada);
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Descargas\graphviz-2.38\release\bin\dot.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Programas y +\graphviz-2.38\release\bin\dot.exe");
            startInfo.Arguments = "-Tpng \"D:\\" + nombre + "_AFD.txt\" -o \"D:\\" + nombre + "_AFD.png\"";
            Process.Start(startInfo);
        }
        void printTransTable(LinkedList<TransicionesAFD> lista, string nombre)
        {
            try
            {
                int i = 0;
                StreamWriter File = new StreamWriter("D:\\"+nombre+".html");
                File.Write("<html>\n");
                File.Write("<head>\n");
                File.Write("<meta charset = \"utf -8\">\n");
                File.Write("<title> Tabla de Tokens</title>\n");
                File.Write("</head>\n");
                File.Write("<body bgcolor=#000>\n");
                File.Write("<table align = \"center\" style = \"width:80% ; font-family: consolas; \">\n");
                File.Write("<tr bgcolor = \"F5C82E\" ><td colspan = 4 align = \"center\" ><h3> Tabla de Tokens</h3></td></tr>\n");
                File.Write("<tr bgcolor = \"FBDD76\" style = \"width: 10%; text-align:center; \"><td style = \"width: 10%; text-align:center; \">#</td><td>De</td><td style = \"width: 30% \">Con</td><td style =\"width: 30% \">Llega a</td></tr>\n");


                Console.WriteLine("-------------------------Tabla de Tokens--------------------------");
                foreach (TransicionesAFD item in lista)
                {
                    i++;
                    Console.WriteLine(i + " " + item.Conjunto + " <--> " + item.Transicion + " <--> " + item.Llegada);
                    File.Write("<tr bgcolor = \"FBF3D6\"><td style = \"text-align:center; \">" + i + "</td><td style = \"text-align:center; \">" + item.Conjunto + "</td><td>" + item.Transicion + "</td><td>" + item.Llegada + "</td></tr>\n");
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
        }
        Boolean ExistingTransicion(TransicionesAFD nonlisted)
        {
            for(int i =0; i < TablaDeTransiciones.Count; i++)
            {
                TransicionesAFD listed = TablaDeTransiciones.ElementAt(i);
                if (listed.Conjunto.Equals(nonlisted.Conjunto) && listed.Transicion.Equals(nonlisted.Transicion) && listed.Llegada.Equals(nonlisted.Llegada))
                {
                    return true;
                }
            }
            return false;
        }
        void CreateTable(LinkedList<TransicionesAFD> Tabla, LinkedList<string> TerminalesList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Estado");
            foreach(string terminal in TerminalesList)
            {
                dt.Columns.Add(terminal);
            }
            LinkedList<int> estados = new LinkedList<int>();
            foreach(TransicionesAFD transicion in Tabla)
            {
                if (!estados.Contains(transicion.Conjunto))
                {
                    estados.AddLast(transicion.Conjunto);
                }
                if (!estados.Contains(transicion.Llegada))
                {
                    estados.AddLast(transicion.Llegada);
                }
            }
            int w = TerminalesList.Count+1;
            string[] transiciones = new string[w];
            for(int i =0; i < estados.Count; i++)
            {
                transiciones[0] = estados.ElementAt(i).ToString();
                for(int o=0; o < TerminalesList.Count; o++)
                {
                    for (int e =0; e < Tabla.Count; e++)
                    {
                        if(Tabla.ElementAt(e).Conjunto==estados.ElementAt(i) && Tabla.ElementAt(e).Transicion.Equals(TerminalesList.ElementAt(o))){
                            transiciones[o+1] = Tabla.ElementAt(e).Llegada.ToString();
                        }
                    }                    
                }
                dt.Rows.Add(transiciones);
                transiciones = new string[w];
            }
            dgV1.DataSource = dt;
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                foreach(AnalizadorGenerico analizador in Tablas){
                    if (analizador.ExprID1.Equals(treeView1.SelectedNode.Text)){
                        CreateTable(analizador.TablaTransiciones1, analizador.Termianles1);
                        try
                        {
                            pictureBox1.Image = System.Drawing.Image.FromFile(@"D:\\" + analizador.ExprID1 + "_AFD.png");
                            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            pictureBox2.Image = System.Drawing.Image.FromFile(@"D:\\" + analizador.ExprID1 + "_AFN.png");
                            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }

        }
        void EvaluarLexema(string ExpID, LinkedList<TransicionesAFD> Transiciones, LinkedList<TerminalesTH> TerminalList, string cadena, LinkedList<int> Aceptacion, int fila)
        {
            int iterador = 0;
            //cadena += "#";
            Char c;
            int estado = 1;
            int token_number = 1;
            Boolean transitionmade = false;
            while (iterador < cadena.Length)
            {
                int columna = iterador + 1;
                c = cadena.ElementAt(iterador);
                transitionmade = false;
                foreach (TransicionesAFD transicionesAFD in Transiciones)
                {
                    if (!transitionmade)
                    {
                        if (transicionesAFD.Conjunto == estado)
                        {
                            switch (TransicionType(TerminalList, transicionesAFD.Transicion))
                            {
                                case TerminalesTH.Tipo.cadena:
                                    if (CorrectStr(cadena, iterador, transicionesAFD.Transicion))
                                    {
                                        //registra Token
                                        Console.WriteLine("TOKEN Nombre: Cadena Valor: " + transicionesAFD.Transicion + " Columna: " + columna + " Fila: " + fila);
                                        TablaTokens.AddCell(token_number.ToString());
                                        TablaTokens.AddCell("Cadena");
                                        TablaTokens.AddCell(transicionesAFD.Transicion);
                                        TablaTokens.AddCell(columna.ToString());
                                        TablaTokens.AddCell(fila.ToString());
                                        iterador += transicionesAFD.Transicion.Length;
                                        estado = transicionesAFD.Llegada;
                                        transitionmade = true;
                                        token_number++;
                                    }
                                    break;
                                case TerminalesTH.Tipo.conjunto:
                                    if (Char.IsDigit(c))
                                    {
                                        if (AllowedInteger(transicionesAFD.Transicion, int.Parse(c.ToString())))
                                        {
                                            //registra Token
                                            Console.WriteLine("TOKEN Nombre: " + transicionesAFD.Transicion +" Valor: "+c+" Columna: "+columna+" Fila: "+fila);
                                            TablaTokens.AddCell(token_number.ToString());
                                            TablaTokens.AddCell(transicionesAFD.Transicion);
                                            TablaTokens.AddCell(c.ToString());
                                            TablaTokens.AddCell(columna.ToString());
                                            TablaTokens.AddCell(fila.ToString());
                                            iterador++;
                                            estado = transicionesAFD.Llegada;
                                            transitionmade = true;
                                            token_number++;
                                        }                                        
                                    }else if (CorrectChar(transicionesAFD.Transicion, c))
                                    {
                                        //registra Token
                                        Console.WriteLine("TOKEN Nombre: " + transicionesAFD.Transicion + " Valor: " + c + " Columna: " + columna + " Fila: " + fila);
                                        iterador++;
                                        TablaTokens.AddCell(token_number.ToString());
                                        TablaTokens.AddCell(transicionesAFD.Transicion);
                                        TablaTokens.AddCell(c.ToString());
                                        TablaTokens.AddCell(columna.ToString());
                                        TablaTokens.AddCell(fila.ToString());
                                        estado = transicionesAFD.Llegada;
                                        transitionmade = true;
                                        token_number++;
                                    }else if(CorrectSpcConj(transicionesAFD.Transicion, c)){
                                        //registra Token
                                        Console.WriteLine("TOKEN Nombre: " + transicionesAFD.Transicion + " Valor: " + c + " Columna: " + columna + " Fila: " + fila);
                                        iterador++;
                                        TablaTokens.AddCell(token_number.ToString());
                                        TablaTokens.AddCell(transicionesAFD.Transicion);
                                        TablaTokens.AddCell(c.ToString());
                                        TablaTokens.AddCell(columna.ToString());
                                        TablaTokens.AddCell(fila.ToString());
                                        estado = transicionesAFD.Llegada;
                                        transitionmade = true;
                                        token_number++;
                                    }
                                    break;
                                case TerminalesTH.Tipo.especial:
                                        if(CorrectSpcChar(transicionesAFD.Transicion, c))
                                        {
                                            Console.WriteLine("TOKEN Nombre: " + transicionesAFD.Transicion + " Valor: " + c + " Columna: " + columna + " Fila: " + fila);
                                            iterador++;
                                            TablaTokens.AddCell(token_number.ToString());
                                            TablaTokens.AddCell(transicionesAFD.Transicion);
                                            TablaTokens.AddCell(c.ToString());
                                            TablaTokens.AddCell(columna.ToString());
                                            TablaTokens.AddCell(fila.ToString());
                                            estado = transicionesAFD.Llegada;
                                            transitionmade = true;
                                            token_number++;
                                        }                                        
                                    break;
                                //not sure about the default clause
                                default:
                                    iterador = cadena.Length; //fin al ciclo
                                    Console.WriteLine("ERROR Nombre: " + transicionesAFD.Transicion + " Valor: " + c + " Columna: " + columna + " Fila: " + fila);
                                    TablaErrores.AddCell(c.ToString());
                                    TablaErrores.AddCell(columna.ToString());
                                    TablaErrores.AddCell(fila.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            //registra error
                        }
                    }
                }                
                if (!transitionmade)
                {
                    Console.WriteLine("ERROR " + CuttedString(iterador, cadena) +" Columna: " + columna + " Fila: " + fila);
                    TablaErrores.AddCell(CuttedString(iterador, cadena));
                    TablaErrores.AddCell(columna.ToString());
                    TablaErrores.AddCell(fila.ToString());
                    iterador = cadena.Length+1;
                }                
            }
            if (Aceptacion.Contains(estado) && iterador==cadena.Length)
            {
                //Cadena Correcta
                ConsoleOutPut.Text += "\n"+ ExpID + " aceptó la expresion \"" + cadena + "\"";
            }
            else
            {
                //Cadena incorrecta
                ConsoleOutPut.Text += "\n" + ExpID + " no aceptó la expresion \"" + cadena + "\"";
            }
        }
        Boolean CorrectStr(string entrada, int iterador, string cadena)
        {
            for(int i =0; i < cadena.Length; i++)
            {
                if (!entrada.ElementAt(iterador).Equals(cadena.ElementAt(i)))
                {
                    return false;
                }
                iterador++;
            }
            return true;
        }
        Boolean CorrectChar(string conjuntoID, Char c)
        {
            foreach(Conjunto conjunto in FileConjuntos){
                if (conjunto.getID().Equals(conjuntoID))
                {
                    if (conjunto.DefConjunto1.Contains(c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        Boolean AllowedInteger(string conjuntoID, int c)
            {
                foreach (Conjunto conjunto in FileConjuntos)
                {
                    if (conjunto.getID().Equals(conjuntoID))
                    {
                        if (conjunto.DefIntConjunto1.Contains(c))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }        
        Boolean CorrectSpcChar(string transicion, char c)
        {
            if (transicion.Equals("\\t"))
            {
                if (c.CompareTo('\t')==0){
                    return true;
                }
            }else if (transicion.Equals("\\\""))
            {
                if (c.CompareTo('"')==0){
                    return true;
                }
            }else if (transicion.Equals("\\\'"))
            {
                if (c.CompareTo('\'') == 0)
                {
                    return true;
                }
            }
            else if (transicion.Equals("\\n"))
            {
                if (c.CompareTo('\n') == 0)
                {
                    return true;
                }
            }
            return false;
        }
        Boolean CorrectSpcConj(string conjuntoID, char c)
        {
            foreach (Conjunto conjunto in FileConjuntos)
            {
                if (conjunto.getID().Equals(conjuntoID))
                {
                    if (c.CompareTo('\n') == 0)
                    {
                        if (conjunto.SpcConjunto1.Contains("\\n")){
                            return true;
                        }
                    }else if (c.CompareTo('\t') == 0)
                    {
                        if (conjunto.SpcConjunto1.Contains("\\t")){
                            return true;
                        }
                    }
                    else if(c.CompareTo('"') == 0)
                    {
                        if (conjunto.SpcConjunto1.Contains("\\\"")){
                            return true;
                        }
                    }
                    else if(c.CompareTo('\'') == 0)
                    {
                        if (conjunto.SpcConjunto1.Contains("\\\'")){
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        TerminalesTH.Tipo TransicionType(LinkedList<TerminalesTH> TerminalList, string TerminalID)
        {
            //cambie el TermianlesList, variable global, por TerminalList que es variable de ambiente
            for(int i =0; i< TerminalList.Count; i++)
            {
                if (TerminalList.ElementAt(i).TerminalID1==TerminalID)
                {
                    return TerminalList.ElementAt(i).TipoTerminal1;
                }
            }
            return 0;
        }
        string CuttedString(int iterador, string entrada)
        {
            string cadena = "";
            for(int i = iterador; i< entrada.Length; i++)
            {
                cadena += entrada.ElementAt(i);
            }
            return cadena;
        }
        void DefineConjuntos(LinkedList<Conjunto> ListaConjuntos)
        {
            foreach(Conjunto conjunto in ListaConjuntos)
            {
                if (Char.IsDigit(conjunto.getConjunto().ElementAt(0))){
                    int i = 0;
                    while (Char.IsDigit(conjunto.getConjunto().ElementAt(i))){//Recorrer todo el primer número
                        i++;
                    }
                    if (conjunto.getConjunto().ElementAt(i).Equals('~')){
                        String[] EspaciadoSup = conjunto.getConjunto().Split('~');
                        for (int w = 0; (w + Int32.Parse(EspaciadoSup[0])) <= Int32.Parse(EspaciadoSup[1]); w++)
                        {
                            conjunto.DefIntConjunto1.AddLast(Int32.Parse(EspaciadoSup[0])+w);
                        }
                    }
                    else if (conjunto.getConjunto().ElementAt(i).Equals(','))
                    {
                        String[] EspaciadoSup = conjunto.getConjunto().Split(',');
                        for (int w = 0; w < EspaciadoSup.Length; w++)
                        {
                            conjunto.DefIntConjunto1.AddLast(Int32.Parse(EspaciadoSup[w]));
                        }
                    }
                }
                else if (conjunto.getConjunto().ElementAt(0).Equals('\\'))
                {
                    String[] EspaciadoSup = conjunto.getConjunto().Split(',');
                    for (int w = 0; w < EspaciadoSup.Length; w++)
                    {
                        if (EspaciadoSup[w].ElementAt(1).Equals('t'))
                        {
                            conjunto.SpcConjunto1.AddLast("\\t");
                        }else if (EspaciadoSup[w].ElementAt(1).Equals('n')){
                            conjunto.SpcConjunto1.AddLast("\\n");
                        }else if (EspaciadoSup[w].ElementAt(1).Equals('"')){
                            conjunto.SpcConjunto1.AddLast("\\\"");
                        }else if (EspaciadoSup[w].ElementAt(1).Equals('\'')){
                            conjunto.SpcConjunto1.AddLast("\\\'");
                        }
                    }                    
                }
                else
                {
                    if (conjunto.getConjunto().ElementAt(1).Equals('~'))//Rango
                    {
                        if (Char.IsLetterOrDigit(conjunto.getConjunto().ElementAt(0)))
                        {
                            for (int i = 0; i + conjunto.getConjunto().ElementAt(0) <= conjunto.getConjunto().ElementAt(2); i++)//No se si reconoce el límite dado orpor el char
                            {
                                    conjunto.DefConjunto1.AddLast((char)(i + conjunto.getConjunto().ElementAt(0)));
                            }
                        }
                        else
                        {
                            for (int i = 0; i + conjunto.getConjunto().ElementAt(0) <= conjunto.getConjunto().ElementAt(2); i++)//No se si reconoce el límite dado orpor el char
                            {
                                if ((i + conjunto.getConjunto().ElementAt(0)) == 65)
                                {
                                    i += 25;
                                }
                                else if ((i + conjunto.getConjunto().ElementAt(0)) == 97)
                                {
                                    i += 25;
                                }
                                else
                                {
                                    conjunto.DefConjunto1.AddLast((char)(i + conjunto.getConjunto().ElementAt(0)));
                                }
                            }
                        }                      
                    }
                    else if (conjunto.getConjunto().ElementAt(1).Equals(','))//Definido
                    {
                        String[] EspaciadoSup = conjunto.getConjunto().Split(',');
                        for (int w = 0; w < EspaciadoSup.Length; w++)
                        {
                            conjunto.DefConjunto1.AddLast(EspaciadoSup[w].ElementAt(0));
                        }
                    }
                }
                //Los Rangos cambian si es Caracter Especial
                
            }
        }

    }
}
