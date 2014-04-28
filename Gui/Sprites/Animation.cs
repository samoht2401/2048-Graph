using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Sprites
{
    public class Animation
    {
        public List<Sprite> Sprites { get; protected set; }
        public float FramePerSecond { get; set; }
        public bool Loop { get; set; }

        public Animation(SpriteSheet sheet, float fps, bool loop)
        {
            Sprites = new List<Sprite>();
            for (int y = 0; y < sheet.Height / sheet.SpriteHeight; y++)
            {
                for (int x = 0; x < sheet.Width / sheet.SpriteWidth; x++)
                {
                    Sprites.Add(sheet.GetSprite(x, y));
                }
            }
            FramePerSecond = fps;
            Loop = loop;
        }

        private float timer;
        private int index;
        private void ComputeElapsed(float elapsed)
        {
            timer += elapsed;
            if (timer > 1 / FramePerSecond)
            {
                timer -= 1 / FramePerSecond;
                index++;
                if (index >= Sprites.Count)
                    index = 0;
            }
        }
        public void Draw(float elapsed, int x, int y, float scale = 1, float rotate = 0, float depth = 0, bool centered = false)
        {
            ComputeElapsed(elapsed);
            Sprites[index].Draw(x, y, scale, rotate, depth, centered);
        }
        public void Draw(float elapsed, int x, int y, int width, int height, float rotate = 0, float depth = 0, bool centered = false)
        {
            ComputeElapsed(elapsed);
            Sprites[index].Draw(x, y, width, height, rotate, depth, centered);
        }
    }
}
