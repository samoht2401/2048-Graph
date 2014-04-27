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
        private Texture background;
        private Bird bird;
        private List<Pipe> pipes;

        public GameFlappyBirdScreen(ScreenManager manager)
            : base(manager)
        {
            OpeningTransition = new TranslationTransition(Transition.Types.Opening, TranslationTransition.Directions.Left);
            ClosingTransition = new TranslationTransition(Transition.Types.Closing, TranslationTransition.Directions.Right);

            background = TextureHelper.LoadTexture("Sprites\\FlappyBird\\background.png");

            bird = new Bird();
            pipes = new List<Pipe>();
        }

        public override void Open()
        {
            base.Open();

            Game.ForceResize(background.Width * 6, background.Height * 3);
        }

        public override void Close()
        {
            base.Close();

            Game.FreeResize();
        }

        float timer;
        public override void Update(TimeSpan elapsed, bool isInForeground)
        {
            base.Update(elapsed, isInForeground);

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
            if (timer >= 2)
            {
                AddNewPipe();
                timer -= 2;
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

            background.Bind();
            DrawHelper.Draw2DSprite(0, 0, background.Width * 3, background.Height * 3, -1);
            DrawHelper.Draw2DSprite(background.Width * 3, 0, background.Width * 3, background.Height * 3, -1);

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
