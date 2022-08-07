using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using All_Shapes;

namespace UndoRedo
{
    public class PointColorChange : Operation
    {
        Color colorChanged;
        Color recolorChanged;

        public PointColorChange() { }

        public PointColorChange(Color colorChanged)
        {
            this.colorChanged = colorChanged;
        }

        public override void undo(List<Shape> list)
        {
            recolorChanged = Shape.PointColor;
            Shape.PointColor = colorChanged;

        }

        public override void redo(List<Shape> list)
        {
            colorChanged = Shape.PointColor;
            Shape.PointColor = recolorChanged;

        }
    }
}
