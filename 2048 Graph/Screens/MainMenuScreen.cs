using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Gui.Screens;
using Gui.Sprites;
using Gui.Helper;
using Gui.Controls;
using Gui.Bounds;
using _2048_Graph._2048;
using _2048_Graph.FlappyBird;

namespace _2048_Graph.Screens
{
    public class MainMenuScreen : Screen
    {
        float lastWheel;
        float zoom = 1;

        public MainMenuScreen(ScreenManager manager)
            : base(manager)
        {
            OpeningTransition = new TranslationTransition(Transition.Types.Opening, TranslationTransition.Directions.Left);
            ClosingTransition = new TranslationTransition(Transition.Types.Closing, TranslationTransition.Directions.Right);

            CompoundSprite sprites = new CompoundSprite();
            Texture buttonTex = TextureHelper.LoadTexture("Sprites\\Button\\2048_button.png");
            sprites.Add("Idle", new Sprite(buttonTex, 256, 64));
            sprites.Add("Overflew", new Sprite(buttonTex, 256, 64));
            sprites.Add("Pressed", new Sprite(buttonTex, 256, 64));
            Controls.Add("2048_button", new ShakingButton(sprites, DrawHelper.GetBoundRelativeOnScreen(0.5f, 0.2f, buttonTex.Width / 2, buttonTex.Height / 2), 100));
            ((Button)Controls["2048_button"]).Click += _2048_button_MouseButtonDown;

            sprites = new CompoundSprite();
            buttonTex = TextureHelper.LoadTexture("Sprites\\Button\\flappyBird_button.png");
            sprites.Add("Idle", new Sprite(buttonTex, 256, 64));
            sprites.Add("Overflew", new Sprite(buttonTex, 256, 64));
            sprites.Add("Pressed", new Sprite(buttonTex, 256, 64));
            Controls.Add("flappyBird_button", new ShakingButton(sprites, DrawHelper.GetBoundRelativeOnScreen(0.5f, 0.7f, buttonTex.Width * 5, buttonTex.Height * 5), 100));
            ((Button)Controls["flappyBird_button"]).Click += flappyBird_button_MouseButtonDown;
        }

        void _2048_button_MouseButtonDown(object sender, GuiMouseButtonEventArgs e)
        {
            Manager.CloseAllAndThenOpen(new Game2048Screen(Manager));
        }
        void flappyBird_button_MouseButtonDown(object sender, GuiMouseButtonEventArgs e)
        {
            Manager.CloseAllAndThenOpen(new GameFlappyBirdScreen(Manager));
        }

        public override void Update(TimeSpan elapsed, bool isInForeground)
        {
            base.Update(elapsed, isInForeground);

            if (InputHelper.Mouse.WheelPrecise != lastWheel)
            {
                float diff = lastWheel - InputHelper.Mouse.WheelPrecise;
                diff *= (float)elapsed.TotalSeconds * 10;
                if (diff > 0)
                    zoom *= diff;
                else
                    zoom /= -diff;
                lastWheel = InputHelper.Mouse.WheelPrecise;
            }
        }

        public override void Draw(TimeSpan elapsed, bool isInForeground)
        {
            base.Draw(elapsed, isInForeground);

            int size = Math.Max(Manager.Width, Manager.Height) / 2;

            ApplyTransitionTransformation();

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
