using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Gui.Helper
{
    public class Text
    {
        private Texture tex;

        private bool needRedraw;
        private string texte;
        public string Texte { get { return texte; } set { texte = value; needRedraw = true; } }
        private Font font;
        public Font Font { get { return font; } set { font = value; needRedraw = true; } }
        private Brush brush;
        public Brush Brush { get { return brush; } set { brush = value; needRedraw = true; } }

        public Text(string text)
        {
            Texte = text;
            Font = new Font(FontFamily.GenericSansSerif, 128, FontStyle.Regular);
            Brush = new SolidBrush(Color.Black);
            needRedraw = false;
            CreateTexture();
        }
        private void CreateTexture()
        {
            // Get the size
            SizeF size;
            Bitmap b = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(b))
            {
                size = g.MeasureString(Texte, Font);
            }

            Bitmap img = new Bitmap((int)size.Width, (int)size.Height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.Transparent);
                g.DrawString(Texte, Font, Brush, new PointF(0, 0));
            }

            System.Drawing.Imaging.BitmapData data = img.LockBits(new System.Drawing.Rectangle(0, 0, (int)img.Width, (int)img.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id;
            id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            img.UnlockBits(data);

            tex = new Texture(id, (int)size.Width, (int)size.Height);
        }
        private void DeleteTexture()
        {
            GL.DeleteTexture(tex.Id);
            tex = null;
        }

        public void Draw(int x, int y, float rotate =0, float depth = 0, bool centered = false)
        {
            if (needRedraw)
            {
                DeleteTexture();
                CreateTexture();
            }

            if (tex != null)
            {
                tex.Bind();
                DrawHelper.Draw2DSprite(x, y, tex.Width, tex.Height, rotate, depth, centered);
            }
        }
    }
}
