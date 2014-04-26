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

namespace _2048_Graph.Screens
{
    public class MainMenuScreen : Screen
    {
        Texture testTex;
        Animation birdAnim;
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

            SpriteSheet birdSheet = new SpriteSheet("Sprites\\FlappyBird\\bird_17x12.png", 17, 12);
            birdAnim = new Animation(birdSheet, 20f, true);

            //testTex = TextureHelper.LoadTexture("Sprites\\pieuvre.png");
            testTex = TextureHelper.LoadTexture("supernova.png");
        }

        void button_MouseButtonDown(object sender, GuiMouseButtonEventArgs e)
        {
            Manager.CloseAllAndThenOpen(new Game2048Screen(Manager));
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

            birdAnim.Draw((float)elapsed.TotalSeconds, 60, 200, 5);

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
