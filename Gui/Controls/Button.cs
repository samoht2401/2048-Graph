using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Gui.Bounds;
using Gui.Helper;
using Gui.Sprites;
using Gui.Controls;

namespace Gui.Controls
{
    public class Button : Control
    {
        public enum States
        {
            Idle,
            Overflew,
            Pressed
        };

        public TimeSpan PressedTime { get; set; }
        private TimeSpan timeRemainingPressed;
        public States State { get; protected set; }

        public event GuiMouseButtonEventHandler Click;

        public Button(CompoundSprite sprites, Bound bound, double clickLenght)
            : base(sprites)
        {
            PressedTime = TimeSpan.FromMilliseconds(clickLenght);
            timeRemainingPressed = PressedTime;
            Bound = bound;
            MouseEnter += Button_MouseEnter;
            MouseLeave += Button_MouseLeave;
            MouseButtonDown += Button_MouseButtonDown;
        }

        void Button_MouseLeave(object sender, GuiMouseMoveEventArgs e)
        {
            if (State != States.Pressed)
                State = States.Idle;
        }
        void Button_MouseEnter(object sender, GuiMouseMoveEventArgs e)
        {
            if (State != States.Pressed)
                State = States.Overflew;
        }
        void Button_MouseButtonDown(object sender, GuiMouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left && State != States.Pressed)
            {
                State = States.Pressed;
                if (Click != null)
                    Click(this, e);
            }
        }

        public override void Update(TimeSpan elapsed)
        {
            if (State == States.Pressed)
            {
                timeRemainingPressed -= elapsed;
                if (timeRemainingPressed.TotalMilliseconds <= 0)
                {
                    if (Bound.Intersect(InputHelper.MouseOpenGLPosition))
                        State = States.Overflew;
                    else
                        State = States.Idle;
                    timeRemainingPressed = PressedTime;
                }
            }
            UpdateChilds(elapsed);
        }
        public override void Draw(TimeSpan elapsed)
        {
            Sprite currentSprite = Sprites.Sprites[State.ToString()];
            currentSprite.Texture.Bind();
            DrawHelper.Draw2DSprite(Bound.getMinX(), Bound.getMinY(), Bound.getMaxWidth(), Bound.getMaxHeight());
        }
    }
}
