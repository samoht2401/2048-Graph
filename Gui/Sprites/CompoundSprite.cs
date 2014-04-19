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

        public void Add(string key, Sprite val)
        {
            if (!Sprites.ContainsKey(key))
                Sprites.Add(key, val);
        }
    }
}
