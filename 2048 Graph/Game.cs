using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Gui.Helper;
using Gui.Screens;
using _2048_Graph.Screens;

namespace _2048_Graph
{
    public class Game : GameWindow
    {
        ScreenManager screenManager;

        public Game()
            : base()
        {
            InputHelper.Init(Keyboard, Mouse);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DrawHelper.Enable2D();

            screenManager = new ScreenManager();
            screenManager.OpenScreen(new MainMenuScreen(screenManager));
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            DrawHelper.Disable2D();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            screenManager.Resize(Width, Height);
            WindowBorder = WindowBorder.Resizable;
            DrawHelper.Reset2DMatrix();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            screenManager.Update(TimeSpan.FromSeconds(e.Time));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            DrawHelper.Update();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            screenManager.Draw(TimeSpan.FromSeconds(e.Time));
            /*Tex.Bind();
            DrawHelper.Draw2DSprite(0, 0, 128, 128);
            DrawHelper.Draw2DSprite(128, 0, 128, 128);
            DrawHelper.Draw2DSprite(0, 128, 128, 128);
            DrawHelper.Draw2DSprite(128, 128, 128, 128);
            DrawHelper.Draw2DSprite(256, 0, 128, 128);
            DrawHelper.Draw2DSprite(0, 256, 128, 128);
            DrawHelper.Draw2DSprite(256, 256, 128, 128);
            DrawHelper.Draw2DSprite(256, 128, 128, 128);
            DrawHelper.Draw2DSprite(128, 256, 128, 128);
            DrawHelper.Draw2DSprite(384, 0, 128, 128);
            DrawHelper.Draw2DSprite(384, 128, 128, 128);
            DrawHelper.Draw2DSprite(384, 256, 128, 128);
            DrawHelper.Draw2DSprite(384, 384, 128, 128);
            DrawHelper.Draw2DSprite(0, 384, 128, 128);
            DrawHelper.Draw2DSprite(128, 384, 128, 128);
            DrawHelper.Draw2DSprite(256, 384, 128, 128);*/

            SwapBuffers();
        }
    }
}
