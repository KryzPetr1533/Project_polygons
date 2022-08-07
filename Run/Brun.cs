using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace Run
{
    public class Brun
    {
        public static void run(List<Shape> l, Random random)
        {
            for (int i = 0; i < l.Count; i++)
            {

                l[i].X += random.Next(-1, 2);
                l[i].Y += random.Next(-1, 2);
            }
        }
    }
}
