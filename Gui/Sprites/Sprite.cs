using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gui.Bounds;
using Gui.Helper;

namespace Gui.Sprites
{
    public class Sprite
    {
        public Texture Texture { get; protected set; }
        public Dictionary<string, RectangleBound> Zones { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Sprite(Texture tex, int width, int height)
        {
            Texture = tex;
            Zones = new Dictionary<string, RectangleBound>();
            Width = width;
            Height = height;
        }

        public void Bind()
        {
            Texture.Bind();
        }
    }
}
