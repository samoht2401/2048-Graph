﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Gui.Helper
{
    public static class DrawHelper
    {
        private static bool needRefreshZoom = false;
        private static float zoom = 1;
        public static float Zoom
        {
            get { return zoom; }
            set
            {
                if (zoom != value) zoom = value;
                needRefreshZoom = true;
            }
        }

        private static bool isEnable = false;
        public static void Enable2D()
        {
            if (isEnable)
                return;

            Reset2DMatrix();

            // Make sure depth testing and lighting are disabled for 2D rendering until we are finished rendering in 2D
            //GL.PushAttrib(AttribMask.DepthBufferBit | AttribMask.LightingBit | AttribMask.TextureBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Color4(1f, 1f, 1f, 1f);

            isEnable = true;
        }

        public static void Disable2D()
        {
            if (!isEnable)
                return;

            //GL.PopAttrib();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();

            isEnable = false;
        }

        public static void Reset2DMatrix()
        {
            EmptyMatrixStack();

            int[] iViewport = new int[4];
            GL.GetInteger(GetPName.Viewport, iViewport);

            int width = iViewport[2];
            int height = iViewport[3];

            // Save a copy of the projection matrix so that we can restore it 
            // when it's time to do 3D rendering again.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Set up the orthographic projection
            GL.Ortho(0, width, height, 0, -1.1, 1.1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Scale(Zoom, Zoom, 1);
        }

        public static void EmptyMatrixStack()
        {
            GL.MatrixMode(MatrixMode.Projection);
            int max;
            GL.GetInteger(GetPName.MaxProjectionStackDepth, out max);
            for (int i = 0; i < max; i++)
            {
                GL.PopMatrix();
                if (GL.GetError() == ErrorCode.StackUnderflow)
                    break;
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.GetInteger(GetPName.MaxModelviewStackDepth, out max);
            for (int i = 0; i < max; i++)
            {
                GL.PopMatrix();
                if (GL.GetError() == ErrorCode.StackUnderflow)
                    break;
            }
        }

        private static int listIndex = int.MaxValue;
        public static void Draw2DSprite()
        {
            if (listIndex == int.MaxValue)
            {
                listIndex = GL.GenLists(1);
                GL.NewList(listIndex, ListMode.Compile);
                GL.Begin(PrimitiveType.QuadStrip);
                GL.TexCoord3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(0f, 0f, 0.0f);
                GL.TexCoord3(0.0f, 1.0f, 0.0f);
                GL.Vertex3(0f, 1f, 0.0f);
                GL.TexCoord3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(1f, 0f, 0.0f);
                GL.TexCoord3(1.0f, 1.0f, 0.0f);
                GL.Vertex3(1f, 1f, 0.0f);
                GL.End();
                GL.EndList();
            }
            GL.CallList(listIndex);
        }
        public static void Draw2DSprite(int x, int y, int width, int height, float depth = 0, bool centered = false)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            if (centered)
                GL.Translate(x - width / 2.0, y - height / 2.0, depth);
            else
                GL.Translate(x, y, depth);
            GL.Scale(width, height, 1);

            Draw2DSprite();

            GL.PopMatrix();
        }

        public static void Update()
        {
            if (needRefreshZoom)
                Reset2DMatrix();
        }
    }
}