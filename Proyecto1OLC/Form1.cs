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
            LinkedList<Nodo> Ramas = new LinkedList<Nodo>();
            foreach (Token objeto in lista)
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
                    objeto.setValor(objeto.GetVal().Substring(1, objeto.GetVal().Length-2 ));
                    Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                }
                else if ("Identificador".Equals(objeto.GetTipo()))
                {
                    Ramas.AddLast(new Nodo(Nodo.Tipo.Terminal, objeto, nodos));
                }
                nodos++;
            }
            return Ramas;
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
            if(raiz.getValue().GetTipo().Equals("Signo de Interrogacion")){
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
                raiz.setValue(new Token(Token.Tipo.Absoluto, ".", 0, 0));
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
            if(raiz.getRight() != null)
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

        Boolean IsLeftSon(Nodo raiz, Nodo hijo){
            if(raiz.getLeft() == hijo)
            {
                return true;
            }
            return false;
        }

        private void AnalizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnalizadorLexico analizador = new AnalizadorLexico();
            ListasAnalisis resultado = analizador.escanear(GetRichTextBox().Text);
            //GraphConjuntos(resultado.getConjuntos());
            analizador.imprimiListaToken(resultado.getSalida());
            //System.out.println("Expreisones Regulares");
            //for (int i = 0; i < resultado.getExpresionesRegulares().size(); i++)
            foreach(ExpresionRegular Expr in resultado.getExpresionesRegulares())
            {
                ListasAnalisis expresiones = analizador.escanear(Expr.getExpresion());
                analizador.imprimiListaToken(expresiones.getSalida());
                LinkedList<Nodo> temporal = SetType(expresiones.getSalida());
                //System.out.println(temporal.size());
                temporal.AddFirst(new Nodo(Nodo.Tipo.Operador_Binario, new Token(Token.Tipo.Punto, ".", 0, 0), 0));
                temporal.AddLast(new Nodo(Nodo.Tipo.Terminal, new Token(Token.Tipo.aceptacion, "#", 0, 0), 1));
                Nodo cabeza = GenerarArbol(temporal);
                Console.WriteLine(cabeza.getValue().GetVal());
                Graficar(cabeza, Expr.getExpID());
                ThompsonTree(cabeza);
                Graficar(cabeza, Expr.getExpID() + "_Th");
                AFN automata = Thompson(cabeza.getLeft());
                LinkedList<int> Recorridos = new LinkedList<int>();
                GraphAFN(GenGraphAFN(automata.getPrimero(), Recorridos), Expr.getExpID()+"_AFN");
                /*PreOrderAnulables(cabeza);
                PreOrderPrimeros(cabeza);
                PreOrderUltimos(cabeza);
                PreOrderSiguientes(cabeza);
                LinkedList<Nodo> Hojas = new LinkedList<>();
                GetHojas(cabeza, Hojas);
                System.out.println(cabeza.getID());
                GraphSiguientes(Hojas, resultado.getExpresionesRegulares().get(i).getExpID());
                */
            }
        }

        void Graficar(Nodo raiz, string nombre)
        {
            string entrada = "digraph G {\n nodesep=0.3;\n ranksep=0.2;\n    margin=0.1;\n   node [shape=circle];\n  edge [arrowsize=0.8];";
            entrada += NextNodos(raiz) +"}";
            System.IO.File.WriteAllText(@"D:\\" + nombre + ".txt", entrada);
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Descargas\graphviz-2.38\release\bin\dot.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Programas y +\graphviz-2.38\release\bin\dot.exe");
            startInfo.Arguments = "-Tpng \"D:\\" + nombre + ".txt\" -o \"D:\\" + nombre + ".png\"";
            Process.Start(startInfo);
        }
        void GraphAFN(string contenido, string nombre)
        {
            string entrada = "digraph G {\n nodesep=0.3;\n ranksep=0.2;\n    margin=0.1;\n   node [shape=circle];\n  edge [arrowsize=0.8];";
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
            if(Raiz.getLeft() != null)
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
                /*
                NodoAFN inicio = hijoIzq.getPrimero();
                nodosAFN++;
                NodoAFN join = hijoDer.getPrimero();
                nodosAFN++;
                NodoAFN fin = hijoDer.getUltimo();
                nodosAFN++;
                inicio.setLeft(join);
                */
                hijoIzq.getUltimo().setLeft(hijoDer.getPrimero());
                AFN Union = new AFN(hijoIzq.getPrimero(), hijoDer.getUltimo());
                //GraphAFN(GenGraphAFN(hijoIzq.getPrimero()), "AFN"+hijoIzq.getPrimero().getID());
                return Union;
            }
            else if(Raiz.getValue().GetTipo().Equals("Cerradura de Kleene"))//Asterisco
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
                //GraphAFN(GenGraphAFN(inicio), "AFN" + hijoIzq.getPrimero().getID());
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
                //GraphAFN(GenGraphAFN(inicio), "AFN" + hijoIzq.getPrimero().getID());
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
                content += "\"" + raiz.getID() +"\" [label=\"\"]";

                if (raiz.getLeft() != null)
                {
                    content +="\""+raiz.getID()+"\" -> \""+ raiz.getLeft().getID()+"\"[label=\"" + raiz.getTransicionLeft() + "\"];\n";
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
    }
}
