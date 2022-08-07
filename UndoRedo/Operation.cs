using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using All_Shapes;

namespace UndoRedo
{
    public abstract class Operation
    {
        public abstract void undo(List<Shape> list);
        public abstract void redo(List<Shape> list);
    }
}
