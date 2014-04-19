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
            /*System.Drawing.Imaging.BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, img.PixelFormat);
            int length = bitmapData.Stride * bitmapData.Height;
            byte[] bytes = new byte[length];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, bytes, 0, length);
            img.UnlockBits(bitmapData);*/
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
    }
}
