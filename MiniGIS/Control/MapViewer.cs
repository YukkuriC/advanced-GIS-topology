using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MiniGIS.Data;

namespace MiniGIS.Control
{
    class MapViewer : MapSingle<MapViewer>
    {
        public override string DefaultText() => "拖动鼠标左键移动查看位置；上下拖动鼠标右键/使用鼠标滚轮改变缩放倍率";

        Vector2 lastScreen, lastWorld;
        bool dragging = false, dragging2 = false;
        double zoomLevel;

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (dragging2) return;
                    dragging = true;
                    lastScreen = new Vector2(e.X, e.Y);
                    lastWorld = MainForm.port.center;
                    break;

                case MouseButtons.Right:
                    if (dragging) return;
                    dragging2 = true;
                    lastScreen = new Vector2(e.X, e.Y);
                    zoomLevel = MainForm.port.zoom;
                    break;

                case MouseButtons.None: // 默认记录初始
                    lastScreen = new Vector2(e.X, e.Y);
                    lastWorld = MainForm.port.center;
                    break;
            }
        }

        // 释放拖拽模式
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MouseMove(sender, e);
                    dragging = false;
                    break;
                case MouseButtons.Right:
                    MouseMove(sender, e);
                    dragging2 = false;
                    zoomLevel = MainForm.port.zoom;
                    break;
            }
        }

        // (拖拽模式下)移动中心点、平滑缩放
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                double dx = (lastScreen.X - e.X) / MainForm.port.zoom,
                      dy = -(lastScreen.Y - e.Y) / MainForm.port.zoom; // 屏幕坐标向下为正
                MainForm.port.center = new Vector2(
                    lastWorld.X + dx,
                    lastWorld.Y + dy
                );
                MainForm.port.Render(true);
            }
            else if (dragging2)
            {
                double dDrag = (lastScreen.Y - e.Y) / 100;
                MainForm.port.zoom = (float)(zoomLevel * Math.Pow(2, dDrag));
                MainForm.port.Render(true);
            }
        }

        // (非拖拽模式下)以鼠标位置为基准缩放
        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            if (dragging || dragging2) return;

            // 更新缩放等级
            zoomLevel = Math.Log(MainForm.port.zoom, 2);
            if (e.Delta > 0) zoomLevel += 1;
            else zoomLevel -= 1;
            //zoomLevel = Math.Max(Math.Min(zoomLevel, 10), -10);
            float newZoom = (float)Math.Pow(2, zoomLevel);

            // 计算位置偏移
            Vector2 oldCenter = MainForm.port.center;
            MainForm.port.WorldCoord(e.X, e.Y, out double oldX, out double oldY);
            MainForm.port.zoom = newZoom;
            MainForm.port.WorldCoord(e.X, e.Y, out double newX, out double newY);

            // 更新窗口位置
            MainForm.port.center.X += oldX - newX;
            MainForm.port.center.Y += oldY - newY;
            MainForm.port.Render(true);
        }
    }
}
