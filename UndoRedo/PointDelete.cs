using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace UndoRedo
{
    public class PointDelete : Operation
    {
        int number;
        int x;
        int y;
        Type type;

        public PointDelete() { }
        public PointDelete(int number, int x, int y, Type type) : base() //1 -- circle, 2 -- square, 3 -- triangle
        {
            this.number = number;
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public override void undo(List<Shape> list)
        {
            if (type.Name == "Circle")
            {
                list.Insert(number, new Circle(x, y));
            }
            else
            {
                if (type.Name == "Square")
                {
                    list.Insert(number, new Square(x, y));
                }
                else
                {
                    list.Insert(number, new Triangle(x, y));
                }
            }
        }

        public override void redo(List<Shape> list)
        {
            list.RemoveAt(number);
        }
    }
}
