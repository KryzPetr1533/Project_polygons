using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Shape
{
    public class RadiusEventArgs : EventArgs
    {
        public uint NewRadius;
        public RadiusEventArgs(uint NewRadius)
        {
            this.NewRadius = NewRadius;
        }

    }
}
