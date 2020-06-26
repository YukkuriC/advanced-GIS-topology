using MiniGIS.Data;
using MiniGIS.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Control
{
    public static class GeneralControl
    {
        // 根据滚轮以鼠标位置为基准缩放
        public static void WheelScale(MouseEventArgs e)
        {
            // 更新缩放等级
            double zoomLevel = Math.Log(MainForm.port.zoom, 2);
            if (e.Delta > 0) zoomLevel += 1;
            else zoomLevel -= 1;
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
