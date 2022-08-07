using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using All_Shapes;

namespace Shell_Build
{
    public class Build
    {
        public static void build(List<Shape> l, Graphics e)
        {
            if (l.Count > 2)     // Построение оболочки
            {
                int higher = 0, lower = 0;
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].Drawline = false;
                }
                for (int i = 0; i < l.Count; i++)
                {
                    for (int j = i + 1; j < l.Count; j++)
                    {
                        higher = lower = 0;
                        for (int k = 0; k < l.Count; k++)
                        {
                            if (k == i || k == j) continue;
                            int a = l[i].Y - l[j].Y, b = l[j].X - l[i].X;
                            int c = (-a) * (l[i].X) - (b * l[i].Y);


                            if (0 >= a * l[k].X + b * l[k].Y + c)
                            {
                                lower++;
                            }
                            else
                            {
                                higher++;
                            }
                        }
                        if (higher == l.Count - 2 || lower == l.Count - 2)
                        {
                            Pen pen = new Pen(Color.Red);
                            e.DrawLine(pen, l[i].X, l[i].Y, l[j].X, l[j].Y);
                            l[i].Drawline = true;
                            l[j].Drawline = true;
                        }
                    }
                }
            }
        }
    }
}
