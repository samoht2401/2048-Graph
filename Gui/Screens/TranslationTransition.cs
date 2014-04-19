using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Gui.Screens
{
    public class TranslationTransition : Transition
    {
        public enum Directions
        {
            Left,
            Right,
            Up,
            Down
        };

        public Directions Direction { get; set; }

        public TranslationTransition(Transition.Types type, Directions dir)
            : base(type)
        {
            TotalTime = TimeSpan.FromMilliseconds(1000);
            Direction = dir;
        }

        public override void ApplyTransformation(Screen screen)
        {
            double mult = Avancement;
            if (Type == Types.Closing)
                mult = 1 - mult;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            double trans = 0;
            if (Direction == Directions.Left || Direction == Directions.Up)
                trans = (1 - mult) * -screen.Manager.Width / 2;
            if (Direction == Directions.Right || Direction == Directions.Down)
                trans = (1 - mult) * screen.Manager.Width / 2;
            if (Direction == Directions.Left || Direction == Directions.Right)
                GL.Translate(trans, 0, 0);
            if (Direction == Directions.Up || Direction == Directions.Down)
                GL.Translate(0, trans, 0);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Color4(1, 1, 1, mult);
        }

        public override void UndoTransformation(Screen screen)
        {
            GL.Color4(1, 1, 1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }
    }
}
