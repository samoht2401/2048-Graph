using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Bounds
{
    public interface Bound
    {
        bool Intersect(Point other);
        bool Intersect(Bound other);

        int getMinX();
        int getMinY();
        int getMaxWidth();
        int getMaxHeight();
    }
}
