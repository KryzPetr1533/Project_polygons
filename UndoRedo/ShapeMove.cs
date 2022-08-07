using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace UndoRedo
{
    public class ShapeMove : Operation
    {
        int bias_x;
        int bias_y;

        public ShapeMove() : base() { }

        public ShapeMove(int bias_x, int bias_y)
        {
            this.bias_x = bias_x;
            this.bias_y = bias_y;
        }

        public override void undo(List<Shape> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].X -= bias_x;
                list[i].Y -= bias_y;
            }
        }
        public override void redo(List<Shape> list) //TODO
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].X += bias_x;
                list[i].Y += bias_y;
            }
        }

        public void push(int bias_x, int bias_y)
        {
            this.bias_x = bias_x;
            this.bias_y = bias_y;
        }
    }
}
