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
using System.Runtime.Serialization.Formatters.Binary;
using All_Shapes;
using UndoRedo;
using Shell_Build;
using Run;

namespace Project_Shape
{
    public delegate void RadiusChanged(object sender, RadiusEventArgs e);
    public partial class Form1 : Form
    {
        List<Shape> l;      // хранит все точки
        bool flag; // вспомогательный флаг для d&d
        bool shapeMoving; // флаг для undo&redo
        bool brun; //флаг для таймера
        Random random; //для таймера
        BinaryFormatter formatter;
        Stack<Operation> undo;
        Stack<Operation> redo;
        int bias_x;
        int bias_y;

        public Form1()
        {
            InitializeComponent();

            l = new List<Shape>(); //создаёт лист
            flag = false;
            shapeMoving = false;
            brun = false;
            random = new Random();
            DoubleBuffered = true; //чтобы не моргало
            formatter = new BinaryFormatter();
            undo = new Stack<Operation>();
            redo = new Stack<Operation>();
            undo.Push(null);
            redo.Push(null);
        }

        //private void Shell(List <Shape> l)
        //{
        //    if (l.Count > 2)
        //    {
        //        int higher = 0, lower = 0;
        //        int y; // Вспомогательный y для подстановки в формулу прямой

        //        for (int i = 0; i < l.Count; i++)
        //        {
        //            for (int j = i + 1; j < l.Count; j++)
        //            {
        //                for (int k = j + 1; k < l.Count; k++)
        //                {
        //                    y = (l[i].Y - l[j].Y) * (l[k].X - l[j].X) / (l[i].X - l[k].X) + l[j].Y; // Формула расчёта y при заданом x, взятая из курса геометрии 9 класса) Самая первая...
        //                    if (y > l[k].Y)
        //                    {
        //                        lower++;
        //                    }
        //                    else
        //                    {
        //                        higher++;
        //                    }
        //                }
        //                if (higher == l.Count - 2 || lower == l.Count - 2)
        //                {

        //                }
        //            }
        //        }
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "binary files(*.bin)|*.bin";
            openFileDialog1.Filter = "binary files(*.bin)|*.bin";
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            foreach (Shape i in l) { i.draw(e.Graphics); }

            Build.build(l, e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                bias_x = e.X;
                bias_y = e.Y;
                for (int i = 0; i < l.Count; i++)// Определяет, надо ли двигать вершины
                {
                    if (l[i].IsInside(e.X, e.Y))
                    {
                        flag = true;             //Служит сообщением для добавления вершины 
                        l[i].X_mouse = e.X;
                        l[i].Y_mouse = e.Y;
                        l[i].Moving = true;
                        
                    }
                }
                
                if (!flag) // Добавляет вершину
                {
                    if (кругToolStripMenuItem.Checked)
                    {
                        Circle c = new Circle(e.X, e.Y);
                        l.Add(c);
                        undo.Push(new PointAdd(l.Count - 1, c.X, c.Y, c.GetType()));
                    }
                    else
                    {
                        if (квадратToolStripMenuItem.Checked)
                        {
                            Square c = new Square(e.X, e.Y);
                            l.Add(c);
                            undo.Push(new PointAdd(l.Count - 1, c.X, c.Y, c.GetType()));
                        }
                        else
                        {
                            if (треугольникToolStripMenuItem.Checked)
                            {
                                Triangle c = new Triangle(e.X, e.Y);
                                l.Add(c);
                                undo.Push(new PointAdd(l.Count - 1, c.X, c.Y, c.GetType()));
                            }
                        }
                        Refresh();
                    }
                    if (l.Count >= 3)
                    {
                        Refresh();
                        if (!l[l.Count - 1].Drawline)
                        {
                            undo.Pop();
                            undo.Pop();
                            l.RemoveAt(l.Count - 1);
                            flag = true;
                            shapeMoving = true;
                            for (int j = 0; j < l.Count; j++)
                            {
                                l[j].Moving = true;
                                l[j].X_mouse = e.X;
                                l[j].Y_mouse = e.Y;
                            }
                        }
                        for (int i = 0; i < l.Count; i++)
                        {
                            if (!l[i].Drawline)
                            {
                                undo.Push(new PointDelete(i, l[i].X, l[i].Y, l[i].GetType()));
                                l.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    undo.Push(null);
                    redo.Clear();
                    redo.Push(null);
                }
            }
            if(e.Button == MouseButtons.Right)
            {
                for (int i = l.Count - 1; i >= 0; i--)// Удаляет вершину, если на пересечении, то верхнюю
                {
                    if (l[i].IsInside(e.X, e.Y))
                    {
                        undo.Push(new PointDelete(i, l[i].X, l[i].Y, l[i].GetType()));
                        undo.Push(null);
                        l.RemoveAt(i);
                        break;
                    }
                }
            }
            Refresh();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    if (l[i].Moving)
                    {
                        l[i].X = l[i].X + e.X - l[i].X_mouse;
                        l[i].Y = l[i].Y + e.Y - l[i].Y_mouse;
                        
                        l[i].X_mouse = e.X;
                        l[i].Y_mouse = e.Y;
                        
                    }
                }
                Refresh();
            }
            
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                flag = false;
                if (shapeMoving)
                {
                    undo.Push(new ShapeMove(e.X - bias_x, e.Y - bias_y));
                    for (int i = 0; i < l.Count; i++)
                    {
                        l[i].Moving = false;
                    }
                    shapeMoving = false;
                }
                else
                {
                    for (int i = 0; i < l.Count; i++)
                    {
                        if (l[i].Moving)
                        {
                            undo.Push(new PointMove(i, e.X - bias_x, e.Y - bias_y));
                        }
                    }
                }
                

                if (l.Count >= 3) //Удаление вершины, которая внутри оболочки
                {
                    Refresh();
                    for (int i = 0; i < l.Count; i++)
                    {
                        if (!l[i].Drawline)
                        { 
                            undo.Push(new PointDelete(i, l[i].X, l[i].Y, l[i].GetType()));
                            l.RemoveAt(i);
                            i--;
                        }
                    }
                    Refresh();
                }
                undo.Push(null);
                redo.Clear();
                redo.Push(null);
            }
           
        }

