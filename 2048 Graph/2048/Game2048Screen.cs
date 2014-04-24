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
        private Dictionary<int, Texture> TextureNumbersDic = new Dictionary<int, Texture>();
        private Texture background;
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

            TextureNumbersDic.Add(2, TextureHelper.LoadTexture("Sprites\\2048\\2.png"));
            TextureNumbersDic.Add(4, TextureHelper.LoadTexture("Sprites\\2048\\4.png"));
            TextureNumbersDic.Add(8, TextureHelper.LoadTexture("Sprites\\2048\\8.png"));
            TextureNumbersDic.Add(16, TextureHelper.LoadTexture("Sprites\\2048\\16.png"));
            TextureNumbersDic.Add(32, TextureHelper.LoadTexture("Sprites\\2048\\32.png"));
            TextureNumbersDic.Add(64, TextureHelper.LoadTexture("Sprites\\2048\\64.png"));
            TextureNumbersDic.Add(128, TextureHelper.LoadTexture("Sprites\\2048\\128.png"));
            TextureNumbersDic.Add(256, TextureHelper.LoadTexture("Sprites\\2048\\256.png"));
            TextureNumbersDic.Add(512, TextureHelper.LoadTexture("Sprites\\2048\\512.png"));
            TextureNumbersDic.Add(1024, TextureHelper.LoadTexture("Sprites\\2048\\1024.png"));
            TextureNumbersDic.Add(2048, TextureHelper.LoadTexture("Sprites\\2048\\2048.png"));
            background = TextureHelper.LoadTexture("Sprites\\2048\\grille.png");
            scoreTex = new Text(score.ToString());
            scoreTex.Font = new Font("Arial", 24, FontStyle.Regular);

            InputHelper.Keyboard.KeyDown += Keyboard_KeyDown;

            GetNewNumber();
        }

        public override void Open()
        {
            base.Open();

            Game.ForceResize(533, 670);
        }

        public override void Close()
        {
            base.Close();

            Game.FreeResize();
        }

        public override void Update(TimeSpan elapsed, bool isInForeground)
        {
            base.Update(elapsed, isInForeground);

            if (State == States.Opened && IsLost())
                Manager.CloseAllAndThenOpen(new MainMenuScreen(Manager));

            if (score != oldScore)
            {
                scoreTex.Texte = score.ToString();
                if(score>999 && oldScore <=999)
                    scoreTex.Font = new Font("Arial", 18, FontStyle.Regular);
                if (score > 9999 && oldScore <= 9999)
                    scoreTex.Font = new Font("Arial", 16, FontStyle.Regular);
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

            background.Bind();
            DrawHelper.Draw2DSprite(0, 0, 533, 670, -1);
            scoreTex.Draw(380, 50, 0,true);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (tableau[x, y] != 0)
                    {
                        TextureNumbersDic[tableau[x, y]].Bind();
                        DrawHelper.Draw2DSprite(27 + 121 * x, 175 + 121 * y, 106, 106);
                    }
                }
            }

            DrawControls(elapsed, isInForeground);

            UndoTransitionTransformation();
        }
    }
}
