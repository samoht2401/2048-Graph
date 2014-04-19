using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gui.Bounds;
using Gui.Helper;
using Gui.Sprites;
using OpenTK.Input;

namespace Gui.Controls
{
    public interface HandlableEvent
    {
        bool Handled { get; set; }
    }
    public class GuiMouseMoveEventArgs : MouseMoveEventArgs, HandlableEvent
    {
        public GuiMouseMoveEventArgs(MouseMoveEventArgs s) : base(s) { }
        public bool Handled { get; set; }
    }
    public class GuiMouseButtonEventArgs : MouseButtonEventArgs, HandlableEvent
    {
        public GuiMouseButtonEventArgs(MouseButtonEventArgs s) : base(s) { }
        public bool Handled { get; set; }
    }
    public class GuiMouseWheelEventArgs : MouseWheelEventArgs, HandlableEvent
    {
        public GuiMouseWheelEventArgs(MouseWheelEventArgs s) : base(s) { }
        public bool Handled { get; set; }
    }

    public abstract class Control
    {
        public string Name { get; protected set; }
        public Control Parent { get; protected set; }
        public Dictionary<string, Control> Childs { get; protected set; }
        public Bound Bound { get; protected set; }
        public CompoundSprite Sprites { get; protected set; }
        public bool HasFocus { get; protected set; }
        public bool ChildHasFocus { get; protected set; }

        protected bool wasMouseInside;

        public delegate void GuiMouseMoveEventHandler(object sender, GuiMouseMoveEventArgs e);
        public event GuiMouseMoveEventHandler MouseEnter;
        public event GuiMouseMoveEventHandler MouseLeave;
        public event GuiMouseMoveEventHandler MouseMove;
        public delegate void GuiMouseButtonEventHandler(object sender, GuiMouseButtonEventArgs e);
        public event GuiMouseButtonEventHandler MouseButtonUp;
        public event GuiMouseButtonEventHandler MouseButtonDown;
        public delegate void GuiMouseWheelEventHandler(object sender, GuiMouseWheelEventArgs e);
        public event GuiMouseWheelEventHandler MouseWheel;

        public Control(CompoundSprite sprites)
        {
            Childs = new Dictionary<string, Control>();
            Sprites = sprites;
        }

        public void MouseMoveEvent(object sender, GuiMouseMoveEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
            if (Bound.Intersect(InputHelper.MouseOpenGLPosition))
            {
                if (!wasMouseInside)
                {
                    wasMouseInside = true;
                    if (MouseEnter != null)
                        MouseEnter(this, e);
                }
            }
            else
            {
                if (wasMouseInside)
                {
                    wasMouseInside = false;
                    if (MouseLeave != null)
                        MouseLeave(this, e);
                }
            }
            foreach (Control c in Childs.Values)
                c.MouseMoveEvent(sender, e);
        }
        public void MouseButtonUpEvent(object sender, GuiMouseButtonEventArgs e)
        {
            if (e.Handled)
                return;
            foreach (Control c in Childs.Values)
            {
                c.MouseButtonUpEvent(sender, e);
                if (e.Handled)
                    return;
            }
            if (Bound.Intersect(InputHelper.MouseOpenGLPosition) && MouseButtonUp != null)
                MouseButtonUp(this, e);
        }
        public void MouseButtonDownEvent(object sender, GuiMouseButtonEventArgs e)
        {
            if (e.Handled)
                return;
            foreach (Control c in Childs.Values)
            {
                c.MouseButtonDownEvent(sender, e);
                if (e.Handled)
                    return;
            }
            if (Bound.Intersect(InputHelper.MouseOpenGLPosition) && MouseButtonDown != null)
                MouseButtonDown(this, e);
        }
        public void MouseWheelEvent(object sender, GuiMouseWheelEventArgs e)
        {
            if (e.Handled)
                return;
            foreach (Control c in Childs.Values)
            {
                c.MouseWheelEvent(sender, e);
                if (e.Handled)
                    return;
            }
            if (Bound.Intersect(InputHelper.MouseOpenGLPosition) && MouseWheel != null)
                MouseWheel(this, e);
        }

        public virtual void Update(TimeSpan elapsed) { }
        public virtual void Draw(TimeSpan elapsed) { }
        public virtual void UpdateChilds(TimeSpan elapsed)
        {
            foreach (Control c in Childs.Values)
                Update(elapsed);
        }
        public virtual void DrawChilds(TimeSpan elapsed)
        {
            foreach (Control c in Childs.Values)
                Draw(elapsed);
        }
    }
}
