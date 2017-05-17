using System;
using RB3DOverlay.Interface;

namespace RB3DOverlay.Events
{
    public class DrawingEventArgs : EventArgs
    {
        public DrawingEventArgs(I3DDrawer drawer)

        {

            Drawer = drawer;

        }



        public I3DDrawer Drawer { get; }
    }
}