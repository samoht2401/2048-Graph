using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Gui.Bounds;
using Gui.Sprites;
using Gui.Helper;

namespace Gui.Controls
{
    public class ShakingButton : Button
    {
        public float ShakingSpeed { get; set; }
        public float ShakingAmplitude { get; set; }
        public float ShakingWaveLenght { get; set; }

        public ShakingButton(CompoundSprite sprites, Bound bound, double clickLenght)
            : base(sprites, bound, clickLenght)
        {
            ShakingSpeed = 3;
            ShakingAmplitude = 5f;
            ShakingWaveLenght = float.PositiveInfinity;
        }

        float time = 0;
        public override void Draw(TimeSpan elapsed)
        {
            time += (float)elapsed.TotalSeconds;
            float rotate = 0;
            if (State == States.Overflew)
                rotate = (float)Math.Sin(time * ShakingSpeed * 2 * Math.PI) * ShakingAmplitude * (float)Math.Cos(time / ShakingWaveLenght *  Math.PI);
            else
                time = 0;
            Sprite currentSprite = Sprites.Sprites[State.ToString()];
            currentSprite.Texture.Bind();
            DrawHelper.Draw2DSprite(Bound.getMinX() + Bound.getMaxWidth() / 2, Bound.getMinY() + Bound.getMaxHeight() / 2, Bound.getMaxWidth(), Bound.getMaxHeight(), rotate, 0, true);
        }
    }
}
