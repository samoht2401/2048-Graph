using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using OpenTK.Input;
using Gui.Helper;
using Gui.Controls;
using Gui.Bounds;

namespace Gui.Screens
{
    public class ScreenManager
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        private Stack<Screen> Screens;
        private Screen toOpenWhenCleared;

        public ScreenManager()
        {
            Screens = new Stack<Screen>();
            InputHelper.Mouse.Move += MouseMoveEvent;
            InputHelper.Mouse.ButtonUp += MouseButtonUpEvent;
            InputHelper.Mouse.ButtonDown += MouseButtonDownEvent;
            InputHelper.Mouse.WheelChanged += MouseWheelChangedEvent;
        }
        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            foreach (Screen screen in Screens)
                screen.Resize(width, height);
        }

        public void OpenScreen(Screen toOpen)
        {
            Screens.Push(toOpen);
            toOpen.Open();
        }
        public void CloseScreen()
        {
            if (Screens.Count > 0)
                Screens.Peek().Close();
        }
        public void CloseAllAndThenOpen(Screen toOpen)
        {
            foreach (Screen toClose in Screens)
                toClose.Close();
            toOpenWhenCleared = toOpen;
        }

        public void Update(TimeSpan elapsedTime)
        {
            Screen foregroundScreen = Screens.Peek();
            List<Screen> Temp = Screens.ToList();
            foreach (Screen screen in Temp)
            {
                if (screen.State == Screen.States.FullyClosed)
                {
                    if (screen == foregroundScreen)
                        Screens.Pop();
                    else
                        continue;
                }
                screen.Update(elapsedTime, screen == foregroundScreen);
            }
            if (toOpenWhenCleared != null && Screens.Count == 0)
            {
                OpenScreen(toOpenWhenCleared);
                toOpenWhenCleared = null;
            }
        }
        public void Draw(TimeSpan elapsed)
        {
            Screen foregroundScreen = Screens.Peek();
            Screens.Reverse();
            List<Screen> Temp = Screens.ToList();
            foreach (Screen screen in Temp)
            {
                screen.Draw(elapsed, screen == foregroundScreen);
            }
            Screens.Reverse();
            DrawHelper.DrawBorderMask();
        }

        public void MouseMoveEvent(object sender, MouseMoveEventArgs e)
        {
            Point pts = InputHelper.ConvertScreenToWorldCoords(e.X, e.Y);
            MouseMoveEventArgs args = new MouseMoveEventArgs(pts.X, pts.Y, e.XDelta, e.YDelta);
            if (Screens.Count > 0)
                Screens.Peek().MouveMoveEvent(sender, new GuiMouseMoveEventArgs(args));
        }
        public void MouseButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            Point pts = InputHelper.ConvertScreenToWorldCoords(e.X, e.Y);
            MouseButtonEventArgs args = new MouseButtonEventArgs(pts.X, pts.Y, e.Button, e.IsPressed);
            if (Screens.Count > 0)
                Screens.Peek().MouseButtonUpEvent(sender, new GuiMouseButtonEventArgs(args));
        }
        public void MouseButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            Point pts = InputHelper.ConvertScreenToWorldCoords(e.X, e.Y);
            MouseButtonEventArgs args = new MouseButtonEventArgs(pts.X, pts.Y, e.Button, e.IsPressed);
            if (Screens.Count > 0)
                Screens.Peek().MouseButtonDownEvent(sender, new GuiMouseButtonEventArgs(args));
        }
        void MouseWheelChangedEvent(object sender, MouseWheelEventArgs e)
        {
            if (Screens.Count > 0)
                Screens.Peek().MouseWheelEvent(sender, new GuiMouseWheelEventArgs(e));
        }
    }
}
