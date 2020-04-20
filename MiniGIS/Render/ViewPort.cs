using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MiniGIS.Render
{
    public class ViewPort
    {
        public static ViewPort instance;

        #region prop

        public IEnumerable<Layer> layers;
        public PictureBox target;
        public PointF center;
        public float zoom = 1;

        #endregion

        #region method

        // 世界坐标换算为屏幕坐标
        public PointF ScreenCoord(double worldX, double worldY)
        {
            return new PointF(
                (float)(target.Width / 2f + zoom * (worldX - center.X)),
                (float)(target.Height / 2f - zoom * (worldY - center.Y))
            );
        }

        // 屏幕坐标换算为世界坐标
        public void WorldCoord(float screenX, float screenY, out float worldX, out float worldY)
        {
            worldX = center.X + (screenX - target.Width / 2f) / zoom;
            worldY = center.Y - (screenY - target.Height / 2f) / zoom;
        }

        // 渲染所有图层
        public void Render()
        {
            if (target.Width <= 0 || target.Height <= 0) return;
            Bitmap bmp = new Bitmap(target.Width, target.Height);
            Graphics canvas = Graphics.FromImage(bmp);
            canvas.Clear(Color.FromArgb(unchecked((int)0xff66ccff)));
            foreach (Layer l in (from l in layers where l.visible select l)) l.Render(this, canvas);
            target.Image = bmp;
        }

        public void Reset()
        {
            zoom = 1;
            center = new PointF(0, 0);
            Render();
        }

        #endregion

        public ViewPort(PictureBox _target, IEnumerable<Layer> _layers)
        {
            ViewPort.instance = this;
            target = _target;
            layers = _layers;
            Reset();
        }
    }
}
