using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
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
        private Vector4 TexCoordRect;

        public Sprite(Texture tex, int width, int height) : this(tex, width, height, Vector4.UnitZ + Vector4.UnitW) { }
        public Sprite(Texture tex, int width, int height, Vector4 texCoordRect)
        {
            Texture = tex;
            Zones = new Dictionary<string, RectangleBound>();
            Width = width;
            Height = height;
            TexCoordRect = texCoordRect;
        }

        public void Draw(int x, int y, float scale = 1, float rotate = 0, float depth = 0, bool centered = false)
        {
            Draw(x, y, (int)(Width * scale), (int)(Height * scale), rotate, depth, centered);
        }
        public void Draw(int x, int y, int width, int height, float rotate = 0, float depth = 0, bool centered = false)
        {
            Texture.Bind();
            DrawHelper.Draw2DSprite(x, y, width, height, TexCoordRect, rotate, depth, centered);
        }
    }
}
