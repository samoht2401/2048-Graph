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
    public static class TextureHelper
    {
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        public static Texture LoadTexture(string name)
        {
            if (textures.ContainsKey(name))
                return textures[name];

            Bitmap img = new Bitmap(Directory.GetCurrentDirectory() + "\\Img\\" + name);

            System.Drawing.Imaging.BitmapData data = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            img.UnlockBits(data);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, 64, 64, 0, PixelFormat.Rgb, PixelType.UnsignedByte, bytes);

            Texture result = new Texture(id, img.Width, img.Height);
            textures.Add(name, result);
            return result;
        }

        public static Texture GetTextTexture(string text)
        {
            Bitmap img = new Bitmap(64, 64 * text.Length);
            Font font = new Font(FontFamily.GenericSansSerif, 64, FontStyle.Regular);
            //Brush brush = new SolidBrush(Color.Black);=

            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.Transparent);
                //g.DrawString(text, font, brush, PointF.Empty);
            }

            System.Drawing.Imaging.BitmapData data = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            img.UnlockBits(data);

            Texture result = new Texture(id, img.Width, img.Height);
            return result;
        }
    }
}
