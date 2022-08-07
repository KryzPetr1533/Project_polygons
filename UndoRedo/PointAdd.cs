using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace UndoRedo
{
    public class PointAdd : Operation
    {
        int number;
        int x;
        int y;
        Type type;

        public PointAdd() { }
        public PointAdd(int number, int x, int y, Type type) : base()
        {
            this.number = number;
            this.x = x;
            this.y = y;
            this.type = type;
        }
        public override void undo(List<Shape> list)
        {
            list.RemoveAt(number);
        }
        public override void redo(List<Shape> list)
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
    }
}
