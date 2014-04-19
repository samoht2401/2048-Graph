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

namespace _2048_Graph.Screens
{
    public class MainMenuScreen : Screen
    {
        Texture testTex;
        float lastWheel;
        float zoom = 1;

        public MainMenuScreen(ScreenManager manager)
            : base(manager)
        {
            OpeningTransition = new TranslationTransition(Transition.Types.Opening, TranslationTransition.Directions.Left);
            ClosingTransition = new TranslationTransition(Transition.Types.Closing, TranslationTransition.Directions.Right);

            CompoundSprite sprites = new CompoundSprite();
            sprites.Add("Idle", new Sprite(TextureHelper.LoadTexture("Sprites\\Button\\base_released.png"), 256, 64));
            sprites.Add("Overflew", new Sprite(TextureHelper.LoadTexture("Sprites\\Button\\base_released.png"), 256, 64));
            sprites.Add("Pressed", new Sprite(TextureHelper.LoadTexture("Sprites\\Button\\base_pressed.png"), 256, 64));
            Controls.Add("button", new Button(sprites, RectangleBound.New(100, 100, 400, 100), 500));
            ((Button)Controls["button"]).Click += button_MouseButtonDown;

            //testTex = TextureHelper.LoadTexture("Sprites\\pieuvre.png");
            testTex = TextureHelper.LoadTexture("supernova.png");
        }

        void button_MouseButtonDown(object sender, GuiMouseButtonEventArgs e)
        {
            Manager.CloseAllAndThenOpen(new MainMenuScreen(Manager));
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
            DrawHelper.Zoom = zoom;
        }

        public override void Draw(TimeSpan elapsed, bool isInForeground)
        {
            base.Draw(elapsed, isInForeground);

            int size = Math.Max(Manager.Width, Manager.Height) / 2;

            ApplyTransitionTransformation();

            testTex.Bind();
            DrawHelper.Draw2DSprite(0, 0, 1024, 1024, -1f);

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
