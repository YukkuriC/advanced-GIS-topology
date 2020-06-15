using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Algorithm;
using MiniGIS.Data;

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
        public PointF ScreenCoord(Vector2 v) => ScreenCoord(v.X, v.Y);
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
        public void Render(bool updateText = false)
        {
            if (target.Width <= 0 || target.Height <= 0) return;
            Bitmap bmp = new Bitmap(target.Width, target.Height);
            Graphics canvas = Graphics.FromImage(bmp);
            canvas.Clear(Color.FromArgb(unchecked((int)0xff66ccff)));
            foreach (Layer l in (from l in layers where l.Visible select l).Reverse()) l.Render(this, canvas); // 列表首元素绘制于顶层 
            target.Image = bmp;
            if (updateText) ShowText();
        }

        // 
        public void ShowText()
        {
            string text = String.Format("坐标: ({0}, {1}); 缩放等级: {2}",
                Utils.SciString(center.X, 6), Utils.SciString(center.Y, 6),
                Utils.SciString(zoom, 2)
            );
            MainForm.instance.labelInfo.Text = text;
        }

        // 聚焦至区域
        public void Focus(float xMin, float yMin, float xMax, float yMax)
        {
            center = new PointF((xMin + xMax) / 2, (yMin + yMax) / 2);
            bool validZoom = false;
            float newZoom = float.MaxValue;
            if (xMin < xMax && target.Width > 0)
            {
                validZoom = true;
                newZoom = Math.Min(newZoom, target.Width / (xMax - xMin));
            }
            if (yMin < yMax && target.Height > 0)
            {
                validZoom = true;
                newZoom = Math.Min(newZoom, target.Height / (yMax - yMin));
            }
            if (validZoom) zoom = newZoom;
            Render(true);
        }
        public void Focus(Rect rect) => Focus((float)rect.xMin, (float)rect.yMin, (float)rect.xMax, (float)rect.yMax);

        public void Reset()
        {
            zoom = 1;
            center = new PointF(0, 0);
            Render(true);
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
