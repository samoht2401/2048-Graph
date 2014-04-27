using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Gui.Helper;
using Gui.Bounds;

namespace Gui.Sprites
{
    public class SpriteSheet
    {
        public Texture Texture { get; protected set; }
        public int Width { get { return Texture.Width; } }
        public int Height { get { return Texture.Height; } }
        public int SpriteWidth { get; protected set; }
        public int SpriteHeight { get; protected set; }

        public SpriteSheet(string name, int spriteWidth, int spriteHeight)
            : this(TextureHelper.LoadTexture(name), spriteWidth, spriteHeight)
        {
        }
        public SpriteSheet(Texture texture, int spriteWidth, int spriteHeight)
        {
            Texture = texture;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
        }

        public Sprite GetSprite(int x, int y)
        {
            return GetSprite(x, y, SpriteWidth, SpriteHeight);
        }
        public Sprite GetSprite(int x, int y, int width, int height)
        {
            Vector4 tecCoordRect = Vector4.Zero;
            tecCoordRect.X = (float)x * SpriteWidth / Width;
            tecCoordRect.Y = (float)y * SpriteHeight / Height;
            tecCoordRect.Z = tecCoordRect.X + (float)SpriteWidth / Width;
            tecCoordRect.W = tecCoordRect.Y +(float)SpriteHeight / Height;
            return new Sprite(Texture, width, height, tecCoordRect);
        }
    }
}
