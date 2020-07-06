using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Algorithm;
using MiniGIS.Data;
using MiniGIS.Layer;

namespace MiniGIS
{
    public class ViewPort
    {
        public static ViewPort instance;

        #region prop

        public IEnumerable<BaseLayer> layers;
        public PictureBox target;
        public Vector2 center;
        public double zoom = 1;

        #endregion

        #region method

        // 世界坐标换算为屏幕坐标
        public Vector2 ScreenCoord(Vector2 v) => ScreenCoord(v.X, v.Y);
        public Vector2 ScreenCoord(double worldX, double worldY)
        {
            return new Vector2(
                (target.Width / 2.0 + zoom * (worldX - center.X)),
                (target.Height / 2.0 - zoom * (worldY - center.Y))
            );
        }

        // 屏幕坐标换算为世界坐标
        public void WorldCoord(double screenX, double screenY, out double worldX, out double worldY)
        {
            worldX = center.X + (screenX - target.Width / 2f) / zoom;
            worldY = center.Y - (screenY - target.Height / 2f) / zoom;
        }
        public Vector2 WorldCoord(double screenX, double screenY)
        {
            WorldCoord(screenX, screenY, out var x, out var y);
            return new Vector2(x, y);
        }

        // 渲染所有图层
        public void Render(bool updateText = false)
        {
            if (target.Width <= 0 || target.Height <= 0) return;
            Bitmap bmp = new Bitmap(target.Width, target.Height);
            Graphics canvas = Graphics.FromImage(bmp);
            canvas.Clear(Color.FromArgb(unchecked((int)0xff66ccff)));
            foreach (BaseLayer l in (from l in layers where l.Visible select l).Reverse()) l.Render(this, canvas); // 列表首元素绘制于顶层 
            target.BackgroundImage = bmp;
            if (updateText) ShowText();
            RenderTop();
        }

        // 渲染控制工具辅助线
        public void RenderTop()
        {
            if (target.Width <= 0 || target.Height <= 0 || MainForm.controlManager == null) return;
            Bitmap bmp = new Bitmap(target.Width, target.Height);
            Graphics canvas = Graphics.FromImage(bmp);
            MainForm.controlManager.Render(this, canvas);
            target.Image = bmp;
        }

        // 显示位置&缩放文字
        public void ShowText()
        {
            string text = String.Format("中心坐标: ({0}, {1}); 缩放等级: {2}",
                Utils.SciString(center.X, 6), Utils.SciString(center.Y, 6),
                Utils.SciString(zoom, 2)
            );
            MainForm.instance.labelInfo.Text = text;
        }

        // 聚焦至区域
        public void Focus(double xMin, double yMin, double xMax, double yMax)
        {
            center = new Vector2((xMin + xMax) / 2, (yMin + yMax) / 2);
            bool validZoom = false;
            double newZoom = double.MaxValue;
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
            Render();
        }
        public void Focus(Rect rect) => Focus((float)rect.XMin, (float)rect.YMin, (float)rect.XMax, (float)rect.YMax);

        public void Reset()
        {
            zoom = 1;
            center = new Vector2();
            Render();
        }

        #endregion

        public ViewPort(PictureBox _target, IEnumerable<BaseLayer> _layers)
        {
            ViewPort.instance = this;
            target = _target;
            layers = _layers;
            Reset();
        }
    }
}
