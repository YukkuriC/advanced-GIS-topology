using MiniGIS.Render;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniGIS.Control
{
    public abstract class MapControl
    {
        public virtual string DefaultText() => "";
        public virtual void Render(ViewPort port, Graphics canvas) { }
        public virtual void MouseDown(object sender, MouseEventArgs e) { }
        public virtual void MouseUp(object sender, MouseEventArgs e) { }
        public virtual void MouseMove(object sender, MouseEventArgs e) { }
        public virtual void MouseWheel(object sender, MouseEventArgs e) { }
        public virtual void MouseIn(object sender, EventArgs e) { }
        public virtual void MouseOut(object sender, EventArgs e) { MainForm.instance.labelInfo.Text = DefaultText(); } // 离开画布时显示默认文字
        public virtual void Load() => MouseOut(null, null);
        public virtual void Unload() { }
    }

    public abstract class MapSingle<SELF> : MapControl where SELF : new()
    {
        static SELF instance;
        public static SELF Instance
        {
            get
            {
                if (instance == null) instance = new SELF();
                return instance;
            }
        }
    }
}
