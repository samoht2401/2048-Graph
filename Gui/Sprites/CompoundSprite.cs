using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Sprites
{
    public class CompoundSprite
    {
        public Dictionary<string, Sprite> Sprites { get; protected set; }

        public CompoundSprite()
        {
            Sprites = new Dictionary<string, Sprite>();
        }

        public CompoundSprite(params object[] content)
        {
            Sprites = new Dictionary<string, Sprite>();
            for (int i = 0; i < content.Length / 2; i++)
                Sprites.Add((string)content[i], (Sprite)content[i + 1]);
        }

        public void Add(string key, Sprite val)
        {
            if (!Sprites.ContainsKey(key))
                Sprites.Add(key, val);
        }
    }
}
