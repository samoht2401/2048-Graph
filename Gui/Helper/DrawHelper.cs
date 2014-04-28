using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Gui.Bounds;

namespace Gui.Helper
{
    public static class DrawHelper
    {
        private static int[] iViewport;
        public static int ViewportWidth { get { return iViewport[2]; } }
        public static int ViewportHeight { get { return iViewport[3]; } }
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
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusDstAlpha);
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

            iViewport = new int[4];
            GL.GetInteger(GetPName.Viewport, iViewport);

            int width = iViewport[2];
            int height = iViewport[3];
            float windowRatio = (float)width / height;
            int xOffset = 0;
            int yOffset = 0;
            float ratio = 16 / 9f;
            if (windowRatio < ratio)
                yOffset = (int)((height - width / ratio) / 2 * (1920f / width));
            else
                xOffset = (int)((width - height * ratio) / 2 * (1080f / height));

            // Save a copy of the projection matrix so that we can restore it 
            // when it's time to do 3D rendering again.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Set up the orthographic projection
            GL.Ortho(-xOffset, xOffset + 1920, yOffset + 1080, -yOffset, -1.2, 1.2);

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

        private static Dictionary<Vector4, int> listsIndex = new Dictionary<Vector4, int>();
        private static readonly Vector4 fullTexCoordRect = Vector4.UnitZ + Vector4.UnitW;
        private static float depthIterator = 0.000001f;
        private static void Draw2DSprite(Vector4 texCoordRect)
        {
            if (!listsIndex.ContainsKey(texCoordRect))
            {
                int index = GL.GenLists(1);
                listsIndex.Add(texCoordRect, index);
                GL.NewList(index, ListMode.Compile);
                GL.Begin(PrimitiveType.QuadStrip);
                GL.TexCoord3(texCoordRect.X, texCoordRect.Y, 0.0f);
                GL.Vertex3(0f, 0f, 0.0f);
                GL.TexCoord3(texCoordRect.X, texCoordRect.W, 0.0f);
                GL.Vertex3(0f, 1f, 0.0f);
                GL.TexCoord3(texCoordRect.Z, texCoordRect.Y, 0.0f);
                GL.Vertex3(1f, 0f, 0.0f);
                GL.TexCoord3(texCoordRect.Z, texCoordRect.W, 0.0f);
                GL.Vertex3(1f, 1f, 0.0f);
                GL.End();
                GL.EndList();
            }
            GL.CallList(listsIndex[texCoordRect]);
        }
        public static void Draw2DSprite(int x, int y, int width, int height, float rotate = 0, float depth = 0, bool centered = false)
        {
            Draw2DSprite(x, y, width, height, fullTexCoordRect, rotate, depth, centered);
        }
        public static void Draw2DSprite(int x, int y, int width, int height, Vector4 texCoordRect, float rotate = 0, float depth = 0, bool centered = false)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            GL.Translate(x, y, depth + depthIterator);
            GL.Rotate(rotate, Vector3.UnitZ);
            if (centered)
                GL.Translate(-width / 2, -height / 2, 0);
            GL.Scale(width, height, 1);

            Draw2DSprite(texCoordRect);

            GL.PopMatrix();
            depthIterator += 0.0000001f;
        }

        private static int borderMaskList = -1;
        private static readonly float bigNumber = 10000f;
        public static void DrawBorderMask()
        {
            if (borderMaskList == -1)
            {
                borderMaskList = GL.GenLists(1);
                GL.NewList(borderMaskList, ListMode.Compile);
                GL.Color4(1, 0, 0, 1);
                GL.Disable(EnableCap.Texture2D);
                GL.Disable(EnableCap.Blend);
                GL.Begin(PrimitiveType.QuadStrip);
                GL.Vertex3(-bigNumber, -bigNumber, 1.1f);
                GL.Vertex3(0, 0, 1.1f);
                GL.Vertex3(1920, 0, 1.1f);
                GL.Vertex3(bigNumber, -bigNumber, 1.1f);
                GL.Vertex3(1920, 0, 1.1f);
                GL.Vertex3(bigNumber, -bigNumber, 1.1f);
                GL.Vertex3(bigNumber, bigNumber, 1.1f);
                GL.Vertex3(1920, 1080, 1.1f);
                GL.Vertex3(bigNumber, bigNumber, 1.1f);
                GL.Vertex3(1920, 1080, 1.1f);
                GL.Vertex3(0, 1080, 1.1f);
                GL.Vertex3(-bigNumber, bigNumber, 1.1f);
                GL.Vertex3(0, 1080, 1.1f);
                GL.Vertex3(-bigNumber, bigNumber, 1.1f);
                GL.Vertex3(-bigNumber, -bigNumber, 1.1f);
                GL.Vertex3(0, 0, 1.1f);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.EndList();
          }
            GL.CallList(borderMaskList);
        }

        public static RectangleBound GetBoundRelativeOnScreen(float xRel, float yRel, int width, int height)
        {
            return RectangleBound.New((int)((1920 - width) * xRel), (int)((1080 - height) * yRel), width, height);
        }

        public static void Update()
        {
            if (needRefreshZoom)
                Reset2DMatrix();
        }
    }
}
