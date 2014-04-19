using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Bounds
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Point New(int x, int y)
        {
            Point result = new Point();
            result.X = x;
            result.Y = y;
            return result;
        }
    }
}
