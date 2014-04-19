using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Gui.Bounds;

namespace Gui.Helper
{
    public static class InputHelper
    {
        public static KeyboardDevice Keyboard { get; private set; }
        public static MouseDevice Mouse { get; private set; }
        public static Point MouseScreenPosition { get { return Point.New(Mouse.X, Mouse.Y); } }
        public static Point MouseOpenGLPosition { get { return ConvertScreenToWorldCoords(Mouse.X, Mouse.Y); } }

        public static void Init(KeyboardDevice keyboard, MouseDevice mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
        }

        public static Point ConvertScreenToWorldCoords(int x, int y)
        {
            int[] viewport = new int[4];
            Matrix4 modelViewMatrix, projectionMatrix;
            GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionMatrix);
            GL.GetInteger(GetPName.Viewport, viewport);
            Vector2 mouse;
            mouse.X = x;
            mouse.Y = y;
            Vector4 vector = UnProject(projectionMatrix, modelViewMatrix, Point.New(viewport[2], viewport[3]), mouse);
            Point coords = Point.New((int)vector.X, (int)vector.Y);
            return coords;
        }
        public static Vector4 UnProject(Matrix4 projection, Matrix4 view, Point viewport, Vector2 mouse)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / (float)viewport.X - 1;
            vec.Y = 2.0f * mouse.Y / (float)viewport.Y - 1;
            vec.Z = 0;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);
            projInv.M22 = -projInv.M22;

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec;
        }

        /*public static void TransposeMouseCoordToGL()
        {
            Mouse.
        }*/
    }
}
