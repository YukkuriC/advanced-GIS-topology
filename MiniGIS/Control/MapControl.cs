using System.Windows.Forms;

namespace MiniGIS.Control
{
    public interface MapControl
    {
        void MouseDown(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseWheel(object sender, MouseEventArgs e);
    }
}
