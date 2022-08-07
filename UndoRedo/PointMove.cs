using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;
namespace UndoRedo
{
    public class PointMove : Operation
    {
        int number;
        int bias_x;
        int bias_y;
        public PointMove() { }

        public PointMove(int number, int bias_x, int bias_y) : base() //Перелаётся разница конечной и начальной координат
        {
            this.number = number;
            this.bias_x = bias_x;
            this.bias_y = bias_y;
        }
        public override void undo(List<Shape> list)
        {
            list[number].X -= bias_x;
            list[number].Y -= bias_y;
        }
        public override void redo(List<Shape> list) //TODO
        {
            list[number].X += bias_x;
            list[number].Y += bias_y;
        }
    }
}
