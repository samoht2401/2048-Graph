using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Bounds
{
    public struct RectangleBound : Bound
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public int Width { get { return XMax - X; } set { XMax -= Width - value; } }
        public int Height { get { return YMax - Y; } set { YMax -= Height - value; } }

        public static RectangleBound New(int x, int y, int width, int height)
        {
            RectangleBound r = new RectangleBound();
            r.X = x;
            r.Y = y;
            r.Width = width;
            r.Height = height;
            return r;
        }

        public bool Intersect(Point other)
        {
            if (other.X < X)
                return false;
            if (other.Y < Y)
                return false;
            if (other.X > XMax)
                return false;
            if (other.Y > YMax)
                return false;
            return true;
        }

        public bool Intersect(Bound other)
        {
            if (other is RectangleBound)
                return Intersect((RectangleBound)other);
            return false;
        }

        public bool Intersect(RectangleBound other)
        {
            if (other.X >= XMax)
                return false;
            if (other.Y >= YMax)
                return false;
            if (other.XMax <= X)
                return false;
            if (other.YMax <= Y)
                return false;
            return true;
        }
        public int getMinX()
        {
            return X;
        }
        public int getMinY()
        {
            return Y;
        }
        public int getMaxWidth()
        {
            return Width;
        }
        public int getMaxHeight()
        {
            return Height;
        }
    }
}
