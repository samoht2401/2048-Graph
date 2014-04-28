using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Gui.Screens;
using Gui.Helper;
using Gui.Sprites;
using _2048_Graph.Screens;

namespace _2048_Graph.FlappyBird
{
    public class GameFlappyBirdScreen : Screen
    {
        private Sprite background;
        private Bird bird;
        private List<Pipe> pipes;
        private bool hasStarted = false;

        public GameFlappyBirdScreen(ScreenManager manager)
            : base(manager)
        {
            OpeningTransition = new TranslationTransition(Transition.Types.Opening, TranslationTransition.Directions.Left);
            ClosingTransition = new TranslationTransition(Transition.Types.Closing, TranslationTransition.Directions.Right);

            Texture tex = TextureHelper.LoadTexture("Sprites\\FlappyBird\\background.png");
            background = new Sprite(tex, Manager.Width, manager.Height, new Vector4(0, 0, 1920 / (tex.Width * (1080f / tex.Height)), 1f));

            bird = new Bird();
            pipes = new List<Pipe>();

            InputHelper.Keyboard.KeyDown += Keyboard_KeyDown;
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (!hasStarted && e.Key == OpenTK.Input.Key.Space)
                hasStarted = true;
        }

        float timer;
        public override void Update(TimeSpan elapsed, bool isInForeground)
        {
            base.Update(elapsed, isInForeground);

            if (!hasStarted)
                return;

            bird.Update(elapsed);

            List<Pipe> oldPipes = new List<Pipe>(pipes);
            foreach (Pipe pipe in oldPipes)
            {
                pipe.Update(elapsed);
                if (pipe.HolePosition.X < -90)
                    pipes.Remove(pipe);

                if (bird.Bound.Intersect(pipe.BoundTop) || bird.Bound.Intersect(pipe.BoundBottom))
                    bird.IsDead = true;
            }

            timer += (float)elapsed.TotalSeconds;
            if (timer >= 3)
            {
                AddNewPipe();
                timer -= 3;
            }

            if (State == States.Opened && bird.IsDead)
                Manager.CloseAllAndThenOpen(new MainMenuScreen(Manager));
        }

        int lastHolePos = 500;
        Random rand = new Random();
        int signe = 0;
        private void AddNewPipe()
        {
            int r = rand.Next(300);
            if (signe == -1)
                signe = rand.Next(2);
            int height = lastHolePos;
            if (signe == 1)
                height += r;
            else
                height -= r;
            signe = -1;
            if (height < 100)
            {
                height = 100;
                signe = 1;
            }
            if (height > 500)
            {
                height = 500;
                signe = 2;
            }
            pipes.Add(new Pipe(new Vector2(Manager.Width, height), 150, -150));
            lastHolePos = height;
        }

        public override void Draw(TimeSpan elapsed, bool isInForeground)
        {
            base.Draw(elapsed, isInForeground);

            int size = Math.Max(Manager.Width, Manager.Height) / 2;

            ApplyTransitionTransformation();

            background.Draw(0, 0, 1f, 0, -1);

            bird.Draw(elapsed);
            foreach (Pipe pipe in pipes)
            {
                pipe.Draw(elapsed);
            }

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
