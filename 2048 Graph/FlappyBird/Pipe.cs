using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Gui.Sprites;
using Gui.Bounds;

namespace _2048_Graph.FlappyBird
{
    public class Pipe
    {
        private Sprite[] pipeSprites;

        private Vector2 holePosition;
        public Vector2 HolePosition { get { return holePosition; } }
        private int holeHeight;
        public int HoleHeight { get { return holeHeight; } }
        public float Velocity { get; protected set; }
        private RectangleBound boundTop;
        public RectangleBound BoundTop { get { return boundTop; } }
        private RectangleBound boundBottom;
        public RectangleBound BoundBottom { get { return boundBottom; } }

        public Pipe(Vector2 holeP, int holeH, float velocity)
        {
            SpriteSheet pipeSheet = new SpriteSheet("Sprites\\FlappyBird\\pipe_26x12.png", 26, 12);
            pipeSprites = new Sprite[2];
            pipeSprites[0] = pipeSheet.GetSprite(0, 0);
            pipeSprites[1] = pipeSheet.GetSprite(1, 0);

            holePosition = holeP;
            holeHeight = holeH;
            Velocity = velocity;
            RecaculeBound();
        }

        public void Update(TimeSpan elapsed)
        {
            holePosition.X += (float)(Velocity * elapsed.TotalSeconds);
            RecaculeBound();
        }

        public void RecaculeBound()
        {
            boundTop = RectangleBound.New((int)holePosition.X, -100, pipeSprites[0].Width * 3, (int)holePosition.Y + 100);
            boundBottom = RectangleBound.New((int)holePosition.X, (int)holePosition.Y + holeHeight, pipeSprites[0].Width * 3, 1000);
        }

        public void Draw(TimeSpan elapsed)
        {
            pipeSprites[0].Draw((int)holePosition.X, (int)holePosition.Y - pipeSprites[0].Height * 3, 3);
            pipeSprites[1].Draw((int)holePosition.X, 0, pipeSprites[1].Width * 3, (int)holePosition.Y - pipeSprites[0].Height * 3);
            pipeSprites[0].Draw((int)holePosition.X, (int)holePosition.Y + holeHeight, 3);
            pipeSprites[1].Draw((int)holePosition.X, (int)holePosition.Y + holeHeight + pipeSprites[0].Height * 3, pipeSprites[1].Width * 3, 1000);
        }
    }
}
