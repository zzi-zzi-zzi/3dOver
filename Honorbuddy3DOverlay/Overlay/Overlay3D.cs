using System;
using RB3DOverlay.Events;

namespace RB3DOverlay.Overlay
{
    public static class Overlay3D

    {

        public static event EventHandler<DrawingEventArgs> Drawing;



        public static void InvokeDrawing(DrawingEventArgs args)

        {

            Drawing?.Invoke(null, args);

        }

    }
}