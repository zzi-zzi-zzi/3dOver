using System.Drawing;
using Clio.Utilities;

namespace RB3DOverlay.Interface
{
    public interface I3DDrawer
    {

        void DrawLine(Vector3 start, Vector3 end, Color color);

        void DrawTriangles(Vector3[] verts, Color color);

        void DrawTriangleFan(Vector3[] poly, int index, int count, Color color);

        void DrawBox(Vector3 center, Vector3 extents, Color color);

        void DrawBoxOutline(Vector3 center, Vector3 extents, Color color);

        void DrawCircleOutline(Vector3 center, float radius, Color color);
    }
}