using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Layer;

namespace MiniGIS.Control
{
    public class ControlManager : MapSingle<ControlManager>
    {
        MapControl[] pool;
        MapControl cur;
        MouseEventArgs cursor = null;

        public void Set(int index)
        {
            if (cur != null) cur.Unload();
            cur = pool[index];
            cur.Load();
        }

        public override void Render(ViewPort port, Graphics canvas)
        {
            if (cur != null) cur.Render(port, canvas);
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseDown(sender, e);
        }
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseUp(sender, e);
        }
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (cur != null)
            {
                cur.MouseMove(sender, e);
                MainForm.port.RenderTop();
            }
        }
        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseWheel(sender, e);
        }
        public override void MouseIn(object sender, EventArgs e)
        {
            if (cur != null) cur.MouseIn(sender, e);
        }
        public override void MouseOut(object sender, EventArgs e)
        {
            if (cur != null) cur.MouseOut(sender, e);
        }

        public ControlManager()
        {
            pool = new MapControl[]
            {
                MapViewer.Instance,
                DataExplorer.Instance,
            };
        }
    }
}