        private void кругToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.CheckState = CheckState.Checked;
            кругToolStripMenuItem.Checked = true;
            квадратToolStripMenuItem.Checked = false;
            треугольникToolStripMenuItem.Checked = false;
        }

        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.CheckState = CheckState.Checked;
            кругToolStripMenuItem.Checked = false;
            квадратToolStripMenuItem.Checked = true;
            треугольникToolStripMenuItem.Checked = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.CheckState = CheckState.Checked;
            кругToolStripMenuItem.Checked = false;
            квадратToolStripMenuItem.Checked = false;
            треугольникToolStripMenuItem.Checked = true;
        }

        
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(brun)
            {
                Brun.run(l, random);
                Refresh();
                if (l.Count >= 3)
                {
                    Refresh();
                    for (int i = 0; i < l.Count; i++)
                    {
                        if (!l[i].Drawline)
                        {
                            l.RemoveAt(i);
                            i--;
                        }
                    }
                    Refresh();
                }
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            brun = true;
            timer1.Start();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            brun = false;
            timer1.Stop();
        }

        private void RadiusChange(object sender, RadiusEventArgs e) //Меняет радиус вершины
        {
            undo.Push(new PointRadiusChange(e.NewRadius - Shape.Radius));
            undo.Push(null);
            redo.Clear();
            redo.Push(null);
            Shape.Radius = e.NewRadius;
            Refresh();
        }

        private void PointColorChanger(Color color) //Меняет цвет вершины
        {
            undo.Push(new PointColorChange(Shape.PointColor));
            undo.Push(null);
            redo.Clear();
            redo.Push(null);
            Shape.PointColor = color;
            Refresh();
        }

        private void изменениеРадиусаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRadius FR = new FormRadius();
            FR.Show();
            FR.RC += new RadiusChanged(RadiusChange);
        }

        private void изменениеЦветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            PointColorChanger(colorDialog1.Color);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
            {
                formatter.Serialize(stream, l);
                stream.Close();
            }
            
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            saveFileDialog1.FileName = openFileDialog1.FileName;
            using (Stream stream = new FileStream(openFileDialog1.FileName, FileMode.Open))
            {
                l = (List<Shape>)formatter.Deserialize(stream);
                Refresh();
                stream.Close();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName == "")
            {
                openFileDialog1.ShowDialog();
                using (Stream stream = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    l = (List<Shape>)formatter.Deserialize(stream);
                    Refresh();
                    stream.Close();
                }
            }
            else
            {
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    formatter.Serialize(stream, l);
                    stream.Close();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (undo.Count > 1)
            {
                undo.Pop();
                while (undo.Peek() != null)
                {
                    undo.Peek().undo(l);
                    redo.Push(undo.Pop());
                }
                redo.Push(null);
                Refresh();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if(redo.Count > 1)
            {
                redo.Pop();
                while (redo.Peek() != null)
                {
                    redo.Peek().redo(l);
                    undo.Push(redo.Pop());
                }
                undo.Push(null);
                Refresh();
            }
        }
    }
}
