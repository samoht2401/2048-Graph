using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Gui.Helper
{
    public class Texture
    {
        private int gl_id;
        public readonly int Width;
        public readonly int Height;

        public Texture(int id, int width, int height)
        {
            gl_id = id;
            Width = width;
            Height = height;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, gl_id);
        }
    }
}
