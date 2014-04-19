using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gui.Controls;

namespace Gui.Screens
{
    public abstract class Screen
    {
        public enum States
        {
            Sleeping,
            Opening,
            Opened,
            Closing,
            FullyClosed
        };

        public ScreenManager Manager { get; private set; }
        //Transition part
        public States State { get; protected set; }
        public Dictionary<string, Control> Controls { get; protected set; }
        public Transition OpeningTransition;
        public Transition ClosingTransition;

        public Screen(ScreenManager manager)
        {
            Manager = manager;
            State = States.Sleeping;
            Controls = new Dictionary<string, Control>();
        }

        public virtual void Open()
        {
            State = States.Opening;
            OpeningTransition.Start();
        }
        public virtual void Close()
        {
            State = States.Closing;
            ClosingTransition.Start();
        }

        public void Resize(int width, int height)
        {
            
        }

        public virtual void Update(TimeSpan elapsed, bool isInForeground)
        {
            switch (State)
            {
                case States.Sleeping: return;
                case States.Opening:
                    {
                        if (OpeningTransition.ActualState == Transition.States.Finish)
                            State = States.Opened;
                        break;
                    }
                case States.Opened: break;
                case States.Closing:
                    {
                        if (ClosingTransition.ActualState == Transition.States.Finish)
                            State = States.FullyClosed;
                        break;
                    }
                case States.FullyClosed: return;
            }
            if (isInForeground)
                foreach (Control c in Controls.Values)
                    c.Update(elapsed);
        }
        public virtual void Draw(TimeSpan elapsed, bool isInForeground)
        {
            if (State == States.Opening)
                OpeningTransition.Update(elapsed);
            else if (State == States.Closing)
                ClosingTransition.Update(elapsed);
        }
        public virtual void DrawControls(TimeSpan elapsed, bool isInForeground)
        {
            if (State != States.Sleeping && State != States.FullyClosed)
                foreach (Control c in Controls.Values)
                    c.Draw(elapsed);
        }
        protected void ApplyTransitionTransformation()
        {
            if (State == States.Opening || State == States.Opened || State == States.Sleeping)
                OpeningTransition.ApplyTransformation(this);
            else if (State == States.Closing || State == States.FullyClosed)
                ClosingTransition.ApplyTransformation(this);
        }
        protected void UndoTransitionTransformation()
        {
            if (State == States.Opening)
                OpeningTransition.UndoTransformation(this);
            else if (State == States.Closing)
                ClosingTransition.UndoTransformation(this);
        }

        public virtual void MouveMoveEvent(object sender, GuiMouseMoveEventArgs e)
        {
            if (State == States.Opened)
                foreach (Control c in Controls.Values)
                    c.MouseMoveEvent(sender, e);
        }
        public virtual void MouseButtonUpEvent(object sender, GuiMouseButtonEventArgs e)
        {
            if (State == States.Opened)
                foreach (Control c in Controls.Values)
                    c.MouseButtonUpEvent(sender, e);
        }
        public virtual void MouseButtonDownEvent(object sender, GuiMouseButtonEventArgs e)
        {
            if (State == States.Opened)
                foreach (Control c in Controls.Values)
                    c.MouseButtonDownEvent(sender, e);
        }
        public void MouseWheelEvent(object sender, GuiMouseWheelEventArgs e)
        {
            if (State == States.Opened)
                foreach (Control c in Controls.Values)
                    c.MouseWheelEvent(sender, e);
        }
    }
}
