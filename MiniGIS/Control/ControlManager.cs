using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Control
{
    public class ControlManager : MapControl
    {
        public static ControlManager instance;

        IDictionary<ControlIndex, MapControl> pool;
        MapControl cur;

        public void Set(ControlIndex type)
        {
            cur = pool[type];
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseDown(sender, e);
        }
        public void MouseUp(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseUp(sender, e);
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseMove(sender, e);
        }
        public void MouseWheel(object sender, MouseEventArgs e)
        {
            if (cur != null) cur.MouseWheel(sender, e);
        }

        public ControlManager()
        {
            instance = this;
            pool = new Dictionary<ControlIndex, MapControl>
            {
                [ControlIndex.Viewer] = new Viewer()
            };
            Set(ControlIndex.Viewer);
        }
    }

    public enum ControlIndex
    {
        Viewer = 0,
    }
}
