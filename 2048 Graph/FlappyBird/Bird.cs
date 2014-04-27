using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Gui.Sprites;
using Gui.Helper;
using Gui.Bounds;

namespace _2048_Graph.FlappyBird
{
    public class Bird
    {
        private const float ACCELERATION = 150f;
        private const float GRAVITY = 60f;
        private const float MAX_VERTICAL_VELOCITY = 500f;

        private Sprite idleBirdSprite;
        private Animation flyingBirdSprite;
        private bool isSpaceDown;

        private Vector2 position;
        public Vector2 Position { get { return position; } }
        private Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } }
        private RectangleBound bound;
        public RectangleBound Bound { get { return bound; } }
        public bool IsDead { get; set; }

        public Bird()
        {
            SpriteSheet birdSheet = new SpriteSheet("Sprites\\FlappyBird\\bird_17x12.png", 17, 12);
            idleBirdSprite = birdSheet.GetSprite(1, 0);
            flyingBirdSprite = new Animation(birdSheet, 20, true);

            InputHelper.Keyboard.KeyDown += Keyboard_KeyDown;
            InputHelper.Keyboard.KeyUp += Keyboard_KeyUp;
            isSpaceDown = false;
            IsDead = false;
            position.X = 80;
            RecaculeBound();
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Space)
                isSpaceDown = true;
        }

        void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Space)
                isSpaceDown = false;
        }

        public void Update(TimeSpan elapsed)
        {
            if (isSpaceDown && !IsDead)
                velocity.Y -= ACCELERATION;
            velocity.Y += GRAVITY;

            if (Math.Abs(velocity.Y) > MAX_VERTICAL_VELOCITY)
            {
                if (velocity.Y > 0)
                    velocity.Y = MAX_VERTICAL_VELOCITY;
                if (velocity.Y < 0)
                    velocity.Y = -MAX_VERTICAL_VELOCITY;
            }

            position += Vector2.Multiply(velocity, (float)elapsed.TotalSeconds);
            if (position.Y < -10 || position.Y > 700)
                IsDead = true;
            RecaculeBound();
        }

        public void RecaculeBound()
        {
            bound = RectangleBound.New((int)position.X, (int)position.Y, idleBirdSprite.Width * 4, idleBirdSprite.Height * 4);
        }

        public void Draw(TimeSpan elapsed)
        {
            if (isSpaceDown)
                flyingBirdSprite.Draw((float)elapsed.TotalSeconds, (int)position.X, (int)position.Y, 4);
            else
                idleBirdSprite.Draw((int)position.X, (int)position.Y, 4);
        }
    }
}
