using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Gui.Screens;
using Gui.Sprites;
using Gui.Helper;
using Gui.Controls;
using Gui.Bounds;
using _2048_Graph.Screens;

namespace _2048_Graph._2048
{
    public class Game2048Screen : Screen
    {
        private Dictionary<int, Sprite> TextureNumbersDic = new Dictionary<int, Sprite>();
        private Sprite background;
        private float backgroundScale;
        private Text scoreTex;
        private int oldScore = 0;
        private int score = 0;

        int[,] tableau = new int[4, 4]{ { 0, 0, 0, 0 }, // pour [X,Y] : X vertical, Y horizontale
	                                    { 0, 0, 0, 0 }, // !!! La table est donc transposé (symétrique à la diagonale haut-gauche, bas-droite)
	                                    { 0, 0, 0, 0 },
	                                    { 0, 0, 0, 0 } };

        public Game2048Screen(ScreenManager manager)
            : base(manager)
        {
            OpeningTransition = new TranslationTransition(Transition.Types.Opening, TranslationTransition.Directions.Left);
            ClosingTransition = new TranslationTransition(Transition.Types.Closing, TranslationTransition.Directions.Right);

            Texture tex = TextureHelper.LoadTexture("Sprites\\2048\\grille.png");
            background = new Sprite(tex, (int)((float)tex.Width / tex.Height * Manager.Height), Manager.Height);
            backgroundScale = (float)background.Height / tex.Height;
            int size = (int)(backgroundScale * 106);
            TextureNumbersDic.Add(2, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\2.png"), size, size));
            TextureNumbersDic.Add(4, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\4.png"), size, size));
            TextureNumbersDic.Add(8, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\8.png"), size, size));
            TextureNumbersDic.Add(16, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\16.png"), size, size));
            TextureNumbersDic.Add(32, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\32.png"), size, size));
            TextureNumbersDic.Add(64, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\64.png"), size, size));
            TextureNumbersDic.Add(128, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\128.png"), size, size));
            TextureNumbersDic.Add(256, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\256.png"), size, size));
            TextureNumbersDic.Add(512, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\512.png"), size, size));
            TextureNumbersDic.Add(1024, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\1024.png"), size, size));
            TextureNumbersDic.Add(2048, new Sprite(TextureHelper.LoadTexture("Sprites\\2048\\2048.png"), size, size));

            scoreTex = new Text(score.ToString());
            scoreTex.Font = new Font("Arial", 34, FontStyle.Regular);

            InputHelper.Keyboard.KeyDown += Keyboard_KeyDown;

            GetNewNumber();
        }

        /*public override void Open()
        {
            base.Open();

            Game.ForceResize(533, 670);
        }

        public override void Close()
        {
            base.Close();

            Game.FreeResize();
        }*/

        public override void Update(TimeSpan elapsed, bool isInForeground)
        {
            base.Update(elapsed, isInForeground);

            if (State == States.Opened && IsLost())
                Manager.CloseAllAndThenOpen(new MainMenuScreen(Manager));

            if (score != oldScore)
            {
                scoreTex.Texte = score.ToString();
                if (score > 999 && oldScore <= 999)
                    scoreTex.Font = new Font("Arial", 30, FontStyle.Regular);
                if (score > 9999 && oldScore <= 9999)
                    scoreTex.Font = new Font("Arial", 24, FontStyle.Regular);
            }
            oldScore = score;
        }

        private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            bool changed = false;
            if (e.Key == Key.Left) // Left Arrow is pressed
            {
                for (int x = 1; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        int val = tableau[x, y];
                        if (val != 0)
                        {
                            for (int i = 1; i <= x; i++)
                            {
                                int other = tableau[x - i, y];
                                if (other != 0)
                                {
                                    if (val == other)//Fusion
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x - i, y] = val * 2;
                                        score += val * 2;
                                        changed = true;
                                    }
                                    else if (i > 1)//Collision
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x - i + 1, y] = val;
                                        changed = true;
                                    }
                                    break;
                                }
                                else if (x - i == 0)//Border
                                {
                                    tableau[x, y] = 0;
                                    tableau[0, y] = val;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            if (e.Key == Key.Right) // Right Arrow is pressed
            {
                for (int x = 2; x >= 0; x--)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        int val = tableau[x, y];
                        if (val != 0)
                        {
                            for (int i = 1; i < 4 - x; i++)
                            {
                                int other = tableau[x + i, y];
                                if (other != 0)
                                {
                                    if (val == other)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x + i, y] = val * 2;
                                        score += val * 2;
                                        changed = true;
                                    }
                                    else if (i > 1)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x + i - 1, y] = val;
                                        changed = true;
                                    }
                                    break;
                                }
                                else if (x + i == 3)
                                {
                                    tableau[x, y] = 0;
                                    tableau[3, y] = val;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            if (e.Key == Key.Up) // Up Arrow is pressed
            {
                for (int y = 1; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int val = tableau[x, y];
                        if (val != 0)
                        {
                            for (int i = 1; i <= y; i++)
                            {
                                int other = tableau[x, y - i];
                                if (other != 0)
                                {
                                    if (val == other)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x, y - i] = val * 2;
                                        score += val * 2;
                                        changed = true;
                                    }
                                    else if (i > 1)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x, y - i + 1] = val;
                                        changed = true;
                                    }
                                    break;
                                }
                                else if (y - i == 0)
                                {
                                    tableau[x, y] = 0;
                                    tableau[x, 0] = val;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            if (e.Key == Key.Down) // Down Arrow is pressed
            {
                for (int y = 2; y >= 0; y--)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int val = tableau[x, y];
                        if (val != 0)
                        {
                            for (int i = 1; i < 4 - y; i++)
                            {
                                int other = tableau[x, y + i];
                                if (other != 0)
                                {
                                    if (val == other)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x, y + i] = val * 2;
                                        score += val * 2;
                                        changed = true;
                                    }
                                    else if (i > 1)
                                    {
                                        tableau[x, y] = 0;
                                        tableau[x, y + i - 1] = val;
                                        changed = true;
                                    }
                                    break;
                                }
                                else if (y + i == 3)
                                {
                                    tableau[x, y] = 0;
                                    tableau[x, 3] = val;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            if (changed && !IsTableauFull())
                GetNewNumber();
        }
        private bool IsTableauFull()
        {
            bool isThere0 = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (tableau[x, y] == 0)
                    {
                        isThere0 = true;
                        break;
                    }
                }
                if (isThere0)
                    break;
            }
            return !isThere0;
        }
        private void GetNewNumber()
        {
            //Randomize :
            int val = 0;
            Random rand = new Random();
            int r = rand.Next(0, 100);
            if (r < 80)
                val = 2;
            else
                val = 4;
            for (int i = 500; i > 0; i--)
            {
                r = rand.Next(0, 16);
                if (tableau[r % 4, r / 4] == 0)
                {
                    tableau[r % 4, r / 4] = val;
                    break;
                }
            }
        }
        private bool IsLost()
        {
            if (!IsTableauFull())
                return false;

            //Combine posible ?
            bool canCombine = false;
            for (int i = 0; i < 8; i++)
            {
                int pair = i / 2 % 2; // Pair == 0 // Impair == 1
                int x = i % 2 + pair;
                int y = i / 2;

                int val = tableau[x, y];
                if ((x > 0 && val == tableau[x - 1, y]) || (x < 3 && val == tableau[x + 1, y]) ||
                    (y > 0 && val == tableau[x, y - 1]) || (y < 3 && val == tableau[x, y + 1]))
                {
                    canCombine = true;
                    break;
                }
            }

            if (!canCombine)
                return true;
            return false;
        }

        public override void Draw(TimeSpan elapsed, bool isInForeground)
        {
            base.Draw(elapsed, isInForeground);
            ApplyTransitionTransformation();

            int xOffset = Manager.Width / 2 - background.Width / 2;
            float bs = backgroundScale;
            background.Draw(xOffset, 0, 1f, 0, -1);
            scoreTex.Draw(xOffset + (int)(380 * bs), (int)(50 * bs), 0, 0, true);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (tableau[x, y] != 0)
                    {
                        TextureNumbersDic[tableau[x, y]].Draw(xOffset + (int)(28 * bs + 121 * x * bs), (int)(175 * bs + 121 * y * bs));
                    }
                }
            }

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
