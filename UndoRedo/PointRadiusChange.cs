using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace UndoRedo
{
    public class PointRadiusChange : Operation
    {
        uint radiusChanged;

        public PointRadiusChange() { }
        public PointRadiusChange(uint radiusChange)
        {
            this.radiusChanged = radiusChange;
        }

        public override void undo(List<Shape> list)
        {
            Shape.Radius -= radiusChanged;
        }
        public override void redo(List<Shape> list)
        {
            Shape.Radius += radiusChanged;
        }
    }
}
